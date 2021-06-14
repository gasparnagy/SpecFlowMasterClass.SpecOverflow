using System;
using SpecFlowMasterClass.SpecOverflow.Specs.Controller.Support;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Controllers;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

namespace SpecFlowMasterClass.SpecOverflow.Specs.Controller.Drivers
{
    public class VoteAnswerDriver : ActionAttempt<Tuple<Guid, Guid, VoteDirection>, AnswerDetailModel>
    {
        private readonly QuestionContext _questionContext;

        public VoteAnswerDriver(QuestionContext questionContext, AuthContext authContext, QuestionDetailsPageDriver questionDetailsPageDriver) : base(input =>
        {
            var controller = new QuestionController();
            var result = controller
                .VoteAnswer(input.Item1, input.Item2, (int)input.Item3, authContext.AuthToken);
            questionDetailsPageDriver.LoadPage(input.Item1);
            return result;
        })
        {
            _questionContext = questionContext;
        }

        public AnswerDetailModel Perform(Guid questionId, Guid answerId, VoteDirection vote, bool attemptOnly = false) => 
            Perform(new Tuple<Guid, Guid, VoteDirection>(questionId, answerId, vote), attemptOnly);

        public AnswerDetailModel Perform(Guid answerId, VoteDirection vote, bool attemptOnly = false) => 
            Perform(_questionContext.CurrentQuestionId, answerId, vote, attemptOnly);

        public AnswerDetailModel Perform(VoteDirection vote, bool attemptOnly = false) => 
            _questionContext.CurrentAnswer = Perform(_questionContext.CurrentQuestionId, _questionContext.CurrentAnswerId, vote, attemptOnly);
    }
}
