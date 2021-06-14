using System;
using SpecFlowMasterClass.SpecOverflow.Specs.Controller.Drivers;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Specs.Support.Data;
using SpecFlowMasterClass.SpecOverflow.Web.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowMasterClass.SpecOverflow.Specs.Controller.StepDefinitions
{
    [Binding]
    public class QuestionAskingStepDefinitions
    {
        private readonly AskQuestionDriver _askQuestionDriver;
        private readonly QuestionDetailsPageDriver _questionDetailsPageDriver;
        private Table _askQuestionSpecification;

        public QuestionAskingStepDefinitions(AskQuestionDriver askQuestionDriver, QuestionDetailsPageDriver questionDetailsPageDriver)
        {
            _askQuestionDriver = askQuestionDriver;
            _questionDetailsPageDriver = questionDetailsPageDriver;
        }

        [When("the user asks a question as")]
        public void WhenTheUserAsksAQuestionAs(Table questionTable)
        {
            _askQuestionSpecification = questionTable;
            var question = questionTable.CreateInstance(DomainDefaults.GetDefaultAskInput);
            var result = _askQuestionDriver.Perform(question);
            _questionDetailsPageDriver.LoadPage(result.Id);
        }

        [When("the user attempts to ask a question")]
        public void WhenTheUserAttemptsToAskAQuestion()
        {
            _askQuestionDriver.Perform(DomainDefaults.GetDefaultAskInput(), true);
        }

        [When("the user attempts to ask a question as")]
        public void WhenTheUserAttemptsToAskAQuestionAs(AskInputModel askedQuestion)
        {
            _askQuestionDriver.Perform(askedQuestion, true);
        }

        [Then("the question should be posted as above")]
        public void ThenTheQuestionShouldBePostedAsAbove()
        {
            _askQuestionSpecification.CompareToInstance(_questionDetailsPageDriver.PageContent.ToQuestionData());
        }

        [Then("the question meta data should be")]
        public void ThenTheQuestionMetaDataShouldBe(Table expectedQuestionMetaDataTable)
        {
            expectedQuestionMetaDataTable.CompareToInstance(_questionDetailsPageDriver.PageContent.ToQuestionData());
        }

        [Then("the ask attempt should fail with error {string}")]
        public void ThenTheAskAttemptShouldFailWithError(string expectedErrorMessageKey)
        {
            _askQuestionDriver.ShouldFailWithError(expectedErrorMessageKey);
        }
    }
}
