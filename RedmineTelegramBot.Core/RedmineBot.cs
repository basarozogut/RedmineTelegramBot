using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RedmineTelegramBot.Core.Config;
using RedmineTelegramBot.Core.Data;
using RedmineTelegramBot.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core
{
    public class RedmineBot : IRedmineBot
    {
        private readonly ILogger<RedmineBot> _logger;
        private readonly IChatClient _chatClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly BotOptions _botOptions;

        public RedmineBot(
            ILogger<RedmineBot> logger,
            IChatClient chatClient,
            IServiceProvider serviceProvider,
            BotOptions botOptions)
        {
            _logger = logger;
            _chatClient = chatClient;
            _serviceProvider = serviceProvider;
            _botOptions = botOptions;
        }

        public async Task Start()
        {
            _logger.LogInformation("Bot started.");

            _chatClient.OnMessage += _telegramBotClient_OnMessage;
            await _chatClient.SetCommandsAsync(Commands.GetBotCommands());
            _chatClient.StartReceiving();
        }

        private async void _telegramBotClient_OnMessage(object sender, ChatClientEventArgs e)
        {
            var username = e.ChatMessage.Username;
            var chatId = e.ChatMessage.ChatId;

            if (!_botOptions.AllowedUsers.Contains(username))
            {
                await _chatClient.SendTextMessageAsync(e.ChatMessage.ChatId, "You are not allowed to interact with the bot.");
                return;
            }

            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    await _chatClient.SendTextMessageAsync(e.ChatMessage.ChatId, "You must have a username to use this bot.");
                    return;
                }

                await HandleRequest(e.ChatMessage, username, chatId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while processing message.");
                await _chatClient.SendTextMessageAsync(e.ChatMessage.ChatId, "An error occured while processing your request.");
            }
        }

        private async Task HandleRequest(ChatMessage message, string username, string chatId)
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
                        await _chatClient.SendTextMessageAsync(message.ChatId, "An error occured while processing your request. Conversation has been reset.");
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
            _chatClient.StopReceiving();

            _logger.LogInformation("Bot stopped.");

            return Task.CompletedTask;
        }
    }
}
