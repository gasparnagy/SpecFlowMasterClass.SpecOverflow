Feature: Question suggestions

The user receives a suggested list of related questions while entering a new question
in order to avoid posting a duplicate.

#Rule: Questions with at least one common word should be suggested, excluding common English words

Scenario Outline: There is a question with common words of the one being asked
	Given there are questions asked as
		| title             |
		| What is SpecFlow? |
		| What is Cucumber? |
	And the user is authenticated
	When the user starts asking a question as
		| title                   |
		| Best SpecFlow practices |
	Then the suggestions list should be
		| title             |
		| What is SpecFlow? |
Examples: 
	| description                               | asked question                     |
	| Another question contains the same word   | Best SpecFlow practices            |
	| Word match is case insensitive            | Best SPECFLOW practices            |
	| Word 'is' is ignored from second question | What is the best SpecFlow practice |


#Rule: Words should be matched in title, body and tags

Scenario: The same word is in different fields
	Given there are questions asked as
		| title             | body               | tags     |
		| What is SpecFlow? | Body 1             |          |
		| Question 2        | I'm using SpecFlow |          |
		| Question 3        | I'm using SpecFlow | SpecFlow |
	And the user is authenticated
	When the user starts asking a question as
		| title                   |
		| Best SpecFlow practices |
	Then the suggestions list should be
		| title             |
		| What is SpecFlow? |
		| Question 2        |
		| Question 3        |


#Rule: Suggestions with more common words should be earlier in the list

Scenario: There are existing questions with multiple common words
	Given there are questions asked as
		| title                                 |
		| What is SpecFlow?                     |
		| Who is the best SpecFlow contributor? |
	And the user is authenticated
	When the user starts asking a question as
		| title                   |
		| Best SpecFlow practices |
	Then the suggestions list should be provided in this order
		| title                                 |
		| Who is the best SpecFlow contributor? |
		| What is SpecFlow?                     |


