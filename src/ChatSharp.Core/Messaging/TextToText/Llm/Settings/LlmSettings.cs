using ChatSharp.Core.Platform.Confirguration.Domain;

namespace ChatSharp.Core.Messaging.TextToText.Llm.Settings
{
    public class LlmSettings : ISettings
    {
        public bool EnableLLM { get; set; } = true;
        public string ModelsPath { get; set; } = "D:/Models";
        public string DefaultModel { get; set; } = "llama-2-13b-chat.Q4_K_M.gguf";
        public string PathToSaveSessions { get; set; }= "D:/Models/Sessions";
    }
}
