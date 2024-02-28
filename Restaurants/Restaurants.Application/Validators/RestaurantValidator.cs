using FluentValidation;
using Restaurants.Application.Models;

namespace Restaurants.Application.Validators;

public class RestaurantValidator : AbstractValidator<Restaurant>
{
    public RestaurantValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.YearStarted).LessThanOrEqualTo(DateTime.Now.Year);
    }
}
