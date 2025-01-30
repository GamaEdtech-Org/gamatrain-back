using GamaEdtech.DataSource.Contries;
using GamaEdtech.DataSource.Schools;
using GamaEdtech.Domain.Cities;
using GamaEdtech.Domain.Countries;
using GamaEdtech.Domain.Schools;
using GamaEdtech.Domain.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace GamaEdtech.DataSource.Utils;

public class GamaEdtechDbContext : DbContext
{
    private readonly ConnectionString _connectionString;

    public GamaEdtechDbContext(ConnectionString connectionString)
    {
        _connectionString = connectionString;
    }

    public DbSet<School> Schools { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<City> Cities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder
        //.UseLazyLoadingProxies()
        .UseSqlServer(_connectionString.Value, x => x.UseNetTopologySuite());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ApplyConfigurations(modelBuilder);
    }

    private static void ApplyConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SchoolConfiguration());
        modelBuilder.ApplyConfiguration(new CountryConfiguration());
        modelBuilder.ApplyConfiguration(new StateConfiguration());
        modelBuilder.ApplyConfiguration(new CityConfiguration());
    }
}

public class GamaEdtechDbContextFactory : IDesignTimeDbContextFactory<GamaEdtechDbContext>
{
    public GamaEdtechDbContext CreateDbContext(string[] args)
    {
        var environment =
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString =
            environment == "Development"
                ? configuration.GetConnectionString("Default")
                : Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING")!;

        return new GamaEdtechDbContext(new ConnectionString(connectionString!));
    }
}
