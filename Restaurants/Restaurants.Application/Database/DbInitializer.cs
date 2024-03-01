using Dapper;

namespace Restaurants.Application.Database;

public class DbInitializer(IDbConnectionFactory dbConnectionFactory)
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync("""
            create table if not exists restaurants (
            id uuid primary key,
            name text not null,
            yearstarted integer not null);
        """);

        await connection.ExecuteAsync("""
            create table if not exists features (
            restaurantid uuid references restaurants (id),
            name text not null);
            """);

        await connection.ExecuteAsync("""
            create table if not exists ratings (
            userid uuid,
            restaurantid UUID references restaurants (id),
            rating integer not null,
            primary key (userid, restaurantid));
            """);
    }
}