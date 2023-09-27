using ChatSharp.Core.Data;
using ChatSharp.Core.Platform.Confirguration.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace ChatSharp.Core.Starter
{
    public static class ProgramSettingStarter
    {
        public static void ConfigureSettings(this IServiceCollection services)
        {
            var settingsAssembly = typeof(ISettings).Assembly;
            var settingsTypes = settingsAssembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ISettings)));

            foreach (var settingsType in settingsTypes)
            {
                services.AddTransient(settingsType, provider =>
                {
                    var dbContext = provider.GetRequiredService<ChatSharpDbContext>();
                    var instance = Activator.CreateInstance(settingsType);

                    // Get properties of the type
                    var properties = settingsType.GetProperties();
                    foreach (var property in properties)
                    {
                        // Get corresponding setting value from the database
                        var setting = dbContext.Settings.SingleOrDefault(s => s.Name == property.Name);
                        if (setting != null)
                        {
                            // Set property value
                            var value = Convert.ChangeType(setting.Value, property.PropertyType);
                            property.SetValue(instance, value);
                        }
                    }

                    return instance;
                });
            }
        }
    }
}
