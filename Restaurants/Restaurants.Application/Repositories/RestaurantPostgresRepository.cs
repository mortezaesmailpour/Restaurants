using Dapper;
using Restaurants.Application.Database;
using Restaurants.Application.Models;

namespace Restaurants.Application.Repositories;

public class RestaurantPostgresRepository(IDbConnectionFactory dbConnectionFactory) : IRestaurantRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<bool> CreateAsync(Restaurant restaurant)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
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

    public async Task<Restaurant?> GetByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

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

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var restaurants = await connection.QueryAsync<Restaurant>(new CommandDefinition("""
            select * from restaurants
            """));
        foreach (var restaurant in restaurants)
        {
           var features = await connection.QueryAsync<string>(new CommandDefinition("""
            select name from features where restaurantId = @Id
            """, new { restaurant.Id }));

            foreach (var feature in features)
                restaurant.Features.Add(feature);
        }
        return restaurants;
        
        //var result = await connection.QueryAsync(new CommandDefinition("""
        //    select r.*, string_agg(f.name , ',') as features
        //    from restaurants r left join features f on f.restaurantId = r.id
        //    group by id
        //    """));
        //return result.Select(x => new Restaurant()
        //{
        //    Id = x.id,
        //    Name = x.name,
        //    YearStarted = x.yearstarted,
        //    Features = Enumerable.ToList(x.features.split(',')),
        //});

    }

    public async Task<bool> UpdateAsync(Restaurant restaurant)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            update restaurants set name = @Name, yearstarted = @YearStarted
            where id = @Id
            """, restaurant));
        if (result > 0)
        {
            await connection.ExecuteAsync(new CommandDefinition("""
                delete from features where restaurantId = @id
                """, new { id = restaurant.Id }));
            foreach (var feature in restaurant.Features)
            {
                await connection.ExecuteAsync(new CommandDefinition("""
                    insert into features (restaurantId, name)
                    values (@RestaurantId, @Name)
                    """, new { RestaurantId = restaurant.Id, Name = feature }));
            }
        }
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition("""
            delete from features where restaurantid = @id
            """, new { id }));

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            delete from restaurants where id = @id
            """, new { id }));

        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
            select count(1) from restaurants where id = @id
            """, new { Id = id }));
    }
}
