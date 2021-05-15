using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core.Models
{
    public class RedmineGetUsersModel
    {
        public List<RedmineUserModel> users { get; set; }

        public int total_count { get; set; }

        public int offset { get; set; }

        public int limit { get; set; }
    }
}
