using ChatSharp.Domain;

namespace ChatSharp.Core.Platform.Messaging.Dto
{
    public class SessionDto : BaseDto
    {
        public string Name { get; set; }
        public string ModelName { get; set; }
        public Guid Guid { get; set; }
    }

    public class SessionDtoFilter
    {
        public string Guid { get; set; }
        public string ModelName { get; set; }
    }
}
