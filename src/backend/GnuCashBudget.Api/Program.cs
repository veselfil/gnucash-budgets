using System.Text.Json;
using System.Text.Json.Serialization;
using GnuCashBudget.SharedDomain;
using GnuCashBudget.Api.Configuration;
using GnuCashBudget.Feature.BudgetedAccounts.Budgeting.Requests;
using GnuCashBudget.GnuCashData.EntityFramework;
using GnuCashBudget.SharedDomain.Data;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var controllersAssembly = typeof(ListAccounts).Assembly;
var controllersApplicationPart = new AssemblyPart(controllersAssembly);

builder.Services
    .AddControllers()
    .ConfigureApplicationPartManager(manager =>
    {
        manager.ApplicationParts.Add(controllersApplicationPart);
    })
    .AddControllersAsServices()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes();
});

builder.Services.AddOptions<SourceOptions>().BindConfiguration("Source");
builder.Services.AddDbContext<GnuCashContext>((provider, builder) =>
{
    var sourceOptions = provider.GetRequiredService<IOptions<SourceOptions>>();
    builder.UseSqlite($"Data Source={sourceOptions.Value.DatabaseFile}");
    builder.EnableSensitiveDataLogging();
});

builder.Services.AddDbContext<BudgetsContext>((provider, builder) =>
{
    var sourceOptions = provider.GetRequiredService<IOptions<SourceOptions>>();
    builder.UseSqlite($"Data Source={sourceOptions.Value.BudgetsFile}");
    builder.EnableSensitiveDataLogging();
});
//
// builder.Services.AddScoped<IBudgetedAccountRepository, EntityFrameworkBudgetedAccountsRepository>();
// builder.Services.AddScoped<IBudgetsRepository, EntityFrameworkBudgetsRepository>();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<ListAccounts.Handler>();
});

builder.Services.AddSharedDomainServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(config =>
{
    config.AllowAnyOrigin();
    config.AllowAnyHeader();
    config.AllowAnyMethod();
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();