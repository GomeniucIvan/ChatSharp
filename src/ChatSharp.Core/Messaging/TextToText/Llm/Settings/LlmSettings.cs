using ChatSharp.Core.Platform.Confirguration.Domain;

namespace ChatSharp.Core.Messaging.TextToText.Llm.Settings
{
    public class LlmSettings : ISettings
    {
        public bool EnableLlm { get; set; } = true;
        public string ModelPath { get; set; } = "D:/Models";
    }
}
