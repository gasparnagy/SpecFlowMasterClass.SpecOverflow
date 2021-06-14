using System;
using System.Linq;
using FluentAssertions;
using SpecFlowMasterClass.SpecOverflow.Specs.Controller.Drivers;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Specs.Support.Data;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowMasterClass.SpecOverflow.Specs.Controller.StepDefinitions
{
    [Binding]
    public class QuestionDetailsStepDefinitions
    {
        private readonly QuestionContext _questionContext;
        private readonly QuestionDetailsPageDriver _questionDetailsPageDriver;

        public QuestionDetailsStepDefinitions(QuestionContext questionContext, QuestionDetailsPageDriver questionDetailsPageDriver)
        {
            _questionContext = questionContext;
            _questionDetailsPageDriver = questionDetailsPageDriver;
        }

        [Given(@"the user opens the question details page of the question")]
        [When(@"the user checks the question details page of the question")]
        public void WhenTheUserChecksTheQuestionDetailsPageOfTheQuestion()
        {
            _questionDetailsPageDriver.LoadPage(_questionContext.CurrentQuestionId);
        }

        [Then(@"the question details should be shown as")]
        public void ThenTheQuestionDetailsShouldBeShownAs(Table expectedQuestionDetailsTable)
        {
            expectedQuestionDetailsTable.CompareToInstance(_questionDetailsPageDriver.PageContent.ToQuestionData());
        }

        [Then(@"the question details should be shown as above")]
        public void ThenTheQuestionDetailsShouldBeShownAsAbove()
        {
            _questionContext.QuestionSpecification.CompareToInstance(_questionDetailsPageDriver.PageContent.ToQuestionData());
        }

        [Then(@"the answer should be listed among the answers as above")]
        public void ThenTheAnswerShouldBeListedAmongTheAnswersAsAbove()
        {
            var answer = _questionDetailsPageDriver.GetAnswerByIdFromPageContent();
            _questionContext.AnswerSpecification.CompareToInstance(answer.ToAnswerData());
        }

        [Then(@"^the answer should be listed among the answers (?:as|with)$")]
        public void ThenTheAnswerShouldBeListedAmongTheAnswersAs(Table expectedAnswerTable)
        {
            var answer = _questionDetailsPageDriver.GetAnswerByContentFromPageContent(_questionContext.CurrentAnswer.Content);

            expectedAnswerTable.CompareToInstance(answer.ToAnswerData());
        }

        [Then(@"the answers list should contain (\d+) answers")]
        public void ThenTheAnswersListShouldContainAnswers(int expectedAnswerCount)
        {
            _questionDetailsPageDriver.PageContent.Answers.Should().HaveCount(expectedAnswerCount);
        }

        [Then(@"the answer list should be shown as")]
        public void ThenTheAnswerListShouldBeShownAs(Table expectedAnswerListTable)
        {
            var actualAnswers = _questionDetailsPageDriver.PageContent.Answers.Select(a => a.ToAnswerData());
            expectedAnswerListTable.CompareToSet(actualAnswers, true);
        }

        [Then(@"the answers list should be ordered descending by vote")]
        public void ThenTheAnswersListShouldBeOrderedDescendingByVote()
        {
            _questionDetailsPageDriver.PageContent.Answers.Should().BeInDescendingOrder(a => a.Votes);
        }
    }
}
