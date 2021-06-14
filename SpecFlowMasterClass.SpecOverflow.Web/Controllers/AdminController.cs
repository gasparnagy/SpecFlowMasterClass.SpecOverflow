using System;
using Microsoft.AspNetCore.Mvc;

namespace SpecFlowMasterClass.SpecOverflow.Web.Controllers
{
    /// <summary>
    /// Processes admin requests (exposed on Admin page)
    /// </summary>
    [Route("api/admin/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
    }
}
