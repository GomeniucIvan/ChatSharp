using ChatSharp.Core.Platform.Confirguration.Domain;

namespace ChatSharp.Core.Platform.Configuration.Services
{
    public interface ISettingService
    {
        public Task SaveSettingsAsync(ISettings settings);
    }
}
