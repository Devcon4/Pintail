using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pintail.Domain.Aggregates;
using Pintail.Domain.Aggregates.BoardAggregate;
using Pintail.Domain.ValueObjects;

namespace Pintail.Infrastructure;

public class DataSeeder : IDataSeeder {
  private readonly PintailDbContext _dbContext;
  private readonly ILogger<DataSeeder> _logger;

  public DataSeeder(PintailDbContext dbContext, ILogger<DataSeeder> logger) {
    _dbContext = dbContext;
    _logger = logger;
  }

  public async Task SeedAsync() {
    _logger.LogInformation("Migrating database...");
    await _dbContext.Database.MigrateAsync();
    _logger.LogInformation("Database migration completed.");

    _logger.LogInformation("Seeding database...");
    await InsertIFNotEmpty(GetBoards());

    _logger.LogInformation("Database seeding completed.");
  }

  public async Task InsertIFNotEmpty<T>(List<T> entities) where T : class {
    if (!await _dbContext.Set<T>().AnyAsync()) {
      await _dbContext.Set<T>().AddRangeAsync(entities);
      await _dbContext.SaveChangesAsync();
    }
  }

  public Board Board1 = new(new TextLine("Board 1"));
  public Board Board2 = new(new TextLine("Board 2"));
  public Board Board3 = new(new TextLine("Board 3"));

  public Board SeedBoard(Board board) {
    board.AddCard(new (board, new Position(0, 0), new TextMultiline("Card 1")));
    board.AddCard(new (board, new Position(0, 1), new TextMultiline("Card 2")));
    board.AddCard(new (board, new Position(0, 2), new TextMultiline("Card 3")));
    board.AddCard(new (board, new Position(1, 0), new TextMultiline("Card 4")));
    board.AddCard(new (board, new Position(1, 1), new TextMultiline("Card 5")));
    board.AddCard(new (board, new Position(1, 2), new TextMultiline("Card 6")));

    board.AddLabel(new (board, new Position(0, 0), new TextLine("Label 1")));
    board.AddLabel(new (board, new Position(0, 1), new TextLine("Label 2")));
    return board;
  }

  public List<Board> GetBoards() => new() {
    SeedBoard(Board1),
    SeedBoard(Board2),
    SeedBoard(Board3)
  };

}