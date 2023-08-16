using FluentValidation;

namespace MoFe.Contracts.Features.Movies.Add;

public record Request(string MovieName, DateOnly ReleaseDate);

public record Response(int Id);

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(x => x.MovieName)
            .NotEmpty();

        RuleFor(x => x.ReleaseDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.Parse("1900.01.01"));
    }
}