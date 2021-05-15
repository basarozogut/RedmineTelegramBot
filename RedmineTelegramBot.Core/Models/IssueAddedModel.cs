using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core.Models
{
    public class IssueAddedModel
    {
        public IssueModel issue { get; set; }

        public List<string> errors { get; set; }

        public class IssueModel
        {
            public int id { get; set; }
        }
    }
}
