using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RedmineTelegramBot.Core
{
    public class RedmineBot : IRedmineBot
    {
        private readonly ILogger<RedmineBot> _logger;
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IConversationFactory _conversationFactory;

        private readonly Dictionary<long, IConversation> _conversations = new Dictionary<long, IConversation>();

        public RedmineBot(
            ILogger<RedmineBot> logger,
            ITelegramBotClient telegramBotClient,
            IConversationFactory conversationFactory)
        {
            _logger = logger;
            _telegramBotClient = telegramBotClient;
            _conversationFactory = conversationFactory;
        }

        public Task Start()
        {
            _logger.LogInformation("Bot started.");

            _telegramBotClient.OnMessage += _telegramBotClient_OnMessage;
            _telegramBotClient.StartReceiving();
            _telegramBotClient.SetMyCommandsAsync(Commands.GetBotCommands());

            return Task.CompletedTask;
        }

        private async void _telegramBotClient_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            try
            {
                var chatId = e.Message.Chat.Id;
                if (!_conversations.ContainsKey(chatId))
                {
                    _conversations[chatId] = _conversationFactory.CreateConversation();
                }
                await _conversations[chatId].Process(e.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while processing message.");
            }
        }

        public Task Stop()
        {
            _telegramBotClient.StopReceiving();

            _logger.LogInformation("Bot stopped.");

            return Task.CompletedTask;
        }
    }
}
