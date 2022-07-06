using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SpecFlowMasterClass.SpecOverflow.Web.DataAccess;
using SpecFlowMasterClass.SpecOverflow.Web.Models;
using SpecFlowMasterClass.SpecOverflow.Web.Services;
using Microsoft.AspNetCore.Mvc;
using SpecFlowMasterClass.SpecOverflow.Web.Utils;

namespace SpecFlowMasterClass.SpecOverflow.Web.Controllers
{
    /// <summary>
    /// Processes user management requests
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _dataContext = new();
        private readonly ModelTransformationService _modelTransformationService;

        public UserController()
        {
            _modelTransformationService = new ModelTransformationService(_dataContext);
        }

        // POST /api/user -- registers a user
        [HttpPost]
        public UserReferenceModel Register(RegisterInputModel registerModel)
        {
            if (string.IsNullOrEmpty(registerModel.UserName))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Name must be provided");
            if (string.IsNullOrEmpty(registerModel.Password) || string.IsNullOrEmpty(registerModel.PasswordReEnter))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Password and password re-enter must be provided");
            if (registerModel.Password != registerModel.PasswordReEnter)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Re-entered password is different");
            if (registerModel.Password.Length < 4)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Password must be at least 4 characters long");

            var existingUser = _dataContext.Users.FirstOrDefault(u => u.Name == registerModel.UserName);
            if (existingUser != null)
                _dataContext.Users.Remove(existingUser);

            var user = new User
            {
                Name = registerModel.UserName,
                Password = registerModel.Password
            };
            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();

            return _modelTransformationService.ToUserReference(user);
        }

        // GET: api/user -- returns all users
        [HttpGet]
        public List<UserReferenceModel> GetUsers(string token = null)
        {
            AuthenticationServices.EnsureAdminAuthenticated(HttpContext, token);

            var users = _dataContext.Users.OrderBy(u => u.Name);
            
            return users.Select(_modelTransformationService.ToUserReference).ToList();
        }

        // GET: api/user/[guid] -- returns user details
        [HttpGet("{id}")]
        public UserReferenceModel GetUser(Guid id, string token = null)
        {
            var currentUserName = AuthenticationServices.EnsureAuthenticated(HttpContext, token);

            var user = _dataContext.Users.FirstOrDefault(u => u.Id == id);

            //only admins or the user with the ID should be able to call this
            if (user == null || user.Name != currentUserName)
                AuthenticationServices.EnsureAdminAuthenticated(HttpContext, token);

            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return _modelTransformationService.ToUserReference(user);
        }
    }
}
