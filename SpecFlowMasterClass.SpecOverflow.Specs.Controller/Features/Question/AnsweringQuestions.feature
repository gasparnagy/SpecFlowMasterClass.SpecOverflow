Feature: Answering questions

Rule: Should be able to answer a question

Scenario: The answer is registered with no votes
	Given there is a question asked
	And user Marvin is authenticated
	When the user answers the question as
		| content                    | 
		| Check the Formulation book | 
	Then the answer should be listed among the answers with
		| votes | answered at | answered by |
		| 0     | now         | Marvin      |

Rule: Only authenticated users can answer questions

Scenario: Anonymous user cannot answer questions
	Given there is a question asked
	And the user is not authenticated
	When the user attempts to answer the question
	Then the answer attempt should fail with error "not-logged-in"

Rule: The answer content is mandatory

Scenario: Cannot post an answer with empty content
	Given there is a question asked
	And the user is authenticated
	When the user attempts to answer the question as 
		| content |
		|         |
	Then the answer attempt should fail with error "Content cannot be empty"
