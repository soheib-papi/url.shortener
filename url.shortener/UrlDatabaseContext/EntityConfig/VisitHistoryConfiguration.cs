using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Url.Shorter.Entities;

namespace Url.Shorter.UrlDatabaseContext.EntityConfig;

public class VisitHistoryConfiguration : IEntityTypeConfiguration<VisitHistory>
{
    public void Configure(EntityTypeBuilder<VisitHistory> builder)
    {
        //builder.HasOne<UrlItem>()
        //    .WithOne(p => p.VisitHistory)
        //    .HasForeignKey<VisitHistory>(p => p.UrlItemId)
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}
