using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GnuCashBudget.Adapter.Abstractions.Models;

namespace GnuCashBudget.Adapter.Tests.Drivers;

public class WebDriver(HttpClient httpClient, ScenarioContext scenarioContext)
{
    public async Task GetExpensesHistory(string url, bool useContinuationToken = false)
    {
        var adapterResponseIsSaved = scenarioContext.TryGetValue<AdapterResponse>(out var adapterResponse);
        var finalUrl = useContinuationToken && adapterResponseIsSaved
                    ? $"{url}?continuationToken={adapterResponse.ContinuationToken}"
                    : url;
        
        var response = await httpClient.GetAsync(finalUrl);
        var content = response.Content;
        
        scenarioContext.Set<HttpResponseMessage>(response);
        scenarioContext.Set<HttpContent>(content);

        await ParseAdapterResponse();
    }

    public void CheckResponseStatusCode(string expectedStatusCode)
    {
        Enum.TryParse<HttpStatusCode>(expectedStatusCode, out var statusCode)
            .Should().BeTrue("HttpStatus was not parsed");

        var response = scenarioContext.Get<HttpResponseMessage>();
        
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(statusCode);
    }
    
    public void CheckResponseContentIsNotEmpty()
    {
        var result = scenarioContext.Get<AdapterResponse>();
        
        result!.Transactions.Should().NotBeEmpty();
    }

    public void CheckResponseContentContainsExpenseData(int expectedObjects)
    {
        var result = scenarioContext.Get<AdapterResponse>();

        result.Transactions.Count().Should().Be(expectedObjects);
    }

    private async Task ParseAdapterResponse()
    {
        var content = scenarioContext.Get<HttpContent>();
        
        content.Should().NotBeNull();
        var result = await content!.ReadFromJsonAsync<AdapterResponse>();
        
        result.Should().NotBeNull();
        
        scenarioContext.Set<AdapterResponse>(result!);
    }
}