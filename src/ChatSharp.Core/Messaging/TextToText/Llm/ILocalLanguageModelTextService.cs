using ChatSharp.Core.Data;
using ChatSharp.Core.Messaging.TextToText.Llm.Settings;
using ChatSharp.Core.Platform.Messaging.Dto;
using ChatSharp.Core.Platform.Messaging.Proc;
using ChatSharp.Extensions;
using LLama;
using LLama.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChatSharp.Core.Messaging.TextToText.Llm
{
    public class LocalLanguageModelTextService : ITextToTextService
    {
        #region Fields

        private readonly ChatSharpDbContext _dbContext;
        private readonly LlmSettings _settings;
        private readonly IServiceProvider _services;
        private ChatSession _session;
        private LlmModel _llmModel;

        private string instructions =
            "Transcript of a dialog, where the User interacts with an Assistant named Bob. Bob is helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision.";

        public int Order => int.MinValue + 1;

        #endregion

        #region Ctor

        public LocalLanguageModelTextService(ChatSharpDbContext dbContext,
            LlmSettings settings, 
            IServiceProvider services)
        {
            _dbContext = dbContext;
            _settings = settings;
            _services = services;
        }

        #endregion

        #region Methods

        public async Task<bool> HandleTextRequestAsync(MessageDtoHelper helper, 
            Func<string, Task> onMessageReceived,
            CancellationToken cancellationToken)
        {
            if (_settings.EnableLLM)
            {
                var inferenceParams = new InferenceParams()
                {
                    Temperature = 0.6f,
                    AntiPrompts = new List<string> { "User:" }
                };

                if (_session == null)
                {
                    helper.WorkingModel ??= _settings.DefaultModel;

                    _llmModel = _services.GetRequiredService<LlmModel>();
                    _session = _llmModel.CreateSession(Path.Combine(_settings.ModelsPath, helper.WorkingModel));

                    if (helper.DbSession != null)
                    {
                        _session = await LoadSessionAsync(helper.DbSession);
                    }
                    else
                    {
                        //instructions, todo find a better way
                        foreach (var response in _session.Chat(instructions, inferenceParams))
                        {
                        
                        } 
                    }
                }

                foreach (var response in _session.Chat(helper.EnteredMessage, inferenceParams))
                {
                    await onMessageReceived(response);
                }

                if (helper.SaveSession)
                {
                    var folderPathToSave = Path.Combine(_settings.PathToSaveSessions, $"{helper.ModelGuid.ToString().ToLower()}");

                    _session.SaveSession(folderPathToSave);
                }

                return true;
            }

            ConsoleExtensions.ErrorWriteLine(new Exception("Todo implement additional text to text"));
            throw new Exception("Todo implement additional text to text");
        }

        public async Task<ChatSession> LoadSessionAsync(SessionDto dbSession)
        {
            LlmModel llama = _services.GetRequiredService<LlmModel>();
            ChatSession chatSession = llama.LoadSession(dbSession);

            return chatSession;
        }

        #endregion
    }
}
