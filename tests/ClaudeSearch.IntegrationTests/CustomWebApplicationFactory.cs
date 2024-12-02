using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ClaudeSearch.Infrastructure.Persistence;

namespace ClaudeSearch.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<SearchDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<SearchDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<SearchDbContext>();

                    db.Database.EnsureCreated();

                    try
                    {
                        InitializeDatabase(db);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            });
        }

        private void InitializeDatabase(SearchDbContext context)
        {
            // Add test data here
        }
    }
}