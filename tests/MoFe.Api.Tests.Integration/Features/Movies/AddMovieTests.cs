using System.Net;
using FastEndpoints;
using FluentAssertions;
using MoFe.Api.Features.Movies;
using MoFe.Contracts.Features.Movies;

namespace MoFe.Api.Tests.Integration.Features.Movies;

public class AddMovieTests : IClassFixture<CustomWebAppFac>
{
    private readonly CustomWebAppFac _factory;
    private readonly HttpClient _client;

    public AddMovieTests(CustomWebAppFac factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Adding_new_Movie_with_valid_data_should_succeed()
    {
        // Arrange
        var request = new AddMovieRequest("Ace Ventura", DateOnly.Parse("1994.02.04"));

        // Act
        var (resp, result) = await _client.POSTAsync<AddMovie, AddMovieRequest, AddMovieResponse>(request);

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }
}