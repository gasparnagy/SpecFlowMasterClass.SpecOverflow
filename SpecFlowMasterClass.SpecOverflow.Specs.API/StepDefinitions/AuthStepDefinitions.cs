using System;
using FluentAssertions;
using SpecFlowMasterClass.SpecOverflow.Specs.API.Drivers;
using SpecFlowMasterClass.SpecOverflow.Specs.API.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Models;
using TechTalk.SpecFlow;

namespace SpecFlowMasterClass.SpecOverflow.Specs.API.StepDefinitions
{
    [Binding]
    public class AuthStepDefinitions
    {
        private readonly AuthApiDriver _authApiDriver;
        private readonly AuthContext _authContext;

        public AuthStepDefinitions(AuthContext authContext, AuthApiDriver authApiDriver)
        {
            _authContext = authContext;
            _authApiDriver = authApiDriver;
        }

        [Given("user Marvin is authenticated")]
        [Given("the user is authenticated")]
        public void GivenTheUserIsAuthenticated()
        {
            _authContext.Authenticate(DomainDefaults.UserName, DomainDefaults.UserPassword);
        }

        [Given("the user is not authenticated")]
        public void GivenTheUserIsNotAuthenticated()
        {
            _authApiDriver.GetCurrentUser().Should().BeNull();
        }

        [When("the user attempts to log in with user name {string} and password {string}")]
        public void WhenTheUserAttemptsToLogInWithUserNameAndPassword(string userName, string password)
        {
            _authApiDriver.Login.Perform(
                new LoginInputModel {Name = userName, Password = password}, true);
        }

        [Then("the login attempt should be successful")]
        public void ThenTheLoginAttemptShouldBeSuccessful()
        {
            _authApiDriver.Login.ShouldBeSuccessful();
        }

        [Then("the user should be authenticated")]
        public void ThenTheUserShouldBeAuthenticated()
        {
            var currentUser = _authApiDriver.GetCurrentUser();
            currentUser.Should().NotBeNull();
            currentUser.Name.Should().Be(_authApiDriver.Login.LastInput.Name);
        }
    }
}
