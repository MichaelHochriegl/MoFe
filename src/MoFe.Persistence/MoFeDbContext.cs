using Microsoft.EntityFrameworkCore;
using MoFe.Persistence.Entities.Movies;

namespace MoFe.Persistence;

public class MoFeDbContext : DbContext
{
    public DbSet<Movie> Movies => Set<Movie>();
    
    public MoFeDbContext(DbContextOptions<MoFeDbContext> options) : base(options)
    {
        
    }
}