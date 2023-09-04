using System.Net;
using FastEndpoints;
using FluentAssertions;
using MoFe.Api.Features.Movies;
using MoFe.Contracts.Features.Movies;
using MoFe.Persistence.Entities.Movies;

namespace MoFe.Api.Tests.Integration.Features.Movies;

public class GetAllMoviesTests : IClassFixture<CustomWebAppFac>
{
    private readonly CustomWebAppFac _factory;
    private readonly HttpClient _client;

    public GetAllMoviesTests(CustomWebAppFac factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Fetch_all_movies_when_no_present_should_return_empty_list()
    {
        // Arrange
        

        // Act
        var (resp, result) = await _client.GETAsync<GetAllMovies, IEnumerable<GetAllMovieResponse>>();

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Fetch_all_movies_when_present_should_return_all()
    {
        // Arrange
        var seededMovies = new List<Movie>();
        for (int i = 0; i < 10; i++)
        {
            var movie = await _factory.SeedEntry(new Movie()
            {
                Name = $"Movie Nr. {i}",
                ReleaseDate = DateOnly.FromDayNumber(i)
            });
            seededMovies.Add(movie);
        }

        // Act
        var (resp, result) = await _client.GETAsync<GetAllMovies, IEnumerable<GetAllMovieResponse>>();

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
        var fetchedMovies = result!.ToList();
        fetchedMovies.Should().Equal(seededMovies, (response, movie) => response.Id == movie.Id);
        fetchedMovies.Should().Equal(seededMovies, (response, movie) => response.Name == movie.Name);
        fetchedMovies.Should().Equal(seededMovies, (response, movie) => response.ReleaseDate == movie.ReleaseDate);
    }
}