using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RedmineTelegramBot.Core.Config;
using RedmineTelegramBot.Core.Data;
using RedmineTelegramBot.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace RedmineTelegramBot.Core
{
    public class RedmineBot : IRedmineBot
    {
        private readonly ILogger<RedmineBot> _logger;
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly BotOptions _botOptions;

        public RedmineBot(
            ILogger<RedmineBot> logger,
            ITelegramBotClient telegramBotClient,
            IServiceProvider serviceProvider,
            BotOptions botOptions)
        {
            _logger = logger;
            _telegramBotClient = telegramBotClient;
            _serviceProvider = serviceProvider;
            _botOptions = botOptions;
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

            if (!_botOptions.AllowedUsers.Contains(username))
            {
                await _telegramBotClient.SendTextMessageAsync(e.Message.Chat, "You are not allowed to interact with the bot.");
                return;
            }

            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    await _telegramBotClient.SendTextMessageAsync(e.Message.Chat, "You must have a username to use this bot.");
                    return;
                }

                await HandleRequest(e.Message, username, chatId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while processing message.");
                await _telegramBotClient.SendTextMessageAsync(e.Message.Chat, "An error occured while processing your request.");
            }
        }

        private async Task HandleRequest(Message message, string username, long chatId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var workContextSetter = (IWorkContextSetter)scope.ServiceProvider.GetRequiredService<IWorkContext>();
                workContextSetter.ChatId = chatId;
                workContextSetter.Username = username;

                var userSettingsRepo = scope.ServiceProvider.GetRequiredService<IUserSettingsRepository>();
                var userSettings = userSettingsRepo.GetSettings(username);
                if (userSettings != null)
                {
                    workContextSetter.RedmineSecret = userSettings.RedmineSecret;
                }

                var conversationStateRepo = scope.ServiceProvider.GetRequiredService<IConversationStateRepository>();
                var conversationState = conversationStateRepo.GetConversationState(username);
                if (conversationState == null)
                {
                    conversationState = new ConversationState()
                    {
                        ChatId = chatId,
                        Username = username
                    };
                    conversationStateRepo.StoreConversationState(conversationState);
                }

                try
                {
                    var handler = scope.ServiceProvider.GetRequiredService<IConversationHandler>();

                    await handler.Handle(message);

                    handler.SaveState();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occured while processing message.");

                    try
                    {
                        if (conversationStateRepo.GetConversationState(username) != null)
                        {
                            conversationStateRepo.DeleteConversationState(username);
                        }
                        await _telegramBotClient.SendTextMessageAsync(message.Chat, "An error occured while processing your request. Conversation has been reset.");
                    }
                    catch (Exception innerEx)
                    {
                        _logger.LogError(innerEx, "An error occured while resetting conversation.");
                    }
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
