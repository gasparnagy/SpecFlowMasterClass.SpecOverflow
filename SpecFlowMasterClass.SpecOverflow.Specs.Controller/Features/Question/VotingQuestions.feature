@voting
Feature: Voting for questions

Rule: Users can vote up and down questions

Scenario: The user votes up a question
	Given there is a question asked with 2 votes
	And the user is authenticated
	When the user votes up the question
	Then the vote count of the question should be changed to 3


Rule: Only authenticated users can vote

Scenario: Anonymous user cannot vote for question
	Given there is a question asked
	And the user is not authenticated
	When the user attempts to vote up the question
	Then the question voting attempt should fail with error "not-logged-in"


Rule: Users cannot vote for their own question

Scenario: User cannot vote for their own question
	Given there is a question asked by Marvin
	And user Marvin is authenticated
	When the user attempts to vote up the question
	Then the question voting attempt should fail with error "cannot-vote-for-your-own-question"


Rule: Multiple users can vote for the same question

Scenario: Other user also voted while the user was on the question details page
	Given there is a question asked as
		| title                              | votes | 
		| How to write better BDD scenarios? | 3     | 
	And the user is authenticated
	And the user opens the question details page of the question
	And another user votes up the question in the meanwhile
	When the user votes up the question
	Then the vote count of the question should be changed to 5
