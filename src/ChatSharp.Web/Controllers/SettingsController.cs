using ChatSharp.Core.Messaging.TextToText.Llm.Settings;
using ChatSharp.Core.Platform.Configuration.Services;
using ChatSharp.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ChatSharp.Web.Controllers
{
    public class SettingsController : BaseController
    {
        #region Fields

        private readonly ISettingService _settingService;
        private readonly LlmSettings _llmSettings;

        #endregion

        #region Ctor

        public SettingsController(ISettingService settingService,
            LlmSettings llmSettings)
        {
            _settingService = settingService;
            _llmSettings = llmSettings;
        }

        #endregion

        #region Methods

        [Route("SettingsLoad")]
        [HttpGet]
        public async Task<IActionResult> Load()
        {
            return Ok(new GenericResponse<LlmSettings>().Success(_llmSettings));
        }

        [Route("SettingsSave")]
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] LlmSettings settings)
        {
            await _settingService.SaveSettingsAsync(settings);

            return Ok(new GenericResponse<bool>().Success(true));
        }

        #endregion
    }
}
