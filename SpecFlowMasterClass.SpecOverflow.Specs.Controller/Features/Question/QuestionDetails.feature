Feature: Question details

Rule: All question details should be shown on the question details page

Scenario: The question is shown on question details page with all details
	Given there is a question asked as
		| title                              | body        | tags         | votes | answers | asked at       | asked by |
		| How to write better BDD scenarios? | I need help | Gherkin, BDD | 3     | 2       | 6/5/2021 22:38 | Ford     |
	When the user checks the question details page of the question
	Then the question details should be shown as above
	Then the answers list should contain 2 answers

Scenario: The answer is shown on question details page with all details
	Given there is a question asked
	And there is an answer for the question as
		| content                    | votes | answered at    | answered by |
		| Check the Formulation book | 3     | 6/5/2021 22:38 | Marvin      |
	When the user checks the question details page of the question
	Then the answer should be listed among the answers as above

Rule: The answers with more votes should be listed earlier

Scenario: All answers are shown on the question details page the highest voted on top
	Given there is a question asked as
		| title                              | answers | 
		| How to write better BDD scenarios? | 4       | 
	When the user checks the question details page of the question
	Then the answers list should contain 4 answers
	And the answers list should be ordered descending by vote

Rule: The view count of the question sould increase when the question details page is shown

Scenario: The question view count increases
	Given there is a question asked as
		| title                              | views | 
		| How to write better BDD scenarios? | 3     | 
	When the user checks the question details page of the question
	Then the question details should be shown as
		| title                              | views | 
		| How to write better BDD scenarios? | 4     | 
