using Microsoft.Extensions.DependencyInjection;
using RedmineTelegramBot.Core;
using RedmineTelegramBot.Core.Config;
using RedmineTelegramBot.Core.Modules;
using Telegram.Bot;

namespace RedmineTelegramBot.Telegram
{
    public class TelegramModule : IDependencyModule
    {
        private readonly BotOptions _options;

        public TelegramModule(BotOptions configuration)
        {
            _options = configuration;
        }

        public void Load(IServiceCollection services)
        {
            services.AddSingleton<ITelegramBotClient>(c => new TelegramBotClient(_options.Token));
            services.AddTransient<IChatClient, TelegramChatClient>();
        }
    }
}
