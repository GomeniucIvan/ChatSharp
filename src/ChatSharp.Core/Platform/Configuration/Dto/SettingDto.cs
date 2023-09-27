using ChatSharp.Domain;

namespace ChatSharp.Core.Platform.Configuration.Dto
{
    public class SettingDto : BaseDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
