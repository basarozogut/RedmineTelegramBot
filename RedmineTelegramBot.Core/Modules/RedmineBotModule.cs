using Microsoft.Extensions.DependencyInjection;
using RedmineTelegramBot.Core.Config;
using RedmineTelegramBot.Core.Data;
using Telegram.Bot;

namespace RedmineTelegramBot.Core.Modules
{
    public class RedmineBotModule : IDependencyModule
    {
        private readonly BotOptions _options;

        public RedmineBotModule(BotOptions configuration)
        {
            _options = configuration;
        }

        public void Load(IServiceCollection services)
        {
            // repositories
            services.AddTransient<IUserSettingsRepository>(c => new UsersSettingsFileRepository(_options.DataDirectory));

            // app services
            services.AddTransient<IRedmineBot, RedmineBot>();
            services.AddSingleton<ITelegramBotClient>(c => new TelegramBotClient(_options.Token));
            services.AddTransient<IConversationFactory, ConversationFactory>();
            services.AddTransient<IRestClientFactory>(c => new RestClientFactory(_options));
            services.AddTransient<IRedmineApiClient,RedmineApiClient>();
        }
    }
}
