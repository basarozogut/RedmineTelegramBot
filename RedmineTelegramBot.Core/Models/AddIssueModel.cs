using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core.Models
{
    public class AddIssueModel
    {
        public IssueModel issue { get; set; }

        public AddIssueModel()
        {
            issue = new IssueModel();
        }

        public class IssueModel {
            public int project_id { get; set; }

            public string subject { get; set; }

            public string description { get; set; }
        }
    }
}
