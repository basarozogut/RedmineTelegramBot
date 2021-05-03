using RedmineTelegramBot.Core.Config;
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

        public RestClientFactory(BotOptions botOptions)
        {
            _botOptions = botOptions;
        }

        public IRestClient CreateRestClient()
        {
            var client = new RestClient(_botOptions.RedmineUrl);
            client.AddDefaultHeader("X-Redmine-API-Key", _botOptions.RedmineSecret);

            return client;
        }
    }
}
