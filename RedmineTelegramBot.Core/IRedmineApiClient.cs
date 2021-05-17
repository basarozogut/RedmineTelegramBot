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

        Task<IssueAddedModel> AddIssue(AddIssueModel issue);

        Task<RedmineResponseModel> AssignIssue(int id, AssignIssueModel issue);

        Task<IEnumerable<RedmineTrackerModel>> GetTrackers();

        Task<IEnumerable<RedmineUserModel>> GetUsers();

        Task<RedmineGetIssueModel> GetIssue(int id);
    }
}
