using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DomusMind.Infrastructure.Persistence;

public sealed class DomusMindDbContextFactory : IDesignTimeDbContextFactory<DomusMindDbContext>
{
    public DomusMindDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString =
            configuration.GetConnectionString("domusmind")
            ?? configuration["ConnectionStrings:domusmind"]
            ?? throw new InvalidOperationException(
                "Connection string 'domusmind' was not found for design-time DbContext creation.");

        var optionsBuilder = new DbContextOptionsBuilder<DomusMindDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new DomusMindDbContext(optionsBuilder.Options);
    }
}