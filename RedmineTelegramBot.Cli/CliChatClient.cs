using RedmineTelegramBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Cli
{
    public class CliChatClient : IChatClient
    {
        private List<BotCommand> _commands;

        public event EventHandler<ChatClientEventArgs> OnMessage;

        public Task SendTextMessageAsync(string chatId, string text)
        {
            Console.WriteLine(text);

            return Task.CompletedTask;
        }

        public Task SetCommandsAsync(IEnumerable<BotCommand> commands)
        {
            _commands = commands.ToList();
            _commands.Add(new BotCommand
            {
                Command = "exit",
                Description = "Exit the CLI."
            });
            _commands.Add(new BotCommand
            {
                Command = "commands",
                Description = "Display the commands."
            });

            return Task.CompletedTask;
        }

        public void StartReceiving()
        {
            DisplayCommands();

            while (true)
            {
                var text = Console.ReadLine();
                if (text == "/exit")
                    break;

                if (text == "/commands")
                {
                    DisplayCommands();
                    continue;
                }

                if (OnMessage != null)
                {
                    OnMessage(this, new ChatClientEventArgs(new ChatMessage()
                    {
                        ChatId = "local_chat",
                        Text = text,
                        Username = "local_user"
                    }));
                }
            }
        }

        public void StopReceiving()
        {

        }

        private void DisplayCommands()
        {
            if (_commands != null && _commands.Any())
            {
                Console.WriteLine("Commands:");
                foreach (var command in _commands)
                {
                    Console.WriteLine($"{command.Command}: {command.Description}");
                }
            }
        }
    }
}
