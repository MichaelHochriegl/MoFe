using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using MoFe.Contracts.Features.Movies;
using MoFe.Persistence;
using MoFe.Persistence.Entities.Movies;

namespace MoFe.Api.Features.Movies;

// TODO: look at how to properly implement: sorting, filtering and pagination for this endpoint
public class GetAllMovies : EndpointWithoutRequest<IEnumerable<GetAllMovieResponse>, GetMoviesMapper>
{
    private readonly MoFeDbContext _dbContext;

    public GetAllMovies(MoFeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/movies");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var movies = await _dbContext.Movies.AsNoTracking().ToListAsync(cancellationToken: ct);
        await SendOkAsync(Map.FromEntity(movies), ct).ConfigureAwait(false);
    }
}

public class GetMoviesMapper : ResponseMapper<IEnumerable<GetAllMovieResponse>, IEnumerable<Movie>>
{
    public override IEnumerable<GetAllMovieResponse> FromEntity(IEnumerable<Movie> e) =>
        e.Select(m => new GetAllMovieResponse(m.Id, m.Name, m.ReleaseDate));
}