﻿using LLama.Abstractions;

namespace ChatSharp.Core.Platform.Messaging.Dto
{
    public class MessageDtoHelper
    {
        public int CustomerId { get; set; } = 1;
        public string EnteredMessage { get; set; }
        public string WorkingModel { get; set; }
        public Guid ModelGuid { get; set; }
        public bool SaveSession { get; set; }
        public SessionDto DbSession { get; set; }
        public int? CreatedSessionId { get; set; }
    }
}
