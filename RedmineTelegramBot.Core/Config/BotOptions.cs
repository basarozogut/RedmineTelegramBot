using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core.Config
{
    public class BotOptions
    {
        public string Token { get; set; }

        public string RedmineUrl { get; set; }

        public string DataDirectory { get; set; }
    }
}
