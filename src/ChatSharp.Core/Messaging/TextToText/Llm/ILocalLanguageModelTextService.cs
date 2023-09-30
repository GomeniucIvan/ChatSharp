using ChatSharp.Core.Data;
using ChatSharp.Core.Messaging.TextToText.Llm.Settings;
using ChatSharp.Core.Platform.Messaging.Dto;
using LLama;
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
        LLamaWeights _model;

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
                var llama = _services.GetRequiredService<LlmModel>();
                llama.LoadModel(Path.Combine(_settings.ModelsPath, "llama-2-13b-chat.Q4_K_M.gguf"));
                var executor = llama.GetStatelessExecutor();

                var inferenceParams = new InferenceParams()
                {
                    Temperature = 0.1f,
                    MaxTokens = 64
                };

                foreach (var response in executor.Infer(helper.EnteredMessage, inferenceParams))
                {
                    await onMessageReceived(response);
                }

                return true;
            }

            throw new Exception("Todo implement additional text to text");
        }

        #endregion
    }
}
