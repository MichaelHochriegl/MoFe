using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using MoFe.Contracts.Features.Movies;
using MoFe.Persistence;
using MoFe.Persistence.Entities.Movies;

namespace MoFe.Api.Features.Movies;

public class AddMovie : Endpoint<AddMovieRequest, AddMovieResponse, AddMovieMapper>
{
    private readonly MoFeDbContext _dbContext;

    public AddMovie(MoFeDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override void Configure()
    {
        Post("/movies");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddMovieRequest req, CancellationToken ct)
    {
        var isMovieAlreadyPresent =
            await _dbContext.Movies.AnyAsync(m => m.Name == req.MovieName && m.ReleaseDate == req.ReleaseDate,
                cancellationToken: ct).ConfigureAwait(false);
        
        if (isMovieAlreadyPresent)
        {
            AddError($"Movie '{req.MovieName} ({req.ReleaseDate})' already present!");
        }
        ThrowIfAnyErrors();

        var movie = Map.ToEntity(req);
        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync(ct).ConfigureAwait(false);

        await SendCreatedAtAsync<GetMovieEndpoint>(movie.Id, Map.FromEntity(movie), cancellation: ct).ConfigureAwait(false);
    }
}

public class AddMovieMapper : Mapper<AddMovieRequest, AddMovieResponse, Movie>
{
    public override AddMovieResponse FromEntity(Movie e) => new(e.Id);

    public override Movie ToEntity(AddMovieRequest r) => new() { Name = r.MovieName, ReleaseDate = r.ReleaseDate };
}