using Pintail.Domain.Aggregates.SiteAggregate;
using Pintail.Domain.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Pintail.BusinessLogic.Sites {

  public class GetSites {
    public record Query : IRequest<IEnumerable<SiteDto>> { }

    public class Handler : IRequestHandler<Query, IEnumerable<SiteDto>> {
      private readonly IRepository<Site> _repository;
      private readonly ILogger<Handler> _logger;

      public Handler(IRepository<Site> repository, ILogger<Handler> logger) {
        _repository = repository;
        _logger = logger;
      }

      public async Task<IEnumerable<SiteDto>> Handle(Query request, CancellationToken cancellationToken) {
        var sites = await _repository.ListAsync(new GetSitesSpec(), cancellationToken);

        return sites.Select(site => new SiteDto(site.Id, site.Name, site.Ordinal));
      }
    }
  }
}