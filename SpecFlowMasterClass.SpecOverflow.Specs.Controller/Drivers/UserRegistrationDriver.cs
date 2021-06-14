using System;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Controllers;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

namespace SpecFlowMasterClass.SpecOverflow.Specs.Controller.Drivers
{
    public class UserRegistrationDriver : ActionAttempt<RegisterInputModel, UserReferenceModel>
    {
        public UserRegistrationDriver() : base(registerInput =>
        {
            var controller = new UserController();
            return controller.Register(registerInput);
        })
        {
        }
    }
}
