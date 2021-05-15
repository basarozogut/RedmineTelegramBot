using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core.Models
{
    public class RedmineUserModel
    {
        public int id { get; set; }

        public string login { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }
    }
}
