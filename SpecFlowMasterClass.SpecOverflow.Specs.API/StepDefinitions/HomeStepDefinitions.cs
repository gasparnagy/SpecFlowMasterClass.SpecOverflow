using System;
using System.Linq;
using FluentAssertions;
using SpecFlowMasterClass.SpecOverflow.Specs.API.Drivers;
using SpecFlowMasterClass.SpecOverflow.Specs.API.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Models;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Specs.Support.Data;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowMasterClass.SpecOverflow.Specs.API.StepDefinitions
{
    [Binding]
    public class HomeStepDefinitions
    {
        private readonly HomeApiDriver _homeApiDriver;
        private readonly AuthContext _authContext;
        private readonly QuestionContext _questionContext;
        private HomePageModel _homePageModel;

        public HomeStepDefinitions(AuthContext authContext, HomeApiDriver homeApiDriver, QuestionContext questionContext)
        {
            _authContext = authContext;
            _homeApiDriver = homeApiDriver;
            _questionContext = questionContext;
        }

        [When("the client checks the home page")]
        public void WhenTheClientChecksTheHomePage()
        {
            _homePageModel = _homeApiDriver.GetHomePageModel();
        }

        [When("the user checks the home page")]
        public void WhenTheUserChecksTheHomePage()
        {
            _homePageModel = _homeApiDriver.GetHomePageModel();
        }

        [Then("the home page main message should be: {string}")]
        public void ThenTheHomePageMainMessageShouldBe(string expectedMessage)
        {
            _homePageModel.MainMessage.Should().Be(expectedMessage);
        }

        [Then("the user name of the user should be on the home page")]
        public void ThenTheUserNameOfTheUserShouldBeOnTheHomePage()
        {
            _authContext.IsLoggedIn.Should().BeTrue();
            _homePageModel.UserName.Should().Be(_authContext.LoggedInUserName);
        }

        [Then("the question should be listed among the latest questions as above")]
        public void ThenTheQuestionShouldBeListedAmongTheLatestQuestionsAsAbove()
        {
            var question = _homePageModel.LatestQuestions.FirstOrDefault(q => q.Id == _questionContext.CurrentQuestionId);
            _questionContext.QuestionSpecification.CompareToInstance(question.ToQuestionData());
        }

        [Then("the home page should contain the {int} latest questions ordered")]
        public void ThenTheHomePageShouldContainTheLatestQuestionsOrdered(int expectedCount)
        {
            var expectedQuestionIds = _questionContext.QuestionsCreated
                .OrderByDescending(q => q.AskedAt)
                .Take(expectedCount)
                .ToArray();
            _homePageModel.LatestQuestions.Should().Equal(expectedQuestionIds, (q1, q2) => q1.Id == q2.Id);
        }
    }
}
