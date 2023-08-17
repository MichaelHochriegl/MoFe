using FluentValidation;

namespace MoFe.Contracts.Features.Movies;

public record AddMovieRequest(string MovieName, DateOnly ReleaseDate);

public record AddMovieResponse(int Id);

public class AddMovieValidator : AbstractValidator<AddMovieRequest>
{
    public AddMovieValidator()
    {
        RuleFor(x => x.MovieName)
            .NotEmpty();

        RuleFor(x => x.ReleaseDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.Parse("1900.01.01"));
    }
}