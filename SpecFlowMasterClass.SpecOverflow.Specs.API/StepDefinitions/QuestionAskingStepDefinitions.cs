using System;
using SpecFlowMasterClass.SpecOverflow.Specs.API.Drivers;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Specs.Support.Data;
using SpecFlowMasterClass.SpecOverflow.Web.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowMasterClass.SpecOverflow.Specs.API.StepDefinitions
{
    [Binding]
    public class QuestionAskingStepDefinitions
    {
        private readonly QuestionApiDriver _askQuestionDriver;
        private QuestionDetailModel _questionDetails;
        private Table _askQuestionSpecification;

        public QuestionAskingStepDefinitions(QuestionApiDriver askQuestionDriver)
        {
            _askQuestionDriver = askQuestionDriver;
        }

        [When("the user asks a question as")]
        public void WhenTheUserAsksAQuestionAs(Table questionTable)
        {
            _askQuestionSpecification = questionTable;
            var question = questionTable.CreateInstance(DomainDefaults.GetDefaultAskInput);
            var result = _askQuestionDriver.AskQuestion.Perform(question);
            _questionDetails = _askQuestionDriver.GetQuestionDetails(result.Id);
        }

        [When("the user attempts to ask a question")]
        public void WhenTheUserAttemptsToAskAQuestion()
        {
            _askQuestionDriver.AskQuestion.Perform(DomainDefaults.GetDefaultAskInput(), true);
        }

        [When("the user attempts to ask a question as")]
        public void WhenTheUserAttemptsToAskAQuestionAs(Table questionTable)
        {
            var question = questionTable.CreateInstance(DomainDefaults.GetDefaultAskInput);
            _askQuestionDriver.AskQuestion.Perform(question, true);
        }

        [Then("the question should be posted as above")]
        public void ThenTheQuestionShouldBePostedAsAbove()
        {
            _askQuestionSpecification.CompareToInstance(_questionDetails.ToQuestionData());
        }

        [Then("the question meta data should be")]
        public void ThenTheQuestionMetaDataShouldBe(Table expectedQuestionMetaDataTable)
        {
            expectedQuestionMetaDataTable.CompareToInstance(_questionDetails.ToQuestionData());
        }

        [Then("the ask attempt should fail with error {string}")]
        public void ThenTheAskAttemptShouldFailWithError(string expectedErrorMessageKey)
        {
            _askQuestionDriver.AskQuestion.ShouldFailWithError(expectedErrorMessageKey);
        }
    }
}
