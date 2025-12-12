using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Tamweely.Infrastructure.Persistence;

namespace Tamweely.Infrastructure.Design;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        var conn = config.GetConnectionString("DefaultConnection")
                   ?? "Server=.;Database=TamweelyTestDB;Trusted_Connection=True;TrustServerCertificate=True";

        optionsBuilder.UseSqlServer(conn);

        return new AppDbContext(optionsBuilder.Options);
    }
}
