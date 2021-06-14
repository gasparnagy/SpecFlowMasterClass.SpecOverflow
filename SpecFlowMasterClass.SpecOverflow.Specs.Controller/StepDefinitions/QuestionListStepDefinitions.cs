using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Specs.Support.Data;
using SpecFlowMasterClass.SpecOverflow.Web.Controllers;
using SpecFlowMasterClass.SpecOverflow.Web.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowMasterClass.SpecOverflow.Specs.Controller.StepDefinitions
{
    [Binding]
    public class QuestionListStepDefinitions
    {
        private readonly QuestionContext _questionContext;
        private List<QuestionSummaryModel> _questions;

        public QuestionListStepDefinitions(QuestionContext questionContext)
        {
            _questionContext = questionContext;
        }

        [When("the user checks the questions page")]
        public void WhenTheUserChecksTheQuestionsPage()
        {
            var controller = new QuestionController();
            _questions = controller.GetQuestions();
        }

        [Then("the question should be listed among the questions as above")]
        public void ThenTheQuestionShouldBeListedAmongTheQuestionsAsAbove()
        {
            var question = _questions.FirstOrDefault(q => q.Title == _questionContext.CurrentQuestion.Title);
            question.Should().NotBeNull();
            _questionContext.QuestionSpecification.CompareToInstance(question.ToQuestionData());
        }

        [Then("the questions list should contain {int} questions")]
        public void ThenTheQuestionsListShouldContainQuestions(int expectedCount)
        {
            _questions.Should().HaveCount(expectedCount);
        }

        [Then("the question list should be ordered descending by ask date")]
        public void ThenTheQuestionListShouldBeOrderedDescendingByAskDate()
        {
            _questions.Should().BeInDescendingOrder(q => q.AskedAt);
        }
    }
}
