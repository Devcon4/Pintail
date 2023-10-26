using Ardalis.GuardClauses;
using Pintail.Domain.Core;

namespace Pintail.Domain.Aggregates.CheckAggregate;

public class Check : BaseEntity<Guid>, IAggregateRoot {
  public Guid SiteId { get; set; }
  public CheckProgress Status { get; set; }
  public DateTime DateCreated { get; set; }
  public DateTime? DateCompleted { get; set; }

  public Check(Guid siteId) {
    Id = Guid.NewGuid();
    SiteId = Guard.Against.Default(siteId);
    Status = new CheckProgress(CheckProgress.Started);
    DateCreated = DateTime.UtcNow;
  }

  public void SetFailed() {
    Status = new CheckProgress(CheckProgress.Failed);
    DateCompleted = DateTime.UtcNow;
  }

  public void SetCompleted() {
    Status = new CheckProgress(CheckProgress.Completed);
    DateCompleted = DateTime.UtcNow;
  }
}