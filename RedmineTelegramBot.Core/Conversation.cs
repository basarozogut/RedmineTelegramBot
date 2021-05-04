using RedmineTelegramBot.Core.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RedmineTelegramBot.Core
{
    public class Conversation : IConversation
    {
        private enum State
        {
            Command,
            // search projects
            SearchProjects,
            // add issue
            AddIssueSetIssueProjectId,
            AddIssueSetIssueSubject,
            AddIssueSetIssueDescription,
        }

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IRedmineApiClient _redmineApiClient;

        private State _state = State.Command;

        // issue
        private AddIssueModel _createIssueModel;

        public Conversation(
            ITelegramBotClient telegramBotClient,
            IRedmineApiClient redmineApiClient)
        {
            _telegramBotClient = telegramBotClient;
            _redmineApiClient = redmineApiClient;
        }

        public async Task Process(Message message)
        {
            if (message.Text == $"/{Commands.Cancel}")
            {
                await ChangeState(message, State.Command);
                await ReplyMessage(message, "Cancelled current command.");
                return;
            }

            if (_state == State.Command)
            {
                if (message.Text == $"/{Commands.ProjectList}")
                {
                    await ChangeState(message, State.SearchProjects);
                    return;
                }

                if (message.Text == $"/{Commands.AddIssue}")
                {
                    _createIssueModel = new AddIssueModel();
                    await ChangeState(message, State.AddIssueSetIssueProjectId);
                    return;
                }
            }

            if (_state == State.SearchProjects)
            {
                await ReplyWithProjectList(message, message.Text);
                return;
            }

            if (_state == State.AddIssueSetIssueProjectId)
            {
                _createIssueModel.issue.project_id = int.Parse(message.Text);
                await ChangeState(message, State.AddIssueSetIssueSubject);
                return;
            }

            if (_state == State.AddIssueSetIssueSubject)
            {
                _createIssueModel.issue.subject = message.Text;
                await ChangeState(message, State.AddIssueSetIssueDescription);
                return;
            }

            if (_state == State.AddIssueSetIssueDescription)
            {
                _createIssueModel.issue.description = message.Text;
                await AddIssue(message);
                await ChangeState(message, State.Command);
                return;
            }

            await ReplyMessage(message, "Unknown text or command:\n" + message.Text);
        }

        private async Task ChangeState(Message message, State state)
        {
            _state = state;

            if (state == State.AddIssueSetIssueProjectId)
            {
                await ReplyMessage(message, "Project Id:\n");
            }
            else if (state == State.AddIssueSetIssueSubject)
            {
                await ReplyMessage(message, "Issue Title:\n");
            }
            else if (state == State.AddIssueSetIssueDescription)
            {
                await ReplyMessage(message, "Issue Description:\n");
            }
            else if (state == State.SearchProjects)
            {
                await ReplyMessage(message, "Search pattern (type * for all):\n");
            }
        }

        private Task ReplyMessage(Message message, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Task.CompletedTask;
            }

            return _telegramBotClient.SendTextMessageAsync(
             chatId: message.Chat,
             text: text
           );
        }

        private async Task ReplyWithProjectList(Message message, string searchPattern)
        {
            searchPattern = searchPattern.Trim();

            var projects = await _redmineApiClient.GetProjects();

            if (searchPattern != "*")
            {
                projects = projects.Where(r => r.Name.Contains(searchPattern, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (projects.Any())
            {
                var projectListStr = string.Join("\n", projects.OrderBy(r => r.Name).Select(r => $"{r.Id}) {r.Name}"));

                await ReplyMessage(message, projectListStr);
            }
            else
            {
                await ReplyMessage(message, "No project found matching the filter.");
            }
            
            await ChangeState(message, State.Command);
        }

        private async Task AddIssue(Message message)
        {
            var response = await _redmineApiClient.AddIssue(_createIssueModel);
            if (response.Errors != null && response.Errors.Count > 0)
            {
                await ReplyMessage(message, string.Join("\n", response.Errors));
            }
            else
            {
                await ReplyMessage(message, "Issue created.");
            }
        }
    }
}
