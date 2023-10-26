using Ardalis.Specification;

namespace Pintail.Domain.Core;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot { }