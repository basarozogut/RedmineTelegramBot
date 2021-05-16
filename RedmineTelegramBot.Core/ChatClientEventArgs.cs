using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core
{
    public class ChatClientEventArgs
    {
        public ChatMessage ChatMessage { get; }

        public ChatClientEventArgs(ChatMessage chatMessage)
        {
            ChatMessage = chatMessage;
        }
    }
}
