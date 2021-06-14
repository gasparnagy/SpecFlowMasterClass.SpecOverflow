using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

namespace SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Drivers
{
    public class HomePageDriver
    {
        private readonly BrowserContext _browserContext;

        private IWebElement MainMessageH1 => _browserContext.Driver.FindElement(By.Id("MainMessage"));
        private IWebElement LoggedInUserSpan => _browserContext.Driver.FindElements(By.Id("LoggedInUser")).FirstOrDefault();
        private ReadOnlyCollection<IWebElement> LatestQuestionDivs => _browserContext.Driver.FindElements(By.CssSelector("#Questions .question-info"));

        public HomePageDriver(BrowserContext browserContext)
        {
            _browserContext = browserContext;
        }

        public void GoTo()
        {
            _browserContext.NavigateTo("/", true);
        }

        public HomePageModel GetHomePageModel()
        {
            GoTo();
            return new HomePageModel
            {
                MainMessage = MainMessageH1.Text,
                UserName = LoggedInUserSpan?.Text,
                LatestQuestions = ParseLatestQuestions().ToList()
            };
        }

        public UserReferenceModel GetCurrentUser()
        {
            var userName = GetHomePageModel().UserName;
            return string.IsNullOrWhiteSpace(userName) ? null : new() {Name = userName};
        }

        private IEnumerable<QuestionSummaryModel> ParseLatestQuestions()
        {
            foreach (var latestQuestionDiv in LatestQuestionDivs.ToArray())
            {
                yield return new QuestionSummaryModel
                {
                    Id = Guid.Parse(latestQuestionDiv.GetAttribute("data-question-id")),
                    Title = latestQuestionDiv.FindElement(By.CssSelector(".question .body a")).Text,
                    Votes = int.Parse(latestQuestionDiv.FindElement(By.CssSelector(".votes")).Text),
                    Answers = int.Parse(latestQuestionDiv.FindElement(By.CssSelector(".answers")).Text),
                    Views = int.Parse(latestQuestionDiv.FindElement(By.CssSelector(".views")).Text),
                    AskedBy = new UserReferenceModel { Name = latestQuestionDiv.FindElement(By.CssSelector(".question .user-name")).Text },
                    AskedAt = DateTime.Parse(latestQuestionDiv.FindElement(By.CssSelector(".question .timestamp")).GetAttribute("data-time"))
                };
            }
        }
    }
}
