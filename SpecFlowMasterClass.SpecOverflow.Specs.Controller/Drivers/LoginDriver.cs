using System;
using SpecFlowMasterClass.SpecOverflow.Specs.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Controllers;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

namespace SpecFlowMasterClass.SpecOverflow.Specs.Controller.Drivers
{
    public class LoginDriver : ActionAttempt<LoginInputModel, string>
    {
        public event Action<LoginInputModel, string> OnAuthenticated;
        
        protected override string DoAction(LoginInputModel loginInput)
        {
            var controller = new AuthController();
            var authToken = controller.Login(new LoginInputModel { Name = loginInput.Name, Password = loginInput.Password });
            OnAuthenticated?.Invoke(loginInput, authToken);
            return authToken;
        }
    }
}
