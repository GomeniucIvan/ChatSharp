using ChatSharp.Domain;
using ChatSharp.Engine;
using ChatSharp.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace ChatSharp.Web.Controllers
{
    public class StartupController : BaseController
    {
        private readonly IApplicationContext _appContext;

        public StartupController(IApplicationContext appContext)
        {
            _appContext = appContext;
        }

        [Route("Startup")]
        public async Task<IActionResult> Startup()
        {
            var genericModel = new GenericResponse<StartupModel>();

            var installModel = new StartupModel()
            {
                IsInstalled = _appContext.IsDatabaseInstalled,
                IsDevelopment = _appContext.IsDatabaseInstalled
            };

            return Ok(genericModel.Success(installModel));
        }
    }
}
