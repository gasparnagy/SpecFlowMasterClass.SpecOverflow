using System;
using SpecFlowMasterClass.SpecOverflow.Specs.API.Drivers;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

namespace SpecFlowMasterClass.SpecOverflow.Specs.API.Support
{
    public class AuthContext
    {
        private readonly AuthApiDriver.LoginDriver _loginDriver;

        public string LoggedInUserName { get; set; }

        public bool IsLoggedIn => LoggedInUserName != null;

        public AuthContext(AuthApiDriver.LoginDriver loginDriver)
        {
            _loginDriver = loginDriver;

            loginDriver.OnAuthenticated += (loginInput, authToken) =>
            {
                LoggedInUserName = loginInput.Name;
            };
        }

        public void Authenticate(string userName, string password)
        {
            _loginDriver.Perform(new LoginInputModel { Name = userName, Password = password });
        }
    }
}
