using Ardalis.Specification;
using Pintail.Domain.Aggregates.CheckAggregate;

namespace Pintail.BusinessLogic.Checks;

public class GetChecksSpec : Specification<Check> {
  public GetChecksSpec(int count) {
    Query
      .OrderByDescending(check => check.DateCreated)
      .Take(count);
  }
}