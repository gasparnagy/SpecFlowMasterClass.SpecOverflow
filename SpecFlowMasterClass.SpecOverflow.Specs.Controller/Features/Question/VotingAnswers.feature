@voting
Feature: Voting for answers

Rule: Users can vote up and down answers

Scenario Outline: The user votes for answers
	Given there is a question asked
	And there is an answer for the question as
		| content                    | votes        |
		| Check the Formulation book | <vote count> |
	And the user is authenticated
	When the user votes <vote> the answer
	Then the vote count of the answer should be changed to <new vote count>
Examples: 
	| description     | vote count | vote | new vote count |
	| vote up         | 3          | up   | 4              |
	| vote down       | 3          | down | 2              |
	| can go negative | 0          | down | -1             |

Rule: The position of the answer might change after vote

Scenario Outline: The answers swop
	Given there is a question asked
	And there are answers for the question as
		| content                             | votes |
		| Check the Formulation book          | 2     |
		| Check the SpecFlow Masterclass book | 2     |
	And the user is authenticated
	When the user votes <vote> the answer "<voted answer>"
	Then the answer list should be shown as
		| content                             | 
		| Check the SpecFlow Masterclass book | 
		| Check the Formulation book          | 
Examples: 
	| description             | voted answer                        | vote |
	| answer becomes popular  | Check the SpecFlow Masterclass book | up   |
	| answer becomes obsolete | Check the Formulation book          | down |


Rule: Only authenticated users can vote

Scenario: Anonymous user cannot vote for answer
	Given there is a question asked
	And there is an answer for the question
	And the user is not authenticated
	When the user attempts to vote up the answer
	Then the answer voting attempt should fail with error "not-logged-in"


Rule: Users cannot vote for their own answer

Scenario: User cannot vote for their own answer
	Given there is a question asked
	And there is an answer for the question by Marvin
	And user Marvin is authenticated
	When the user attempts to vote up the answer
	Then the answer voting attempt should fail with error "cannot-vote-for-your-own-answer"


Rule: Multiple users can vote for the same answer

Scenario: Other user also voted while the user was on the question details page
	Given there is a question asked
	And there is an answer for the question as
		| content                    | votes |
		| Check the Formulation book | 3     |
	And the user is authenticated
	And the user opens the question details page of the question
	And another user votes up the answer in the meanwhile
	When the user votes up the answer
	Then the vote count of the answer should be changed to 5