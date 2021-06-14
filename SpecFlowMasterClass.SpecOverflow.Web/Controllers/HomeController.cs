using System.Linq;
using SpecFlowMasterClass.SpecOverflow.Web.Models;
using SpecFlowMasterClass.SpecOverflow.Web.Services;
using Microsoft.AspNetCore.Mvc;
using SpecFlowMasterClass.SpecOverflow.Web.DataAccess;

namespace SpecFlowMasterClass.SpecOverflow.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly DataContext _dataContext = new();
        private readonly ModelTransformationService _modelTransformationService;

        public HomeController()
        {
            _modelTransformationService = new ModelTransformationService(_dataContext);
        }

        // GET: api/home
        [HttpGet]
        public HomePageModel GetHomePageModel(string token = null)
        {
            var model = new HomePageModel();
            model.MainMessage = "Welcome to Spec Overflow!";
            model.UserName = AuthenticationServices.GetCurrentUserName(HttpContext, token);
            model.IsAdmin = AuthenticationServices.IsAdmin(HttpContext, token);

            model.LatestQuestions = _dataContext.Questions
                .OrderByDescending(q => q.AskedAt)
                .Take(10)
                .Select(q => _modelTransformationService.ToQuestionSummary(q))
                .ToList();
            
            return model;
        }
    }
}