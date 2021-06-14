using System;
using System.ComponentModel.DataAnnotations;

namespace SpecFlowMasterClass.SpecOverflow.Web.Models
{
    /// <summary>
    /// Represents the information that required for user registration
    /// </summary>
    public class RegisterInputModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordReEnter { get; set; }

        public override string ToString()
            => $"UserName: {UserName}, Password: {Password}, PasswordReEnter: {PasswordReEnter}";
    }
}
