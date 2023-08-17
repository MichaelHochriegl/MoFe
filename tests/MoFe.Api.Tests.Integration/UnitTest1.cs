using System.Net;
using FastEndpoints;
using FluentAssertions;
using MoFe.Api.Features.Movies;
using MoFe.Contracts.Features.Movies;

namespace MoFe.Api.Tests.Integration;

public class UnitTest1 : IClassFixture<CustomWebAppFac>
{
    private readonly CustomWebAppFac _factory;
    private readonly HttpClient _client;

    public UnitTest1(CustomWebAppFac factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task Test1()
    {
        var request = new AddMovieRequest("test", DateOnly.Parse("2023.05.20"));

        var (resp, result) = await _client.POSTAsync<AddMovie, AddMovieRequest, AddMovieResponse>(request);

        resp.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterOrEqualTo(0);
    }
}