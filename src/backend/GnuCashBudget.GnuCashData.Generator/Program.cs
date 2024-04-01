using GnuCashBudget.GnuCashData.EntityFramework;
using GnuCashBudget.GnuCashData.Generator.Configuration;
using GnuCashBudget.GnuCashData.Generator.Interfaces;
using GnuCashBudget.GnuCashData.Generator.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", true, true)
    .Build();

var serviceCollection = new ServiceCollection();

serviceCollection.AddOptions<GeneratorOptions>().Bind(configuration.GetSection(GeneratorOptions.Generator));
serviceCollection.AddDbContext<GnuCashContext>((provider, innerBuilder) =>
{
    var generatorOptions = provider.GetRequiredService<IOptions<GeneratorOptions>>();
    innerBuilder.UseSqlite($"Data Source={generatorOptions.Value.DatabaseFile}");
    innerBuilder.EnableSensitiveDataLogging();
});

serviceCollection.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

serviceCollection.AddScoped<IConsoleAppService, ConsoleAppService>();
serviceCollection.AddScoped<IGeneratorService, GeneratorService>();
serviceCollection.AddGnuCashEntityFrameworkDal();

var serviceProvider = serviceCollection.BuildServiceProvider();

using var scope = serviceProvider.CreateScope();
var workerService = scope.ServiceProvider.GetRequiredService<IConsoleAppService>();

await workerService.DoWorkAsync(CancellationToken.None);
