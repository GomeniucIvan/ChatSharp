using ChatSharp.Core.Platform.Messaging.Dto;

namespace ChatSharp.Core.Messaging.TextToText
{
    public interface ITextToTextService
    {
        public Task<bool> HandleTextRequestAsync(MessageDtoHelper helper,
            Func<string, Task> onMessageReceived,
            CancellationToken cancellationToken);

        public int Order { get; }
    }
}
