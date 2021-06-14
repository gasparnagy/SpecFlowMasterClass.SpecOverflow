using System;
using System.Net;
using SpecFlowMasterClass.SpecOverflow.Web.DataAccess;
using SpecFlowMasterClass.SpecOverflow.Web.Models;
using SpecFlowMasterClass.SpecOverflow.Web.Services;
using Microsoft.AspNetCore.Mvc;
using SpecFlowMasterClass.SpecOverflow.Web.Utils;

namespace SpecFlowMasterClass.SpecOverflow.Web.Controllers
{
    /// <summary>
    /// Processes requests related to authentication and authorization
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _dataContext = new();
        private readonly ModelTransformationService _modelTransformationService;

        public AuthController()
        {
            _modelTransformationService = new ModelTransformationService(_dataContext);
        }

        // GET: api/auth
        public UserReferenceModel GetCurrentUser(string token = null)
        {
            var currentUser = AuthenticationServices.GetCurrentUserName(HttpContext, token);
            if (currentUser == null)
                throw new HttpResponseException(HttpStatusCode.NotFound, "token expired");

            var user = _dataContext.FindUserByName(currentUser);
            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound, "invalid user");

            return _modelTransformationService.ToUserReference(user);
        }

        // POST: api/auth
        [HttpPost]
        public string Login([FromBody] LoginInputModel args)
        {
            //for the sake of the course, we ensure that the default user always exists
            DefaultDataServices.EnsureDefaultUser();

            if (string.IsNullOrWhiteSpace(args.Name) || string.IsNullOrWhiteSpace(args.Password))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "User name and password cannot be empty");

            var user = _dataContext.FindUserByName(args.Name);
            if (user == null || !user.Password.Equals(args.Password))
                throw new HttpResponseException(HttpStatusCode.Forbidden, "Invalid user name or password");

            var token = AuthenticationServices.SetCurrentUser(user.Name);
            if (token == null)
                throw new HttpResponseException(HttpStatusCode.Forbidden, "Authentication error");

            AuthenticationServices.AddAuthCookie(this.Response, token);
            return token;
        }

        // DELETE: api/auth
        [HttpDelete]
        public void Logout(string token = null)
        {
            AuthenticationServices.ClearLoggedInUser(HttpContext, token);
        }
    }
}