using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoFe.Persistence;
using Testcontainers.PostgreSql;

namespace MoFe.Api.Tests.Integration;

public class CustomWebAppFac : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _database = new PostgreSqlBuilder().Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var dbContextDescriptor = services.Single(
                d => d.ServiceType ==
                     typeof(DbContextOptions<MoFeDbContext>));

            services.Remove(dbContextDescriptor);
            services.AddPersistence(_database.GetConnectionString());
            
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MoFeDbContext>();
            dbContext.Database.Migrate();
        });
    }

    public Task InitializeAsync()
    {
        return _database.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _database.StopAsync();
        await _database.DisposeAsync();
    }
}