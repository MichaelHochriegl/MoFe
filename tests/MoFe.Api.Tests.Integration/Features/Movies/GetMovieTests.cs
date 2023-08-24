using System.Net;
using FastEndpoints;
using FluentAssertions;
using MoFe.Api.Features.Movies;
using MoFe.Contracts.Features.Movies;
using MoFe.Persistence.Entities.Movies;

namespace MoFe.Api.Tests.Integration.Features.Movies;

public class GetMovieTests : IClassFixture<CustomWebAppFac>
{
    private readonly CustomWebAppFac _factory;
    private readonly HttpClient _client;

    public GetMovieTests(CustomWebAppFac factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Fetch_existing_Movie_should_succeed()
    {
        // Arrange
        var movie = await _factory.SeedEntry(new Movie()
        {
            Name = "Ace Ventura",
            ReleaseDate = DateOnly.Parse("1994.02.04")
        });

        var request = new GetMovieRequest(movie.Id);

        // Act
        var (resp, result) = await _client.GETAsync<GetMovieEndpoint, GetMovieRequest, GetMovieResponse>(request);

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeEquivalentTo(movie);
    }

    [Fact]
    public async Task Fetch_non_existing_Movie_should_fail()
    {
        // Arrange
        const int unknownMovieId = 999;
        var request = new GetMovieRequest(unknownMovieId);

        // Act
        var (resp, _) = await _client.GETAsync<GetMovieEndpoint, GetMovieRequest, GetMovieResponse>(request);

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);

    }
}