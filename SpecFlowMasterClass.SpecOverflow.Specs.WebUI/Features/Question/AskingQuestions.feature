@web
Feature: Asking questions

Rule: Should be able to ask a question

Scenario: The question is posted
	Given user Marvin is authenticated
	When the user asks a question as
		| title                              | body        | tags        |
		| How to write better BDD scenarios? | I need help | Gherkin,BDD |
	Then the question should be posted as above
	And the question meta data should be 
		| votes | asked at | asked by |
		| 0     | now      | Marvin   |

Rule: Only authenticated users can ask questions

Scenario: Anonymous user cannot answer questions
	Given the user is not authenticated
	When the user attempts to ask a question
	Then the ask attempt should fail with error "not-logged-in"

Rule: The question title and body are mandatory

Scenario Outline: Cannot post an answer with empty content
	Given user Marvin is authenticated
	When the user attempts to ask a question as
		| title   | body   |
		| <title> | <body> |
	Then the ask attempt should fail with error "Title and Body cannot be empty"
Examples: 
	| description | title                              | body        |
	| no title    |                                    | I need help |
	| no body     | How to write better BDD scenarios? |             |
