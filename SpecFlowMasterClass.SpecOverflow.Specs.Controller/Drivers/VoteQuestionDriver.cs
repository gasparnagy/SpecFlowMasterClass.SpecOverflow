using System;
using SpecFlowMasterClass.SpecOverflow.Specs.Controller.Support;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Controllers;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

namespace SpecFlowMasterClass.SpecOverflow.Specs.Controller.Drivers
{
    public class VoteQuestionDriver : ActionAttempt<Tuple<Guid, VoteDirection>, QuestionSummaryModel>
    {
        private readonly QuestionContext _questionContext;

        public VoteQuestionDriver(QuestionContext questionContext, AuthContext authContext, QuestionDetailsPageDriver questionDetailsPageDriver) : base(input =>
        {
            var controller = new QuestionController();
            var result = controller
                .VoteQuestion(input.Item1, (int)input.Item2, authContext.AuthToken);
            questionDetailsPageDriver.LoadPage(input.Item1);
            return result;
        })
        {
            _questionContext = questionContext;
        }

        public QuestionSummaryModel Perform(Guid questionId, VoteDirection vote, bool attemptOnly = false) => 
            Perform(new Tuple<Guid, VoteDirection>(questionId, vote), attemptOnly);

        public QuestionSummaryModel Perform(VoteDirection vote, bool attemptOnly = false) => 
            Perform(_questionContext.CurrentQuestionId, vote, attemptOnly);
    }
}
