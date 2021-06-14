using System;
using SpecFlowMasterClass.SpecOverflow.Specs.API.Support;
using SpecFlowMasterClass.SpecOverflow.Web.Models;

namespace SpecFlowMasterClass.SpecOverflow.Specs.API.Drivers
{
    public class HomeApiDriver
    {
        private readonly WebApiContext _webApiContext;

        public HomeApiDriver(WebApiContext webApiContext)
        {
            _webApiContext = webApiContext;
        }

        public HomePageModel GetHomePageModel()
        {
            return _webApiContext.ExecuteGet<HomePageModel>("/api/home");
        }
    }
}
