using ChatSharp.Core.Messaging.TextToText;
using ChatSharp.Core.Platform.Messaging.Dto;
using ChatSharp.Domain;
using ChatSharp.Web.Models.Conversation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatSharp.Web.Controllers
{
    public class ConversationController : BaseController
    {
        #region Fields

        private readonly ITextToTextService _textToTextService;
        private readonly IHubContext<ChatSharpHub> _hubContext;

        #endregion

        #region Ctor

        public ConversationController(ITextToTextService textToTextService, 
            IHubContext<ChatSharpHub> hubContext)
        {
            _textToTextService = textToTextService;
            _hubContext = hubContext;
        }

        #endregion

        [Route("ConversationSendMessage")]
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ConversationMessageModel model,
            CancellationToken cancelToken = default)
        {
            var genericModel = new GenericResponse<string>();

            if (model == null || string.IsNullOrEmpty(model.Message))
            {
                return Ok(genericModel.Error("Empty message"));
            }

            var helper = new MessageDtoHelper()
            {
                EnteredMessage = model.Message,
            };

            var resultMessage = "";
            var result = await _textToTextService.HandleTextRequestAsync(helper, async msg =>
            {
                resultMessage += msg;
                await _hubContext.Clients.All.SendAsync($"OnMessageReceive", new object[]
                {
                    msg
                }, cancellationToken: cancelToken);
            }, cancelToken);

            return Ok(genericModel.Success(resultMessage));
        }
    }
}
