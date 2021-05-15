using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core.Models
{
    public class RedmineGetUsersModel
    {
        public List<RedmineUserModel> Users { get; set; }

        public int TotalCount { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }
    }
}
