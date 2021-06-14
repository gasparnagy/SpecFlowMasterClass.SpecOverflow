Feature: Home

Rule: A positive message should be shown on the home page

Scenario: Welcome message is shown on home page
	When the user checks the home page
	Then the home page main message should be: "Welcome to Spec Overflow!"

Rule: The user name should be shown on the home page if logged in

Scenario: The logged-in user name is shown on home page
	Given the user is authenticated
	When the user checks the home page
	Then the user name of the user should be on the home page

Rule: The latest 10 question should be shown on the home page

Scenario: The latest question is shown on home page with details
	Given there is a question just asked as
		| title                              | votes | answers | views | asked by |
		| How to write better BDD scenarios? | 0     | 0       | 1     | Ford     |
	When the user checks the home page
	Then the question should be listed among the latest questions as above

Scenario: The latest 10 questions are shown on home page
	Given there are 12 questions asked
	When the user checks the home page
	Then the home page should contain the 10 latest questions ordered
