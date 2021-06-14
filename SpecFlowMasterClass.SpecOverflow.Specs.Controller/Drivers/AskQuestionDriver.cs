using System;
using SpecFlowMasterClass.SpecOverflow.Specs.Controller.Support;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Controllers;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

namespace SpecFlowMasterClass.SpecOverflow.Specs.Controller.Drivers
{
    public class AskQuestionDriver : ActionAttempt<AskInputModel, QuestionSummaryModel>
    {
        public AskQuestionDriver(AuthContext authContext) : base(askInput =>
        {
            var controller = new QuestionController();
            return controller.AskQuestion(askInput, authContext.AuthToken);
        })
        {
        }
    }
}
