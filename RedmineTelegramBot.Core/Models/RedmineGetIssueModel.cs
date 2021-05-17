using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core.Models
{
    public class RedmineGetIssueModel
    {
        public Issue issue { get; set; }

        public class Issue
        {
            public int id { get; set; }

            public string subject { get; set; }
        }
    }
}
