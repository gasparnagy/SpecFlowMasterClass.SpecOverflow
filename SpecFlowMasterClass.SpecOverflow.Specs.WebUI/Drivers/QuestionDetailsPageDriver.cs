using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

namespace SpecFlowMasterClass.SpecOverflow.Specs.WebUI.Drivers
{
    public class QuestionDetailsPageDriver
    {
        private readonly BrowserContext _browserContext;

        private IWebElement QuestionDetails => _browserContext.Driver.FindElement(By.Id("QuestionDetails"));
        private IWebElement QuestionTitle => _browserContext.Driver.FindElement(By.Id("QuestionTitle"));
        private IWebElement QuestionBody => _browserContext.Driver.FindElement(By.Id("QuestionBody"));
        private IWebElement QuestionVotes => _browserContext.Driver.FindElement(By.Id("QuestionVotes"));
        private IWebElement QuestionStats => _browserContext.Driver.FindElement(By.Id("QuestionStats"));
        private IWebElement QuestionViews => _browserContext.Driver.FindElement(By.Id("QuestionViews"));
        private ReadOnlyCollection<IWebElement> QuestionTags => _browserContext.Driver.FindElements(By.CssSelector("#QuestionTags .post-tag"));
        private ReadOnlyCollection<IWebElement> AnswerDivs => _browserContext.Driver.FindElements(By.CssSelector("#Answers .answer-info"));

        public QuestionDetailsPageDriver(BrowserContext browserContext)
        {
            _browserContext = browserContext;
        }

        public QuestionDetailModel GetQuestionDetails()
        {
            return new()
            {
                Id = Guid.Parse(QuestionDetails.GetAttribute("data-question-id")),
                Title = QuestionTitle.Text,
                Body = QuestionBody.Text,
                Tags = QuestionTags.Select(te => te.Text).ToList(),
                Votes = int.Parse(QuestionVotes.Text),
                AskedBy = new UserReferenceModel { Name = QuestionStats.FindElement(By.CssSelector(".user-name")).Text },
                AskedAt = DateTime.Parse(QuestionStats.FindElement(By.CssSelector(".timestamp")).GetAttribute("data-time")),
                Views = int.Parse(QuestionViews.Text),
                Answers = ParseAnswers().ToList()
            };
        }


        private IEnumerable<AnswerDetailModel> ParseAnswers()
        {
            foreach (var answerDiv in AnswerDivs.ToArray())
            {
                yield return new AnswerDetailModel
                {
                    Id = Guid.Parse(answerDiv.GetAttribute("data-answer-id")),
                    Content = answerDiv.FindElement(By.CssSelector(".post-cell .multi-line")).Text,
                    Votes = int.Parse(answerDiv.FindElement(By.CssSelector(".current-votes")).Text),
                    AnsweredBy = new UserReferenceModel { Name = answerDiv.FindElement(By.CssSelector(".answer-stats .user-name")).Text },
                    AnsweredAt = DateTime.Parse(answerDiv.FindElement(By.CssSelector(".answer-stats .timestamp")).GetAttribute("data-time"))
                };
            }
        }

    }
}
