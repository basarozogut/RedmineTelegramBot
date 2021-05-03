using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RedmineTelegramBot.Core
{
    public class ConversationFactory : IConversationFactory
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IRestClientFactory _restClientFactory;

        public ConversationFactory(ITelegramBotClient telegramBotClient, IRestClientFactory restClientFactory)
        {
            _telegramBotClient = telegramBotClient;
            _restClientFactory = restClientFactory;
        }

        public IConversation CreateConversation()
        {
            return new Conversation(_telegramBotClient, _restClientFactory);
        }
    }
}
