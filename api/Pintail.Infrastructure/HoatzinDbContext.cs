using System.Reflection;
using Pintail.Domain.Aggregates.CheckAggregate;
using Pintail.Domain.Aggregates.SiteAggregate;
using Microsoft.EntityFrameworkCore;

namespace Pintail.Infrastructure;

public class PintailDbContext : DbContext {
  public DbSet<Site> Sites => Set<Site>();
  public DbSet<Check> Checks => Set<Check>();

  public PintailDbContext(DbContextOptions<PintailDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    base.OnModelCreating(modelBuilder);
  }
}