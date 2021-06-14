Feature: Questions

Rule: The questions page should list all questions with the latest on top

Scenario: The latest question is shown on questions page with details
	Given there is a question asked as
		| title                              | votes | answers | views | asked at       | asked by |
		| How to write better BDD scenarios? | 3     | 2       | 1     | 6/5/2021 22:38 | Ford     |
	When the user checks the questions page
	Then the question should be listed among the questions as above

Scenario: All questions are shown on the questions page
	Given there are 12 questions asked
	When the user checks the questions page
	Then the questions list should contain 12 questions
	And the question list should be ordered descending by ask date
