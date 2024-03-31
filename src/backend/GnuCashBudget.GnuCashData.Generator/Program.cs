using GnuCashBudget.GnuCashData.EntityFramework;
using GnuCashBudget.GnuCashData.Generator.Configuration;
using GnuCashBudget.GnuCashData.Generator.Interfaces;
using GnuCashBudget.GnuCashData.Generator.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

builder.Services.AddOptions<GeneratorOptions>().BindConfiguration(GeneratorOptions.Generator);

builder.Services.AddDbContext<GnuCashContext>((provider, innerBuilder) =>
{
    var generatorOptions = provider.GetRequiredService<IOptions<GeneratorOptions>>();
    innerBuilder.UseSqlite($"Data Source={generatorOptions.Value.DatabaseFile}");
    innerBuilder.EnableSensitiveDataLogging();
});

builder.Services.AddHostedService<ScopedBackgroundService>();
builder.Services.AddScoped<IScopedProcessingService, WorkerService>();
builder.Services.AddScoped<IGeneratorService, GeneratorService>();

builder.Services.AddGnuCashEntityFrameworkDal();

var host = builder.Build();
await host.RunAsync();
