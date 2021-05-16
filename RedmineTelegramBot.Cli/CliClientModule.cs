using Microsoft.Extensions.DependencyInjection;
using RedmineTelegramBot.Core;
using RedmineTelegramBot.Core.Modules;

namespace RedmineTelegramBot.Cli
{
    public class CliClientModule : IDependencyModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddSingleton<IChatClient, CliChatClient>();
        }
    }
}
