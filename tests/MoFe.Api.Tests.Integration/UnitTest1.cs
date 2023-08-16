using System.Net;
using FastEndpoints;
using FluentAssertions;
using MoFe.Api.Features.Movies.Add;
using MoFe.Contracts.Features.Movies.Add;

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
        var request = new Request("test", DateOnly.Parse("2023.05.20"));

        var (resp, result) = await _client.POSTAsync<Endpoint, Request, Response>(request);

        resp.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterOrEqualTo(0);
    }
}