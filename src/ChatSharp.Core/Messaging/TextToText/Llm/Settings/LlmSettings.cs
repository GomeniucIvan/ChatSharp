using ChatSharp.Core.Platform.Confirguration.Domain;

namespace ChatSharp.Core.Messaging.TextToText.Llm.Settings
{
    public class LlmSettings : ISettings
    {
        public bool EnableLLM { get; set; } = true;
        public string ModelsPath { get; set; } = "D:/Models";
    }
}
