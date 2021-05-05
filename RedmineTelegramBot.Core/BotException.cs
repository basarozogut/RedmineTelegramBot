using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core
{

    [Serializable]
    public class BotException : Exception
    {
        public BotException() { }
        public BotException(string message) : base(message) { }
        public BotException(string message, Exception inner) : base(message, inner) { }
        protected BotException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
