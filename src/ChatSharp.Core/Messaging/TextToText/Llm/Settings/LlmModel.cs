using LLama;
using LLama.Abstractions;
using LLama.Common;

namespace ChatSharp.Core.Messaging.TextToText.Llm.Settings
{
    public class LlmModel
    {
        private readonly LlmSettings _settings;
        private LLamaWeights _model;
        private ModelParams _params;

        private ILLamaExecutor _statelessExecutor;

        public LlmModel(LlmSettings settings)
        {
            _settings = settings;
        }


        public void LoadModel(string modelUrl)
        {
            if (_model != null)
            {
                return;
            }

            _params = new ModelParams(modelUrl)
            {
                ContextSize = 1024,
                Seed = 1337,
                GpuLayerCount = 5
            };

            _model = LLamaWeights.LoadFromFile(_params);
        }

        public ILLamaExecutor GetStatelessExecutor()
        {
            if (_statelessExecutor == null)
            {
                _statelessExecutor = new StatelessExecutor(_model, _params);
            }
            return _statelessExecutor;
        }
    }
}
