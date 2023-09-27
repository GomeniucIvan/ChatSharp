using ChatSharp.Core.Data;
using ChatSharp.Core.Platform.Configuration.Dto;
using ChatSharp.Core.Platform.Configuration.Proc;
using ChatSharp.Core.Platform.Confirguration.Domain;

namespace ChatSharp.Core.Platform.Configuration.Services
{
    public class SettingService : ISettingService
    {
        private readonly ChatSharpDbContext _dbContext;
        private readonly IList<SettingDto> _setSettings;

        public SettingService(ChatSharpDbContext dbContext)
        {
            _dbContext = dbContext;
            _setSettings = dbContext.Settings_GetList();
        }

        public async Task SaveSettingsAsync(ISettings settings)
        {
            // Use reflection to get property names and values
            var properties = settings.GetType().GetProperties();
            foreach (var property in properties)
            {
                var name = property.Name;
                var value =  property.GetValue(settings)?.ToString();

                var existingSetting = _setSettings.FirstOrDefault(s => s.Name == name);
                if (existingSetting != null)
                {
                    existingSetting.Value = value;
                }
                else
                {
                    // Insert new setting
                    _dbContext.Settings.Add(new Setting { Name = name, Value = value });
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
