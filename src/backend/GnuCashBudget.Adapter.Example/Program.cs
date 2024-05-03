using System.Text.Json;
using System.Text.Json.Serialization;
using GnuCashBudget.Adapter.Abstractions;
using GnuCashBudget.Adapter.Example;
using GnuCashBudget.Adapter.Example.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes();
});

builder.Services.AddOptions<ExampleOptions>().BindConfiguration(ExampleOptions.Example);

// First we need to register Adaptee because Adapter is using it in constructor
builder.Services.AddScoped<Adaptee>();
builder.Services.AddScoped<IBankTarget, Adapter>();

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

