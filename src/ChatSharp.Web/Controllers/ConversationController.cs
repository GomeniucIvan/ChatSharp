using ChatSharp.Core.Data;
using ChatSharp.Core.Messaging.TextToText;
using ChatSharp.Core.Messaging.TextToText.Llm.Settings;
using ChatSharp.Core.Platform.Messaging.Domain;
using ChatSharp.Core.Platform.Messaging.Dto;
using ChatSharp.Core.Platform.Messaging.Proc;
using ChatSharp.Domain;
using ChatSharp.Extensions;
using ChatSharp.Web.Models.Conversation;
using LLama.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatSharp.Web.Controllers
{
    public class ConversationController : BaseController
    {
        #region Fields

        private readonly ITextToTextService _textToTextService;
        private readonly IHubContext<ChatSharpHub> _hubContext;
        private readonly ChatSharpDbContext _dbContext;
        private readonly LlmSettings _settings;

        #endregion

        #region Ctor

        public ConversationController(ITextToTextService textToTextService, 
            IHubContext<ChatSharpHub> hubContext,
            ChatSharpDbContext dbContext,
            LlmSettings settings)
        {
            _textToTextService = textToTextService;
            _hubContext = hubContext;
            _dbContext = dbContext;
            _settings = settings;
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
                return Ok(genericModel.Error("Empty message."));
            }

            if (Guid.TryParse(model.ModelGuid, out var guidValue))
            {
                
            }
            else
            {
                return Ok(genericModel.Error("Invalid guid."));
            }

            var dbSession = _dbContext.Session_GetByFilter(new SessionDtoFilter()
            {
                Guid = model.ModelGuid
            });

            var helper = new MessageDtoHelper()
            {
                EnteredMessage = model.Message,
                ModelGuid = guidValue,
                DbSession = dbSession
            };

            if (dbSession == null)
            {
                var newSessionHelper = new MessageDtoHelper()
                {
                    EnteredMessage = $"Provide one very short title for: {model.Message}, without additional text",
                };

                var titleResultMessage = "";

                var titleResult = await _textToTextService.HandleTextRequestAsync(newSessionHelper, async msg =>
                {
                    titleResultMessage += msg;
                }, cancelToken);

                await _hubContext.Clients.All.SendAsync($"OnNewSessionCreate", new List<string>()
                {
                    titleResultMessage,
                    guidValue.ToString()
                }, cancellationToken: cancelToken);

                _dbContext.Session_Save(new Session()
                {
                    AutoDeleteAfterXDays = 0,
                    Guid = guidValue,
                    ModelName = model.WorkingModel.IsEmpty() ? _settings.DefaultModel : model.WorkingModel,
                    Name = titleResultMessage
                });
            }

            var resultMessage = "";
            helper.SaveSession = true;
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

        [Route("ConversationLoadSessionHistory")]
        [HttpGet]
        public async Task<IActionResult> LoadSessionHistory(string guid,
            CancellationToken cancelToken = default)
        {
            var genericModel = new GenericResponse<IList<SessionMessageModel>>();
            if (guid.IsEmpty())
            {
                return Ok(genericModel.Error("Wrong guid."));
            }

            SessionDto dbSession = _dbContext.Session_GetByFilter(new SessionDtoFilter()
            {
                Guid = guid
            });

            if (dbSession == null)
            {
                return Ok(genericModel.Error("Session not found."));
            }

            var chatSession = await _textToTextService.LoadSessionAsync(dbSession);

            var sessionMessages = new List<SessionMessageModel>();
            foreach(var message in chatSession.History.Messages)
            {
                sessionMessages.Add(new SessionMessageModel()
                {
                    IsMine = (message.AuthorRole == AuthorRole.User),
                    Message = message.Content
                });
            }

            return Ok(genericModel.Success(sessionMessages));
        }
    }
}
