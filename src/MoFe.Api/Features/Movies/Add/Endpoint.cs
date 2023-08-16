using FastEndpoints;
using MoFe.Contracts.Features.Movies.Add;

namespace MoFe.Api.Features.Movies.Add;

public class Endpoint : Endpoint<Request, Response, Mapper>
{
    public Endpoint()
    {
    }
    
    public override void Configure()
    {
        Post("/movies");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var movie = Map.ToEntity(req);
        await SendAsync(Map.FromEntity(movie), cancellation: ct);
    }
}