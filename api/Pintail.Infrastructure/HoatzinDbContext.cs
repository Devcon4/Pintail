using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Pintail.Domain.Aggregates.BoardAggregate;

namespace Pintail.Infrastructure;

public class PintailDbContext : DbContext {

  public PintailDbContext(DbContextOptions<PintailDbContext> options) : base(options) { }

  public DbSet<Board> Board => Set<Board>();
  public DbSet<Card> Card => Set<Card>();
  public DbSet<Label> Label => Set<Label>();

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    base.OnModelCreating(modelBuilder);
  }
}