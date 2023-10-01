using ChatSharp.Core.Platform.Messaging.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChatSharp.Web.Models.Common
{
    public class StartupModel
    {
        public bool IsInstalled { get; set; }
        public bool IsDevelopment { get; set; }
    }

    public class ChatModel
    {
        public string DefaultModelName { get; set; }
        public IList<SessionDto> Sessions { get; set; } = new List<SessionDto>();
        public IList<SelectListItem> Models { get; set; } = new List<SelectListItem>();
    }
}
