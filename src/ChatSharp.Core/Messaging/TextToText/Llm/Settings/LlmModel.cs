﻿using ChatSharp.Core.Platform.Messaging.Dto;
using ChatSharp.Extensions;
using LLama;
using LLama.Common;

namespace ChatSharp.Core.Messaging.TextToText.Llm.Settings
{
    public class LlmModel
    {
        private readonly LlmSettings _settings;
        private ChatSession _session;
        public LlmModel(LlmSettings settings)
        {
            _settings = settings;
        }

        public ChatSession CreateSession(string modelUrl)
        {
            if (!File.Exists(modelUrl))
            {
                ConsoleExtensions.ErrorWriteLine(new Exception("Model file not found."));
                throw new Exception("Model file not found.");
            }

            var modelParams = new ModelParams(modelUrl)
            {
                ContextSize = 1024,
                Seed = 1337,
                GpuLayerCount = 5
            };

            var model = LLamaWeights.LoadFromFile(modelParams);
            var context = model.CreateContext(modelParams);
            var ex = new InteractiveExecutor(context);
            _session = new ChatSession(ex).WithOutputTransform(new LLamaTransforms.KeywordTextOutputStreamTransform(new string[] { "User:", "Bob:" }, redundancyLength: 8));
            return _session;
        }

        public ChatSession LoadSession(SessionDto dbSession)
        {
            var sessionPathToLoad = Path.Combine(_settings.PathToSaveSessions, $"{dbSession.Guid.ToString().ToLower()}");

            if (!Directory.Exists(sessionPathToLoad))
            {
                ConsoleExtensions.ErrorWriteLine(new Exception("Session not exists."));
                throw new Exception("Session not exists.");
            }

            if (_session != null)
            {
                _session = null;
            }

            _session = CreateSession(Path.Combine(_settings.ModelsPath, dbSession.ModelName));
            _session.LoadSession(sessionPathToLoad);

            return _session;
        }
    }
}
