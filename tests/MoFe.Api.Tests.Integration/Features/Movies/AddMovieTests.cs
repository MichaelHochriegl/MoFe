using System.Net;
using FastEndpoints;
using FluentAssertions;
using MoFe.Api.Features.Movies;
using MoFe.Contracts.Features.Movies;
using MoFe.Persistence.Entities.Movies;

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
    
    [Fact]
    public async Task Adding_already_existing_Movie_should_fail()
    {
        // Arrange
        var movie = await _factory.SeedEntry(new Movie()
        {
            Name = "Ace Ventura",
            ReleaseDate = DateOnly.Parse("1994.02.04")
        });
        
        var request = new AddMovieRequest(movie.Name, movie.ReleaseDate);

        // Act
        var (resp, result) = await _client.POSTAsync<AddMovie, AddMovieRequest, ErrorResponse>(request);

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeNull();
        result!.Errors.Should()
            .OnlyContain(e => e.Value.Contains($"Movie '{movie.Name} ({movie.ReleaseDate})' already present!"));
    }
    
    [Fact]
    public async Task Adding_new_Movie_with_empty_name_should_fail()
    {
        // Arrange
        var request = new AddMovieRequest(string.Empty, DateOnly.Parse("1994.02.04"));

        // Act
        var (resp, result) = await _client.POSTAsync<AddMovie, AddMovieRequest, ErrorResponse>(request);

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeNull();
        result!.Errors.Should().OnlyContain(e =>
            e.Key == "movieName" && e.Value.All(m => m == "'Movie Name' must not be empty."));
    }
    
    [Fact]
    public async Task Adding_new_Movie_with_old_release_date_should_fail()
    {
        // Arrange
        var request = new AddMovieRequest("Really old movie", DateOnly.Parse("1899.12.31"));

        // Act
        var (resp, result) = await _client.POSTAsync<AddMovie, AddMovieRequest, ErrorResponse>(request);

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeNull();
        result!.Errors.Should().OnlyContain(e =>
            e.Key == "releaseDate" && e.Value.All(m => m == "'Release Date' must be greater than or equal to '1/1/1900'."));
    }
}