using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core.Data
{
    public interface IConversationStateRepository
    {
        ConversationState GetConversationState(string username);

        void StoreConversationState(ConversationState conversationState);

        void DeleteConversationState(string username);
    }
}
