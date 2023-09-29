using Autofac;
using Autofac.Core;
using ChatSharp.Core.Data;
using ChatSharp.Core.Messaging.TextToText;
using ChatSharp.Core.Messaging.TextToText.Llm;
using ChatSharp.Core.Messaging.TextToText.Llm.Settings;
using ChatSharp.Core.Platform.Configuration.Services;
using ChatSharp.Engine;

namespace ChatSharp.Core.Starter
{
    public static class ProgramContainerStarter
    {
        public static void ConfigureContainer(this ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationContext>().As<IApplicationContext>().InstancePerLifetimeScope();
            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerLifetimeScope();
            builder.RegisterType<LocalLanguageModelTextService>().As<ITextToTextService>().InstancePerLifetimeScope();
        }
    }
}
