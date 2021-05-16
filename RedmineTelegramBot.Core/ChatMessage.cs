using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core
{
    public class ChatMessage
    {
        public string Username {get;set;}

        public string ChatId { get; set; }

        public string Text { get; set; }
    }
}
