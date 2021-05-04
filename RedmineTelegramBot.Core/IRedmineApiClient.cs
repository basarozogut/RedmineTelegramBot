using RedmineTelegramBot.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core
{
    public interface IRedmineApiClient
    {
        Task<IEnumerable<RedmineProjectModel>> GetProjects();

        Task<RedmineResponseModel> AddIssue(AddIssueModel issue);
    }
}
