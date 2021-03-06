using RedmineTelegramBot.Core.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core
{
    public class RedmineApiClient : IRedmineApiClient
    {
        private readonly IRestClientFactory _restClientFactory;

        public RedmineApiClient(IRestClientFactory restClientFactory)
        {
            _restClientFactory = restClientFactory;
        }

        public async Task<IEnumerable<RedmineProjectModel>> GetProjects()
        {
            var client = _restClientFactory.CreateRestClient();

            var projects = new List<RedmineProjectModel>();
            var offset = 0;
            var limit = 100;
            var total = 9999;

            while (offset < total)
            {
                var request = new RestRequest($"/projects.json?offset={offset}&limit={limit}");
                var result = await client.GetAsync<RedmineGetProjectsResultModel>(request);
                projects.AddRange(result.projects);
                total = result.total_count;
                offset += limit;
            }

            return projects;
        }

        public Task<IssueAddedModel> AddIssue(AddIssueModel issue)
        {
            var client = _restClientFactory.CreateRestClient();

            var request = new RestRequest("/issues.json", DataFormat.Json);
            request.AddJsonBody(issue);

            return client.PostAsync<IssueAddedModel>(request);
        }

        public async Task<IEnumerable<RedmineTrackerModel>> GetTrackers()
        {
            var client = _restClientFactory.CreateRestClient();
            var request = new RestRequest($"/trackers.json");
            var result = await client.GetAsync<RedmineGetTrackersResultModel>(request);

            return result.trackers;
        }

        public Task<RedmineResponseModel> AssignIssue(int id, AssignIssueModel issue)
        {
            var client = _restClientFactory.CreateRestClient();

            var request = new RestRequest($"/issues/{id}.json", DataFormat.Json);
            request.AddJsonBody(issue);

            return client.PutAsync<RedmineResponseModel>(request);
        }

        public async Task<IEnumerable<RedmineUserModel>> GetUsers()
        {
            var client = _restClientFactory.CreateRestClient();

            var users = new List<RedmineUserModel>();
            var offset = 0;
            var limit = 100;
            var total = 9999;

            while (offset < total)
            {
                var request = new RestRequest($"/users.json?offset={offset}&limit={limit}");
                var result = await client.GetAsync<RedmineGetUsersModel>(request);
                users.AddRange(result.users);
                total = result.total_count;
                offset += limit;
            }

            return users;
        }

        public Task<RedmineGetIssueModel> GetIssue(int id)
        {
            var client = _restClientFactory.CreateRestClient();
            var request = new RestRequest($"/issues/{id}.json");
            return client.GetAsync<RedmineGetIssueModel>(request);
        }
    }
}
