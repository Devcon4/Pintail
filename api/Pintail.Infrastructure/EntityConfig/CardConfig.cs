using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pintail.Domain.Aggregates.BoardAggregate;
using Pintail.Domain.ValueObjects;

namespace Pintail.Infrastructure.EntityConfig;

public class CardConfig : IEntityTypeConfiguration<Card>
{
  public void Configure(EntityTypeBuilder<Card> builder) {
    builder.Property(c => c.Id).ValueGeneratedNever();
    builder.OwnsOne(c => c.Position, p => {
      p.Property(p => p.X).HasColumnName("position_x");
      p.Property(p => p.Y).HasColumnName("position_y");
    });
    builder.Property(c => c.Body).HasConversion(
      v => v.Value,
      v => new TextMultiline(v!)
    ).HasColumnName("body");
  }
}