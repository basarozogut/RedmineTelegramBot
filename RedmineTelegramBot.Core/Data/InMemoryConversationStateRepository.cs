using RedmineTelegramBot.Core.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace RedmineTelegramBot.Core.Data
{
    public class InMemoryConversationStateRepository : IConversationStateRepository
    {
        // store serialized versions of the state to avoid concurrency issues and make it behave like a db.
        private readonly Dictionary<string, string> _store;

        public InMemoryConversationStateRepository()
        {
            _store = new Dictionary<string, string>();
        }

        public void DeleteConversationState(string username)
        {
            if (!_store.ContainsKey(username))
                throw new BotException("Conversation state for this username does not exists!");

            _store.Remove(username);
        }

        public ConversationStateModel GetConversationState(string username)
        {
            if (_store.ContainsKey(username))
            {
                return JsonSerializer.Deserialize<ConversationStateModel>(_store[username]);
            }

            return null;
        }

        public void StoreConversationState(ConversationStateModel conversationState)
        {
            _store[conversationState.Username] = JsonSerializer.Serialize(conversationState);
        }
    }
}
