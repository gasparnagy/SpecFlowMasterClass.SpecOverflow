using System;
using SpecFlowMasterClass.SpecOverflow.Specs.API.Support;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

namespace SpecFlowMasterClass.SpecOverflow.Specs.API.Drivers
{
    public class UserApiDriver
    {
        public ActionAttempt<RegisterInputModel, UserReferenceModel> Register { get; }

        public UserApiDriver(WebApiContext webApiContext, ActionAttemptFactory actionAttemptFactory)
        {
            Register = actionAttemptFactory.CreateWithStatusCheck<RegisterInputModel, UserReferenceModel>(
                nameof(Register),
                registerInput => webApiContext.ExecutePost<UserReferenceModel>("/api/user", registerInput));
        }
    }
}
