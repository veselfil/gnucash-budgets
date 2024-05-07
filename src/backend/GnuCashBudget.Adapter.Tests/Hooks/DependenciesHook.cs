using BoDi;
using Microsoft.Extensions.Configuration;

namespace GnuCashBudget.Adapter.Tests.Hooks;

[Binding]
public class DependenciesHook(IObjectContainer objectContainer)
{
    [BeforeScenario]
    public void CreateConfiguration()
    {
        if (objectContainer.IsRegistered<IConfiguration>()) return;
        
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", true, true)
            .AddEnvironmentVariables()
            .Build();
        
        objectContainer.RegisterInstanceAs<IConfiguration>(configuration);
    }
}