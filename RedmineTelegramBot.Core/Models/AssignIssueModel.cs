using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core.Models
{
    public class AssignIssueModel
    {
        public IssueModel issue { get; set; }

        public AssignIssueModel()
        {
            issue = new IssueModel();
        }

        public class IssueModel
        {
            public int assigned_to_id { get; set; }
        }
    }
}
