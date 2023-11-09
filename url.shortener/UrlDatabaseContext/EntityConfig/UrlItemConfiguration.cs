﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Url.Shorter.Entities;

namespace Url.Shorter.UrlDatabaseContext.EntityConfig;

public class UrlItemConfiguration : IEntityTypeConfiguration<UrlItem>
{
    public void Configure(EntityTypeBuilder<UrlItem> builder)
    {
        builder.Property(p => p.OriginalUrl).IsRequired().HasMaxLength(1024).IsUnicode(true);

        builder.HasOne<VisitHistory>()
            .WithOne(p => p.UrlItem)
            .HasForeignKey<VisitHistory>(p => p.UrlItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}