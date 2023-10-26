using Ardalis.Specification.EntityFrameworkCore;
using Pintail.Domain.Core;

namespace Pintail.Infrastructure;

public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class, IAggregateRoot {
  public Repository(PintailDbContext dbContext) : base(dbContext) { }
}