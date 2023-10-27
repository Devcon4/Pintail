using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pintail.Domain.Aggregates.BoardAggregate;
using Pintail.Domain.ValueObjects;

namespace Pintail.Infrastructure.EntityConfig;

public class BoardConfig : IEntityTypeConfiguration<Board>
{
  public void Configure(EntityTypeBuilder<Board> builder) {
    builder.Property(b => b.Id).ValueGeneratedNever();
    builder.Property(b => b.ShortGuid).HasConversion(
      v => v.Key,
      v => new ShortGuid(v)
    ).HasColumnName("short_guid");
    builder.HasMany(b => b.Cards).WithOne(c => c.Board).HasForeignKey(c => c.BoardId);
    builder.HasMany(b => b.Labels).WithOne(l => l.Board).HasForeignKey(l => l.BoardId);
  }
}