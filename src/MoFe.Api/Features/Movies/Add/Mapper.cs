using FastEndpoints;
using MoFe.Contracts.Features.Movies.Add;
using MoFe.Persistence.Entities.Movies;

namespace MoFe.Api.Features.Movies.Add;

public class Mapper : Mapper<Request, Response, Movie>
{
    public override Response FromEntity(Movie e) => new(e.Id);

    public override Movie ToEntity(Request r) => new() { Name = r.MovieName, ReleaseDate = r.ReleaseDate };
}