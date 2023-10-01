using ChatSharp.Core.Data;
using ChatSharp.Core.Messaging.TextToText.Llm.Settings;
using ChatSharp.Core.Platform.Messaging.Dto;
using ChatSharp.Core.Platform.Messaging.Proc;
using ChatSharp.Domain;
using ChatSharp.Engine;
using ChatSharp.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChatSharp.Web.Controllers
{
    public class StartupController : BaseController
    {
        private readonly IApplicationContext _appContext;
        private readonly LlmSettings _llmSettings;
        private readonly ChatSharpDbContext _dbContext;

        public StartupController(IApplicationContext appContext,
            LlmSettings llmSettings,
            ChatSharpDbContext dbContext)
        {
            _appContext = appContext;
            _llmSettings = llmSettings;
            _dbContext = dbContext;
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

        [Route("StartupChatData")]
        public async Task<IActionResult> LoadChatData(string conversationGuid)
        {
            var genericModel = new GenericResponse<ChatModel>();

            string[] binFiles = Directory.GetFiles(_llmSettings.ModelsPath, "*.gguf", SearchOption.TopDirectoryOnly);

            IList<SelectListItem> models = binFiles.Select(file => 
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                return new SelectListItem
                {
                    Text = fileName,
                    Value = fileName
                };
            }).ToList();

            var chatModel = new ChatModel()
            {
                DefaultModelName = _llmSettings.DefaultModel,
                Sessions = _dbContext.Session_GetList(new SessionDtoFilter()),
                Models = models
            };

            return Ok(genericModel.Success(chatModel));
        }
    }
}
