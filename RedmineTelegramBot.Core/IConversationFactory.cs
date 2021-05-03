using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace RedmineTelegramBot.Core
{
    public interface IConversationFactory
    {
        IConversation CreateConversation();
    }
}
