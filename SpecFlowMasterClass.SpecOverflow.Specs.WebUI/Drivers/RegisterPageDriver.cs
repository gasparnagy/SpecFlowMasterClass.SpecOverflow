using System;
using OpenQA.Selenium;
using SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Support;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

namespace SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Drivers
{
    public class RegisterPageDriver : ActionAttempt<RegisterInputModel, UserReferenceModel>
    {
        private readonly BrowserContext _browserContext;

        private const string PageUrl = "/Register";
        private IWebElement UserName => _browserContext.Driver.FindElement(By.Id("UserName"));
        private IWebElement Password => _browserContext.Driver.FindElement(By.Id("Password"));
        private IWebElement PasswordReEnter => _browserContext.Driver.FindElement(By.Id("PasswordReEnter"));
        private IWebElement RegisterButton => _browserContext.Driver.FindElement(By.Id("RegisterButton"));

        public RegisterPageDriver(BrowserContext browserContext)
        {
            _browserContext = browserContext;
        }

        public void GoTo()
        {
            _browserContext.NavigateTo(PageUrl);
        }

        protected override UserReferenceModel DoAction(RegisterInputModel registerInput)
        {
            GoTo();
            UserName.SendKeys(registerInput.UserName);
            Password.SendKeys(registerInput.Password);
            PasswordReEnter.SendKeys(registerInput.PasswordReEnter);
            _browserContext.SubmitFormWith(RegisterButton, true);
            _browserContext.AssertNotOnPath(PageUrl);
            //TODO: we should parse back the registered user name and ID from the success message
            return new UserReferenceModel { Name = registerInput.UserName };
        }
    }
}
