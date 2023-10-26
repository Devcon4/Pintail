using Ardalis.GuardClauses;
using Ardalis.Specification;
using Pintail.Domain.Aggregates.CheckAggregate;
using Pintail.Domain.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Pintail.BusinessLogic.Checks;

public class GetChecks {
  public record Query : IRequest<IEnumerable<CheckDto>> {
    public int Count { get; init; }

    public Query(int count = 100) {
      Count = Guard.Against.OutOfRange(count, nameof(count), 1, 10000);
    }
  }

  public class Handler : IRequestHandler<Query, IEnumerable<CheckDto>> {
    private readonly ILogger<Handler> _logger;
    private readonly IRepository<Check> _checkRepository;

    public Handler(IRepository<Check> checkRepository, ILogger<Handler> logger) {
      _logger = logger;
      _checkRepository = checkRepository;
    }

    public async Task<IEnumerable<CheckDto>> Handle(Query request, CancellationToken cancellationToken) {
      var checks = await _checkRepository.ListAsync(new GetChecksSpec(request.Count), cancellationToken);

      return checks.Select(check => new CheckDto(check.Id, check.SiteId, check.Status, check.DateCreated, check.DateCompleted));
    }
  }
}