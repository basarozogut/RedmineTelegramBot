using Microsoft.Extensions.Logging;
using RedmineTelegramBot.Core.Models;
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
        private readonly IConversationHandlerFactory _conversationHandlerFactory;

        private readonly Dictionary<long, ConversationStateModel> _conversationStates = new Dictionary<long, ConversationStateModel>();

        public RedmineBot(
            ILogger<RedmineBot> logger,
            ITelegramBotClient telegramBotClient,
            IConversationHandlerFactory conversationFactory)
        {
            _logger = logger;
            _telegramBotClient = telegramBotClient;
            _conversationHandlerFactory = conversationFactory;
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
            var username = e.Message.Chat.Username;
            var chatId = e.Message.Chat.Id;

            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    await _telegramBotClient.SendTextMessageAsync(e.Message.Chat, "You must have a username to use this bot.");
                    return;
                }

                if (!_conversationStates.ContainsKey(chatId))
                {
                    _conversationStates[chatId] = new ConversationStateModel()
                    {
                        ChatId = chatId,
                        Username = username
                    };
                }
                var handler = _conversationHandlerFactory.CreateConversationHandler(_conversationStates[chatId]);

                await handler.Handle(e.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while processing message.");

                try
                {
                    _conversationStates[chatId] = null;
                    await _telegramBotClient.SendTextMessageAsync(e.Message.Chat, "An error occured while processing your request. Conversation has been reset.");
                }
                catch (Exception)
                {
                    _logger.LogError(ex, "An error occured while resetting conversation.");
                }
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
