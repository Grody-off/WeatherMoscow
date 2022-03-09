using Microsoft.EntityFrameworkCore;
using WeatherMoscow.Entity.Entity;

namespace WeatherMoscow.Entity.DbContext;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    private readonly DbContextOptions _options;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        _options = options;
    }
    
    public DbSet<MonthWeather> MonthWeathers { get; set; }
    public DbSet<YearWeather> YearWeathers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<YearWeather>(entity =>
        {
            entity.HasMany(b => b.MonthWeathers)
                .WithOne(b => b.YearWeather)
                .HasForeignKey(s => s.YearId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<MonthWeather>(entity =>
        {
            entity.HasMany(b => b.Weathers)
                .WithOne(b => b.MonthWeather)
                .HasForeignKey(s => s.MonthWeatherId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        base.OnModelCreating(modelBuilder);
    }
}