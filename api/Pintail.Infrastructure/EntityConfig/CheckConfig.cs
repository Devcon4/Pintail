using Pintail.Domain.Aggregates.CheckAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pintail.Infrastructure.EntityConfig;

public class CheckConfig : IEntityTypeConfiguration<Check> {
  public void Configure(EntityTypeBuilder<Check> builder) {
    builder.Property(x => x.Id).ValueGeneratedNever();
    builder.Property(x => x.SiteId).IsRequired();
    builder.Property(x => x.DateCreated).IsRequired();
    builder.Property(x => x.DateCompleted).IsRequired(false);

    builder.OwnsOne(x => x.Status, status => {
      status.Property(x => x.Value).HasColumnName("Status").IsRequired();
    });

  }
}