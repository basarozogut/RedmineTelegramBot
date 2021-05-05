using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core
{
    public class WorkContext : IWorkContext, IWorkContextSetter
    {
        public long ChatId { get; set; }

        public string Username { get; set; }

        public string RedmineSecret { get; set; }
    }
}
