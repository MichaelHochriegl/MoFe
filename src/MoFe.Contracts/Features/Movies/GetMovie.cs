namespace MoFe.Contracts.Features.Movies;

public record GetMovieRequest(int Id);

public record GetMovieResponse(int Id, string Name, DateOnly ReleaseDate);