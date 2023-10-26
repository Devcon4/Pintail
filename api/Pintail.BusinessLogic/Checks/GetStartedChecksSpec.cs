using Ardalis.Specification;
using Pintail.Domain.Aggregates.CheckAggregate;

namespace Pintail.BusinessLogic.Checks;

public class GetStartedChecksSpec : Specification<Check> {
  public GetStartedChecksSpec(DateTime olderThan) {
    Query
      .Where(check => check.Status.Value == CheckProgress.Started && check.DateCreated >= olderThan)
      .OrderByDescending(check => check.DateCreated);
  }
}