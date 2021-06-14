using System;
using FluentAssertions;
using SpecFlowMasterClass.SpecOverflow.Specs.Controller.Drivers;
using SpecFlowMasterClass.SpecOverflow.Specs.Controller.Support;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Models;
using TechTalk.SpecFlow;

namespace SpecFlowMasterClass.SpecOverflow.Specs.Controller.StepDefinitions
{
    [Binding]
    public class QuestionVotingStepDefinitions
    {
        private readonly VoteQuestionDriver _voteQuestionDriver;
        private readonly IsolatedAuthContextFactory _isolatedAuthContextFactory;
        private readonly QuestionContext _questionContext;
        private QuestionSummaryModel _question;

        public QuestionVotingStepDefinitions(VoteQuestionDriver voteQuestionDriver, IsolatedAuthContextFactory isolatedAuthContextFactory, QuestionContext questionContext)
        {
            _voteQuestionDriver = voteQuestionDriver;
            _isolatedAuthContextFactory = isolatedAuthContextFactory;
            _questionContext = questionContext;
        }

        [When(@"the user votes (\w+) the question")]
        public void WhenTheUserVotesUpTheQuestion(VoteDirection vote)
        {
            _question = _voteQuestionDriver.Perform(vote);
        }

        [When(@"the user attempts to vote (\w+) the question")]
        public void WhenTheUserAttemptsToVoteTheQuestion(VoteDirection vote)
        {
            _question = _voteQuestionDriver.Perform(vote, true);
        }

        [Given(@"another user votes (\w+) the question in the meanwhile")]
        public void GivenAnotherUserVotesTheQuestionInTheMeanwhile(VoteDirection vote)
        {
            var otherUserAuthContext = _isolatedAuthContextFactory.CreateAuthContext();
            otherUserAuthContext.Authenticate(DomainDefaults.AltUserName, DomainDefaults.AltUserPassword);
            var otherUserVoteDriver = _isolatedAuthContextFactory.CreateDriver<VoteQuestionDriver>(otherUserAuthContext);
            otherUserVoteDriver.Perform(_questionContext.CurrentQuestionId, vote);
        }

        [Then(@"the vote count of the question should be changed to (-?\d+)")]
        public void ThenTheVoteCountOfTheQuestionShouldBeChangedTo(int expectedVoteCount)
        {
            _question.Votes.Should().Be(expectedVoteCount);
        }

        [Then(@"the question voting attempt should fail with error ""([^""]*)""")]
        public void ThenTheQuestionVotingAttemptShouldFailWithError(string expectedErrorMessageKey)
        {
            _voteQuestionDriver.ShouldFailWithError(expectedErrorMessageKey);
        }
    }
}
