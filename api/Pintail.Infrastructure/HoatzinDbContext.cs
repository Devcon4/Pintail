using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Pintail.Infrastructure;

public class PintailDbContext : DbContext {

  public PintailDbContext(DbContextOptions<PintailDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    base.OnModelCreating(modelBuilder);
  }
}