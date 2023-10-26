using Pintail.Domain.Aggregates.CheckAggregate;

namespace Pintail.BusinessLogic.Checks;

public record CheckDto(Guid id, Guid siteId, CheckProgress status, DateTime dateCreated, DateTime? dateCompleted);