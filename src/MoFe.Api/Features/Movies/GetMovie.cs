using FastEndpoints;
using MoFe.Contracts.Features.Movies;
using MoFe.Persistence;
using MoFe.Persistence.Entities.Movies;

namespace MoFe.Api.Features.Movies;

public class GetMovieEndpoint : Endpoint<GetMovieRequest, GetMovieResponse, GetMovieMapper>
{
    private readonly MoFeDbContext _dbContext;

    public GetMovieEndpoint(MoFeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/movies/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetMovieRequest req, CancellationToken ct)
    {
        var movie = await _dbContext.Movies.FindAsync(req.Id).ConfigureAwait(false);

        if (movie is null)
        {
            await SendNotFoundAsync(ct).ConfigureAwait(false);
            return;
        }

        await SendOkAsync(Map.FromEntity(movie), ct).ConfigureAwait(false);
    }
}

public class GetMovieMapper : Mapper<GetMovieRequest, GetMovieResponse, Movie>
{
    public override GetMovieResponse FromEntity(Movie e) => new(e.Id, e.Name, e.ReleaseDate);
}
