using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core
{
    public interface IChatClient
    {
        event EventHandler<ChatClientEventArgs> OnMessage;
        void StartReceiving();
        void StopReceiving();
        Task SetCommandsAsync(IEnumerable<BotCommand> commands);
        Task SendTextMessageAsync(string chatId, string text);
    }
}
