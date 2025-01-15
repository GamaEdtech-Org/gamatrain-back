using GamaEdtech.Back.DataSource.Contries;
using GamaEdtech.Back.DataSource.Schools;
using GamaEdtech.Back.Domain.Cities;
using GamaEdtech.Back.Domain.Countries;
using GamaEdtech.Back.Domain.Schools;
using GamaEdtech.Back.Domain.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GamaEdtech.Back.DataSource.Utils;

public class GamaEdtechDbContext : DbContext
{
    private readonly ConnectionString _connectionString;

    public GamaEdtechDbContext(ConnectionString connectionString)
    {
        _connectionString = connectionString;
    }

    //public GamaEdtechDbContext(DbContextOptions options) : base(options)
    //{

    //}

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
