using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core
{
    public interface IWorkContext
    {
        public string ChatId { get; }

        public string Username { get; }

        public string RedmineSecret { get; }
    }
}
