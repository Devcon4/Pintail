using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pintail.Domain.Aggregates.BoardAggregate;
using Pintail.Domain.ValueObjects;

namespace Pintail.Infrastructure.EntityConfig;

public class LabelConfig : IEntityTypeConfiguration<Label>
{
  public void Configure(EntityTypeBuilder<Label> builder) {
    builder.Property(l => l.Id).ValueGeneratedNever();
    builder.OwnsOne(l => l.Position, p => {
      p.Property(p => p.X).HasColumnName("position_x");
      p.Property(p => p.Y).HasColumnName("position_y");
    });
    builder.Property(l => l.Body).HasConversion(
      v => v.ToString(),
      v => new TextLine(v!)
    ).HasColumnName("body");
  }
}