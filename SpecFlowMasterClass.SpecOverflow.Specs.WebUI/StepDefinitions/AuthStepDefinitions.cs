using System;
using FluentAssertions;
using SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Drivers;
using SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Models;
using TechTalk.SpecFlow;

namespace SpecFlowMasterClass.SpecOverflow.Specs.WebUI.StepDefinitions
{
    [Binding]
    public class AuthStepDefinitions
    {
        private readonly LoginPageDriver _loginPageDriver;
        private readonly HomePageDriver _homePageDriver;
        private readonly AuthContext _authContext;

        public AuthStepDefinitions(AuthContext authContext, LoginPageDriver loginPageDriver, HomePageDriver homePageDriver)
        {
            _authContext = authContext;
            _loginPageDriver = loginPageDriver;
            _homePageDriver = homePageDriver;
        }

        [Given(@"user Marvin is authenticated")]
        [Given(@"the user is authenticated")]
        public void GivenTheUserIsAuthenticated()
        {
            _authContext.Authenticate(DomainDefaults.UserName, DomainDefaults.UserPassword);
        }

        [Given(@"the user is not authenticated")]
        public void GivenTheUserIsNotAuthenticated()
        {
            _homePageDriver.GetCurrentUser().Should().BeNull();
        }

        [When(@"the user attempts to log in with user name ""([^""]*)"" and password ""([^""]*)""")]
        public void WhenTheUserAttemptsToLogInWithUserNameAndPassword(string userName, string password)
        {
            _loginPageDriver.Perform(
                new LoginInputModel {Name = userName, Password = password}, true);
        }

        [Then(@"the login attempt should be successful")]
        public void ThenTheLoginAttemptShouldBeSuccessful()
        {
            _loginPageDriver.ShouldBeSuccessful();
        }

        [Then(@"the user should be authenticated")]
        public void ThenTheUserShouldBeAuthenticated()
        {
            var currentUser = _homePageDriver.GetCurrentUser();
            currentUser.Should().NotBeNull();
            currentUser.Name.Should().Be(_loginPageDriver.LastInput.Name);
        }
    }
}
