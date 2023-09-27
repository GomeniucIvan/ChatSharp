
namespace ChatSharp.Engine
{
    public class ProgramEngineHelper
    {
        private static bool? _initialized;
        private static bool? _isDevEnvironment;

        public ProgramEngineHelper(bool isDevelopment)
        {
            _isDevEnvironment = isDevelopment;
        }

        public void Initialize()
        {
            _initialized = true;
        }

        public static bool IsDevEnvironment => _isDevEnvironment.GetValueOrDefault();
    }
}
