using RedmineTelegramBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using BotCommand = Telegram.Bot.Types.BotCommand;

namespace RedmineTelegramBot.Telegram
{
    public class TelegramChatClient : IChatClient
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private bool _subscribedToEvents = false;

        public TelegramChatClient(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public event EventHandler<ChatClientEventArgs> OnMessage;

        public Task SendTextMessageAsync(string chatId, string message)
        {
            return _telegramBotClient.SendTextMessageAsync(new ChatId(long.Parse(chatId)), message);
        }

        public Task SetCommandsAsync(IEnumerable<Core.BotCommand> commands)
        {
            var telegramCommands = commands.Select(r => new BotCommand
            {
                Command = r.Command,
                Description = r.Description
            });

            return _telegramBotClient.SetMyCommandsAsync(telegramCommands);
        }

        public void StartReceiving()
        {
            _telegramBotClient.StartReceiving();
            if (_subscribedToEvents)
                return;

            _telegramBotClient.OnMessage += _telegramBotClient_OnMessage;
            _subscribedToEvents = true;
        }

        public void StopReceiving()
        {
            _telegramBotClient.StopReceiving();
        }

        private void _telegramBotClient_OnMessage(object sender, MessageEventArgs e)
        {
            var abstractEvent = new ChatClientEventArgs(new ChatMessage()
            {
                ChatId = e.Message.Chat.Id.ToString(),
                Username = e.Message.Chat.Username,
                Text = e.Message.Text
            });
            OnMessage?.Invoke(this, abstractEvent);
        }
    }
}
