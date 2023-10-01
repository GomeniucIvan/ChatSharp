using ChatSharp.Core.Platform.Messaging.Dto;
using LLama;

namespace ChatSharp.Core.Messaging.TextToText
{
    public interface ITextToTextService
    {
        public Task<bool> HandleTextRequestAsync(MessageDtoHelper helper,
            Func<string, Task> onMessageReceived,
            CancellationToken cancellationToken);

        public int Order { get; }
        public Task<ChatSession> LoadSessionAsync(SessionDto dbSession);
    }
}
