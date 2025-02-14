namespace LolApp.Data;

using Microsoft.EntityFrameworkCore;

public class LeagueContext : DbContext
{
    public LeagueContext(DbContextOptions<LeagueContext> options)
        : base(options)
    {
    }

    public DbSet<AutoSettings> AutoSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Build<AutoSettings>();
    }
}

public static class ModelBuilderExtensions
{
    public static void Build<T>(this ModelBuilder modelBuilder) where T : class
    {
        modelBuilder.Entity<T>().ToTable(typeof (T).Name);
    }
}