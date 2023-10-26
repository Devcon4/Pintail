using Ardalis.Specification;
using Pintail.Domain.Aggregates.SiteAggregate;
// GetSitesSpec. This is a Specification that is used to retrieve a list of Sites from the database.
public class GetSitesSpec : Specification<Site> {
  public GetSitesSpec() {
    Query.OrderBy(site => site.Ordinal);
  }
}