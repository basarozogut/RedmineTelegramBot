using RedmineTelegramBot.Core.Config;
using RedmineTelegramBot.Core.Data;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core
{
    public class RestClientFactory : IRestClientFactory
    {
        private readonly BotOptions _botOptions;
        private readonly UserSettings _userSettings;

        public RestClientFactory(BotOptions botOptions, UserSettings userSettings)
        {
            _botOptions = botOptions;
            _userSettings = userSettings;
        }

        public IRestClient CreateRestClient()
        {
            var client = new RestClient(_botOptions.RedmineUrl);
            client.AddDefaultHeader("X-Redmine-API-Key", _userSettings.RedmineSecret);

            return client;
        }
    }
}
