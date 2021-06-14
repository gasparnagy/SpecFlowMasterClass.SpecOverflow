using System;
using SpecFlowMasterClass.SpecOverflow.Specs.Controller.Support;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Controllers;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

namespace SpecFlowMasterClass.SpecOverflow.Specs.Controller.Drivers
{
    public class PostAnswerDriver : ActionAttempt<AnswerInputModel, AnswerDetailModel>
    {
        public PostAnswerDriver(QuestionContext questionContext, AuthContext authContext, QuestionDetailsPageDriver questionDetailsPageDriver) : base(answerInput =>
        {
            var controller = new QuestionController();
            var result = questionContext.CurrentAnswer = controller
                .PostAnswer(questionContext.CurrentQuestionId, answerInput, authContext.AuthToken);
            questionDetailsPageDriver.LoadPage(questionContext.CurrentQuestionId);
            return result;
        })
        {
        }
    }
}
