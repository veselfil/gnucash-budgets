using GnuCashBudget.Adapter.Tests.Drivers;

namespace GnuCashBudget.Adapter.Tests.Steps;

[Binding]
public class CheckDataSteps(WebDriver webDriver)
{
    [Given(@"I call GET '(.*)'")]
    public async Task GivenICallGet(string url)
    {
        await webDriver.GetExpensesHistory(url);
    }
    
    [When(@"I call GET '(.*)' with ContinuationToken")]
    public async Task WhenICallGetWithContinuationToken(string url)
    {
        await webDriver.GetExpensesHistory(url, true);
    }

    [Then(@"the result should have status code '(.*)'")]
    public void ThenTheResultShouldHaveStatusCode(string expectedStatusCode)
    {
        webDriver.CheckResponseStatusCode(expectedStatusCode);
    }
    
    [Then(@"the response should not be empty")]
    public void ThenTheResponseShouldNotBeEmpty()
    {
        webDriver.CheckResponseContentIsNotEmpty();
    }
    
    [Then(@"the response should contain '(.*)' expense objects")]
    public void ThenTheResponseShouldContainExpenseObjects(int expectedObjects)
    {
        webDriver.CheckResponseContentContainsExpenseData(expectedObjects);
    }
}