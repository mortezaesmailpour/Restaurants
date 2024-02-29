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
            id UUID primary key,
            name TEXT not null,
            yearstarted integer not null);
        """);

        await connection.ExecuteAsync("""
            create table if not exists features (
            restaurantId UUID references restaurants (id),
            name TEXT not null);
            """);
    }
}
