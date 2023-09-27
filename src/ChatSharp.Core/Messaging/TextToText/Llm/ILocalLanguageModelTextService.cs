using ChatSharp.Core.Data;
using ChatSharp.Core.Messaging.TextToText.Llm.Settings;
using ChatSharp.Core.Platform.Messaging.Dto;

namespace ChatSharp.Core.Messaging.TextToText.Llm
{
    public class LocalLanguageModelTextService : ITextToTextService
    {
        #region Fields

        private readonly ChatSharpDbContext _dbContext;
        private readonly LlmSettings _settings;

        public int Order => int.MinValue + 1;

        #endregion

        #region Ctor

        public LocalLanguageModelTextService(ChatSharpDbContext dbContext,
            LlmSettings settings)
        {
            _dbContext = dbContext;
            _settings = settings;
        }

        #endregion

        #region Methods

        public async Task<bool> HandleTextRequestAsync(MessageDtoHelper helper, 
            Func<string, Task> onMessageReceived,
            CancellationToken cancellationToken)
        {
            if (_settings.EnableLlm)
            {
                //foreach (var text in session.Chat(helper.EnteredMessage, new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "User:" } }))
                //{
                //    // Text response received
                //    await onMessageReceived(text);
                //}

                return true;
            }

            throw new Exception("Todo implement additional text to text");
        }

        #endregion

    }
}
