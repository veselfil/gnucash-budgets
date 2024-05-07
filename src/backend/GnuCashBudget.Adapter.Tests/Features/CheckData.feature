Feature: CheckData
Basic data checker. Checks if the data got from Adapter are correct
	
Scenario: Check if there is correct status code for endpoint /expenses/history
	Given I call GET '/expenses/history'
	Then the result should have status code '200'
	
Scenario: Check if there is data on endpoint /expenses/history
	Given I call GET '/expenses/history'
	Then the response should not be empty
	
Scenario: Check if there are 3 objects on endpoint /expenses/history
	Given I call GET '/expenses/history'
	Then the response should contain '3' expense objects
	
Scenario: Check if I call /expenses/history endpoint with ContinuationToken I get 0 objects
	Given I call GET '/expenses/history'
	Then the response should contain '3' expense objects
	
	When I call GET '/expenses/history' with ContinuationToken
	Then the response should contain '0' expense objects