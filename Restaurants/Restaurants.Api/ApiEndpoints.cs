namespace Restaurants.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";
    public static class Restaurant
    {
        private const string Base = $"{ApiBase}/restaurants";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
        public const string Update = Get;
        public const string Delete = Get;

        public const string Rate = $"{Get}/ratings";
        public const string DeleteRating = Rate;
    }
    public static class Ratings
    {
        private const string Base = $"{ApiBase}/ratings";

        public const string GetUserRatings = $"{Base}/me";
    }
}