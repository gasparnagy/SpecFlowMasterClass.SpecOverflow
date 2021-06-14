using System;
using OpenQA.Selenium;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

namespace SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Drivers
{
    public class LoginPageDriver : ActionAttempt<LoginInputModel, string>
    {
        private readonly BrowserContext _browserContext;

        private const string PageUrl = "/Login";
        private IWebElement Name => _browserContext.Driver.FindElement(By.Id("Name"));
        private IWebElement Password => _browserContext.Driver.FindElement(By.Id("Password"));
        private IWebElement LoginButton => _browserContext.Driver.FindElement(By.Id("LoginButton"));

        public LoginPageDriver(BrowserContext browserContext)
        {
            _browserContext = browserContext;
        }

        public event Action<LoginInputModel, string> OnAuthenticated;

        public void GoTo()
        {
            _browserContext.NavigateTo(PageUrl);
        }

        protected override string DoAction(LoginInputModel loginInput)
        {
            GoTo();
            Name.SendKeys(loginInput.Name);
            Password.SendKeys(loginInput.Password);
            _browserContext.SubmitFormWith(LoginButton, true);
            _browserContext.AssertNotOnPath(PageUrl);
            OnAuthenticated?.Invoke(loginInput, loginInput.Name);
            return loginInput.Name;
        }
    }
}