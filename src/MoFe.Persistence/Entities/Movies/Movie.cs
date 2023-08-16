namespace MoFe.Persistence.Entities.Movies;

public class Movie
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateOnly ReleaseDate { get; set; }
}