using System;
using FluentAssertions;
using SpecFlowMasterClass.SpecOverflow.Specs.Controller.Drivers;
using SpecFlowMasterClass.SpecOverflow.Specs.Controller.Support;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using TechTalk.SpecFlow;

namespace SpecFlowMasterClass.SpecOverflow.Specs.Controller.StepDefinitions
{
    [Binding]
    public class AnswerVotingStepDefinitions
    {
        private readonly QuestionContext _questionContext;
        private readonly QuestionDetailsPageDriver _questionDetailsPageDriver;
        private readonly VoteAnswerDriver _voteAnswerDriver;
        private readonly IsolatedAuthContextFactory _isolatedAuthContextFactory;

        public AnswerVotingStepDefinitions(VoteAnswerDriver voteAnswerDriver, QuestionContext questionContext, QuestionDetailsPageDriver questionDetailsPageDriver, IsolatedAuthContextFactory isolatedAuthContextFactory)
        {
            _voteAnswerDriver = voteAnswerDriver;
            _questionContext = questionContext;
            _questionDetailsPageDriver = questionDetailsPageDriver;
            _isolatedAuthContextFactory = isolatedAuthContextFactory;
        }

        [When("the user votes {word} the answer")]
        public void WhenTheUserVotesTheAnswer(VoteDirection vote)
        {
            _voteAnswerDriver.Perform(vote);
        }

        [When("the user attempts to vote {word} the answer")]
        public void WhenTheUserAttemptsToVoteTheAnswer(VoteDirection vote)
        {
            _voteAnswerDriver.Perform(vote, true);
        }

        [When("the user votes {word} the answer {string}")]
        public void WhenTheUserVotesUpTheAnswer(VoteDirection vote, string answerContent)
        {
            _questionDetailsPageDriver.LoadPage();
            var answer = _questionDetailsPageDriver.GetAnswerByContentFromPageContent(answerContent);
            _voteAnswerDriver.Perform(answer.Id, vote);
        }

        [Given("another user votes {word} the answer in the meanwhile")]
        public void GivenAnotherUserVotesUpTheAnswerInTheMeanwhile(VoteDirection vote)
        {
            var otherUserAuthContext = _isolatedAuthContextFactory.CreateAuthContext();
            otherUserAuthContext.Authenticate(DomainDefaults.AltUserName, DomainDefaults.AltUserPassword);
            var otherUserVoteDriver = _isolatedAuthContextFactory.CreateDriver<VoteAnswerDriver>(otherUserAuthContext);
            otherUserVoteDriver.Perform(_questionContext.CurrentQuestionId, _questionContext.CurrentAnswerId, vote);
        }

        [Then("the vote count of the answer should be changed to {int}")]
        public void ThenTheVoteCountOfTheAnswerShouldBeChangedTo(int expectedVoteCount)
        {
            _questionContext.CurrentAnswer.Votes.Should().Be(expectedVoteCount);
        }

        [Then("the answer voting attempt should fail with error {string}")]
        public void ThenTheAnswerVotingAttemptShouldFailWithError(string expectedErrorMessageKey)
        {
            _voteAnswerDriver.ShouldFailWithError(expectedErrorMessageKey);
        }
    }
}
