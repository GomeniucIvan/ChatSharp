using ChatSharp.Core.Data;
using ChatSharp.Core.Messaging.TextToText.Llm.Settings;
using ChatSharp.Core.Platform.Messaging.Dto;
using ChatSharp.Extensions;
using LLama;
using LLama.Abstractions;
using LLama.Common;
using Microsoft.Extensions.DependencyInjection;
using static System.Net.Mime.MediaTypeNames;

namespace ChatSharp.Core.Messaging.TextToText.Llm
{
    public class LocalLanguageModelTextService : ITextToTextService
    {
        #region Fields

        private readonly LlmSettings _settings;
        private readonly IServiceProvider _services;

        public int Order => int.MinValue + 1;

        #endregion

        #region Ctor

        public LocalLanguageModelTextService(LlmSettings settings, 
            IServiceProvider services)
        {
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
                helper.WorkingModel ??= _settings.DefaultModel;

                LlmModel llama = _services.GetRequiredService<LlmModel>();
                ChatSession session = llama.CreateSession(Path.Combine(_settings.ModelsPath, helper.WorkingModel));

                var inferenceParams = helper.InferenceParams ?? new InferenceParams()
                {
                    Temperature = 0.8f,
                    MaxTokens = 64
                };

                foreach (var response in session.Chat(helper.EnteredMessage, inferenceParams))
                {
                    await onMessageReceived(response);
                }

                if (helper.SaveSession)
                    llama.SaveSession(helper.ModelGuid);

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
