using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core
{
    public interface IWorkContextSetter
    {
        public long ChatId { set; }

        public string Username { set; }

        public string RedmineSecret { set; }
    }
}
