using Microsoft.EntityFrameworkCore;
using System.Reflection;
using url.shortener.Entities;

namespace url.shortener.UrlDatabaseContext;

public class UrlDbContext : DbContext
{
    public DbSet<UrlItem> UrlItems { get; set; }
    public DbSet<VisitHistory> VisitHistories { get; set; }

    public UrlDbContext(DbContextOptions<UrlDbContext> options)
           : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
