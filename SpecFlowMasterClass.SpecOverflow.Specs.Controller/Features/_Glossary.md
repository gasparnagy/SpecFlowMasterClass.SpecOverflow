# Glossary

## Domain terms

* **Answer** -- An answer that has been posted for a question by a user.
* **Authenticated [user]** -- User who has registered in the application 
  earlier and logged in with the registered credentials (e.g. email and 
  password).
* **Question** -- A question that a registered user has asked by specifying a 
  *title*, describing it with a *body* and optionally added *tags*.
* **Unauthenticated [user]** -- User who has not logged in.
* **Unrestricted [user]** -- User that can use the full set of features of the application. 
* **User** -- Person who visits the *Spec Overflow* site to check, ask or 
  answer questions. Unauthenticated (anonymous) users are also treated as users.
* **Visitor** -- See *User*.
* **Voting** -- Each question or answer has a number of votes registered. 
  Authenticated users can vote up or down the items. The number of votes can also became negative if there were more down vote then up vote.

## Tags

* **@formulated** -- The scenario has been formulated, but the automation and 
  the implementation is not completed yet. These scenarios might be undefined or 
  failing, so they are excluded from the CI build. Once the scenario is 
  completed, the tag has to be removed.
* **@manual** -- The scenario is verified by manual checks and has to be 
  excluded from automated test execution.
* **@voting** -- A scenario that is related to any kind of voting functionality.
