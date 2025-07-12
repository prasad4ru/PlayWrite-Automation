Feature: FM Pilot 2 Login

A short summary of the feature

@Sample
Scenario: User logs in with valid credentials
	Given I navigate to "https://www.fmpilot2.com"
	When I enter username "testuser" and password "testpass"
	And I click the login button
	Then I should be redirected to the dashboard
