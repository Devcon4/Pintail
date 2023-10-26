using Pintail.Domain.Aggregates.SiteAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pintail.Infrastructure.EntityConfig;

public class SiteConfig : IEntityTypeConfiguration<Site> {
  public void Configure(EntityTypeBuilder<Site> builder) {
    builder.Property(x => x.Id).ValueGeneratedNever();
    builder.Property(x => x.Url).IsRequired();
    builder.Property(x => x.Name).HasMaxLength(120).IsRequired();
  }
}