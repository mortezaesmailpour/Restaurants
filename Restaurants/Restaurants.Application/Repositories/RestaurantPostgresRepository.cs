using Dapper;
using Restaurants.Application.Database;
using Restaurants.Application.Models;

namespace Restaurants.Application.Repositories;

public class RestaurantPostgresRepository(IDbConnectionFactory dbConnectionFactory) : IRestaurantRepository
{
    public async Task<bool> CreateAsync(Restaurant restaurant, CancellationToken token = default)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            insert into restaurants (id, name, yearstarted) 
            values (@Id, @Name, @YearStarted) 
            """, restaurant));

        if (result > 0 )
            foreach ( var feature in restaurant.Features )
                await connection.ExecuteAsync(new CommandDefinition("""
                    insert into features (restaurantId, name) 
                    values (@RestaurantId, @Name)
                    """, new { RestaurantId = restaurant.Id, Name = feature }));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<Restaurant?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);

        var restaurant = await connection.QuerySingleOrDefaultAsync<Restaurant>(new CommandDefinition("""
            select * from restaurants where id = @Id
            """, new { Id = id }));

        if (restaurant is null)
            return null;

        var features = await connection.QueryAsync<string>(new CommandDefinition("""
            select name from features where restaurantId = @Id
            """, new { Id = id }));
        
        foreach (var feature in features)
            restaurant.Features.Add(feature);

        return restaurant;
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync(CancellationToken token = default)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);

        var result = await connection.QueryAsync(new CommandDefinition("""
            select r.*, string_agg(f.name , ',') as features
            from restaurants r left join features f on f.restaurantId = r.id
            group by id
            """));

        return result.Select(x => new Restaurant()
        {
            Id = x.id,
            Name = x.name,
            YearStarted = x.yearstarted,
            Features = Enumerable.ToList(x.features.Split(',')),
        });

    }

    public async Task<bool> UpdateAsync(Restaurant restaurant, CancellationToken token = default)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            update restaurants set name = @Name, yearstarted = @YearStarted
            where id = @Id
            """, restaurant, cancellationToken: token));

        if (result > 0)
        {
            await connection.ExecuteAsync(new CommandDefinition("""
                delete from features where restaurantId = @id
                """, new { id = restaurant.Id }, cancellationToken: token));
            foreach (var feature in restaurant.Features)
                await connection.ExecuteAsync(new CommandDefinition("""
                    insert into features (restaurantId, name)
                    values (@RestaurantId, @Name)
                    """, new { RestaurantId = restaurant.Id, Name = feature }, cancellationToken: token));
        }

        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition("""
            delete from features where restaurantid = @id
            """, new { id }, cancellationToken: token));

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            delete from restaurants where id = @id
            """, new { id }, cancellationToken: token));

        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);

        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
            select count(1) from restaurants where id = @id
            """, new { Id = id }, cancellationToken: token));
    }
}