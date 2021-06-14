using System;
using SpecFlowMasterClass.SpecOverflow.Specs.API.Support;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

namespace SpecFlowMasterClass.SpecOverflow.Specs.API.Drivers
{
    public class QuestionApiDriver
    {
        private readonly WebApiContext _webApiContext;
        public ActionAttempt<AskInputModel, QuestionSummaryModel> AskQuestion { get; }

        public QuestionApiDriver(WebApiContext webApiContext, ActionAttemptFactory actionAttemptFactory)
        {
            _webApiContext = webApiContext;
            AskQuestion = actionAttemptFactory.CreateWithStatusCheck<AskInputModel, QuestionSummaryModel>(
                nameof(AskQuestion),
                askInput => webApiContext.ExecutePost<QuestionSummaryModel>("/api/question", askInput));
        }

        public QuestionDetailModel GetQuestionDetails(Guid questionId)
        {
            return _webApiContext.ExecuteGet<QuestionDetailModel>($"/api/question/{questionId}");
        }
    }
}
