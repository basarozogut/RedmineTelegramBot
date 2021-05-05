using RedmineTelegramBot.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core.Data
{
    public interface IConversationStateRepository
    {
        ConversationStateModel GetConversationState(string username);

        void StoreConversationState(ConversationStateModel conversationState);

        void DeleteConversationState(string username);
    }
}
