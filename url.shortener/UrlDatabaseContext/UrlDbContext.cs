using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Url.Shorter.Entities;

namespace Url.Shorter.UrlDatabaseContext;

public class UrlDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    //public UrlDbContext(IConfiguration configuration)
    //{
    //    _configuration = configuration;
    //}

    public DbSet<UrlItem> UrlItems { get; set; }
    public DbSet<VisitHistory> VisitHistories { get; set; }

    public UrlDbContext(DbContextOptions<UrlDbContext> options)
           : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlServer(_configuration.GetSection("ConnectionStrings:SqlServer").Value);
        //optionsBuilder.UseSqlServer("Server=.;Initial Catalog=UrlShortener;Persist Security Info=False;Integrated Security=True;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
