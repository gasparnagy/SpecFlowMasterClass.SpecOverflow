using System;
using SpecFlowMasterClass.SpecOverflow.Web.DataAccess;
using SpecFlowMasterClass.SpecOverflow.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SpecFlowMasterClass.SpecOverflow.Web.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// API to enable testing back-doors. Should not be deployed in production.
    /// </summary>
    [Route("api/test/[action]")]
    [ApiController]
    public class TestApiController : ControllerBase
    {
        // POST /api/test/Reset -- clears up the database
        [HttpPost]
        public IActionResult Reset()
        {
            AuthenticationServices.ClearLoggedInUser(HttpContext);
            var dataContext = new DataContext();
            dataContext.TruncateTables();
            return NoContent();
        }

        // POST /api/test/Seed -- clears up the database and adds default data
        [HttpPost]
        public IActionResult Seed()
        {
            AuthenticationServices.ClearLoggedInUser(HttpContext);
            var dataContext = new DataContext();
            dataContext.TruncateTables();
            DefaultDataServices.SeedWithDefaultData(dataContext);
            return NoContent();
        }

        // POST /api/test/DefaultLogin -- logs in with a default user
        [HttpPost]
        public IActionResult DefaultLogin()
        {
            DefaultDataServices.EnsureDefaultUser();
            var token = AuthenticationServices.SetCurrentUser(DefaultDataServices.DefaultUserName);
            if (token == null)
                return StatusCode(StatusCodes.Status403Forbidden);

            AuthenticationServices.AddAuthCookie(this.Response, token);
            return Content(token);
        }
    }
}
