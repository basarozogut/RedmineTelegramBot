using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core.Models
{
    public class RedmineUserModel
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }
    }
}
