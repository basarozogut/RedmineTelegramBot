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
        private readonly IWorkContext _workContext;

        public RestClientFactory(
            BotOptions botOptions,
            IWorkContext workContext)
        {
            _botOptions = botOptions;
            _workContext = workContext;
        }

        public IRestClient CreateRestClient()
        {
            var client = new RestClient(_botOptions.RedmineUrl);
            client.AddDefaultHeader("X-Redmine-API-Key", _workContext.RedmineSecret);

            return client;
        }
    }
}
