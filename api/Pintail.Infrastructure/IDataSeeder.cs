namespace Pintail.Infrastructure;

public interface IDataSeeder {
  Task SeedAsync();
}

public interface IDevOnlyDataSeeder : IDataSeeder {
  Task DevOnlySeedAsync();
}