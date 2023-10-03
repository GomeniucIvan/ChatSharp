using ChatSharp.Domain;

namespace ChatSharp.Core.Platform.Messaging.Dto
{
    public class SessionMessageDto : BaseDto
    {
        public int SessionId { get; set; }
        public bool IsMine { get; set; }
        public string Message { get; set; }
    }
}