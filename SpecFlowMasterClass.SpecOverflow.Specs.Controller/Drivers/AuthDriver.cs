using System;
using System.Net;
using SpecFlowMasterClass.SpecOverflow.Specs.Controller.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Controllers;
using SpecFlowMasterClass.SpecOverflow.Web.Models;
using SpecFlowMasterClass.SpecOverflow.Web.Utils;

namespace SpecFlowMasterClass.SpecOverflow.Specs.Controller.Drivers
{
    public class AuthDriver
    {
        private readonly AuthContext _authContext;
        
        public LoginDriver Login { get; }

        public AuthDriver(LoginDriver login, AuthContext authContext)
        {
            Login = login;
            _authContext = authContext;
        }

        public UserReferenceModel GetCurrentUser()
        {
            var controller = new AuthController();
            try
            {
                return controller.GetCurrentUser(_authContext.AuthToken);
            }
            catch (HttpResponseException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}
