using BoDi;
using Microsoft.Extensions.Configuration;

namespace GnuCashBudget.Adapter.Tests.Hooks;

[Binding]
public sealed class HttpClientHook(IObjectContainer objectContainer)
{
    [BeforeScenario]
    public void BeforeScenario()
    {
        var configuration = objectContainer.Resolve<IConfiguration>();
        var baseAddress = configuration["AdapterAddress"] ?? throw new Exception("BaseAddress is not specified");
        
        var handler = new HttpClientHandler();
        var client = new HttpClient(handler, false);
        
        client.BaseAddress = new Uri(baseAddress);
        
        objectContainer.RegisterInstanceAs<HttpClientHandler>(handler);
        objectContainer.RegisterInstanceAs<HttpClient>(client);
    }

    [AfterScenario]
    public void AfterScenario()
    {
        var handler = objectContainer.Resolve<HttpClientHandler>();
        var client = objectContainer.Resolve<HttpClient>();
        
        handler.Dispose();
        client.Dispose();
    }
}