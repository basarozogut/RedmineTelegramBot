using Microsoft.Extensions.DependencyInjection;
using RedmineTelegramBot.Core.Config;
using RedmineTelegramBot.Core.Data;

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
            // config
            services.AddSingleton(_options);

            // repositories
            services.AddTransient<IUserSettingsRepository>(c => new UsersSettingsFileRepository(_options.DataDirectory));
            services.AddSingleton<IConversationStateRepository, InMemoryConversationStateRepository>();

            // context
            services.AddScoped<IWorkContext, WorkContext>();

            // app services
            services.AddTransient<IRedmineBot, RedmineBot>();
            services.AddTransient<IConversationHandler, ConversationHandler>();
            services.AddTransient<IRestClientFactory, RestClientFactory>();
            services.AddTransient<IRedmineApiClient, RedmineApiClient>();
        }
    }
}
