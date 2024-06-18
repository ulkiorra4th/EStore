using EStore.Application.Extensions;
using EStore.Infrastructure.Extensions;
using EStore.Persistence.Extensions;
using EStoreCLI.Data;
using EStoreCLI.Extensions;
using EStoreCLI.IO;
using EStoreCLI.QueryProcess;
using Microsoft.Extensions.DependencyInjection;

const string pathToConfigFile = @"/Users/ulkiorra/RiderProjects/EStoreCLI/EStore.CLI/Config.json";

#region Dependency Injection

IServiceCollection services = new ServiceCollection();

services.AddConfiguration(pathToConfigFile);
var configuration = services.BuildServiceProvider().GetRequiredService<Configuration>();

services.AddApplicationServices();
services.AddDbContextAndRepositories(configuration.GetConnectionString("postgresql")!); 
services.AddPersistenceMappers();
services.AddInfrastructureTools();
services.AddSingleton<QueryHandler>();

var serviceProvider = services.BuildServiceProvider();

#endregion

ConsoleOutput.ShowWelcomeMessage();

var queryHandler = serviceProvider.GetRequiredService<QueryHandler>();

await queryHandler.RequestUserAuthInfo();
await queryHandler.Listen();