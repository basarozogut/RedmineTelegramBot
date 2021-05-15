using RedmineTelegramBot.Core.Data;
using RedmineTelegramBot.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RedmineTelegramBot.Core
{
    public class ConversationHandler : IConversationHandler
    {
        private readonly IWorkContext _workContext;
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IRedmineApiClient _redmineApiClient;
        private readonly IUserSettingsRepository _userSettingsRepository;
        private readonly IConversationStateRepository _conversationStateRepository;

        private ConversationStateModel _conversationState;

        public ConversationHandler(
            IWorkContext workContext,
            ITelegramBotClient telegramBotClient,
            IRedmineApiClient redmineApiClient,
            IUserSettingsRepository userSettingsRepository,
            IConversationStateRepository conversationStateRepository)
        {
            _workContext = workContext;
            _telegramBotClient = telegramBotClient;
            _redmineApiClient = redmineApiClient;
            _userSettingsRepository = userSettingsRepository;
            _conversationStateRepository = conversationStateRepository;
        }

        public async Task Handle(Message message)
        {
            _conversationState = _conversationStateRepository.GetConversationState(_workContext.Username);

            if (message.Text == "/start")
            {
                await ReplyMessage(message, "Hello. Please use / key to see command list. You must first register your api secret with /register command to use other commands.");
                return;
            }

            if (message.Text == $"/{Commands.Cancel}")
            {
                await ChangeState(message, State.Command);
                await ReplyMessage(message, "Cancelled current command.");
                return;
            }

            if (_conversationState.State == State.Command)
            {
                if (message.Text == $"/{Commands.Register}")
                {
                    await ChangeState(message, State.RegisterSecret);
                    return;
                }

                if (message.Text == $"/{Commands.Unregister}")
                {
                    await UnregisterUser(message);
                    return;
                }

                if (!CheckRegistration())
                {
                    await ReplyMessage(message, "You must register your redmine secret before using any functionality.");
                    return;
                }

                if (message.Text == $"/{Commands.ProjectList}")
                {
                    await ChangeState(message, State.SearchProjects);
                    return;
                }

                if (message.Text == $"/{Commands.AddIssue}")
                {
                    _conversationState.CreateIssueModel = new AddIssueModel();
                    await ChangeState(message, State.AddIssueSetIssueProjectId);
                    return;
                }

                if (message.Text == $"/{Commands.AssignIssue}")
                {
                    if (_conversationState.LastIssueId == 0)
                    {
                        await ReplyMessage(message, "No recently created issue exists.");
                        return;
                    }

                    await ChangeState(message, State.AssignIssueSetUserId);
                    return;
                }
            }

            if (_conversationState.State == State.RegisterSecret)
            {
                var settings = _userSettingsRepository.GetSettings(_conversationState.Username);
                if (settings == null)
                {
                    settings = new UserSettings()
                    {
                        Username = _conversationState.Username
                    };
                }
                settings.RedmineSecret = message.Text;
                _userSettingsRepository.StoreSettings(settings);
                await ReplyMessage(message, "User registered.");
                await ChangeState(message, State.Command);
                return;
            }

            if (_conversationState.State == State.SearchProjects)
            {
                await ReplyWithProjectList(message, message.Text);
                return;
            }

            if (_conversationState.State == State.AddIssueSetIssueProjectId)
            {
                _conversationState.CreateIssueModel.issue.project_id = int.Parse(message.Text);
                await ChangeState(message, State.AddIssueSetIssueSubject);
                return;
            }

            if (_conversationState.State == State.AddIssueSetIssueSubject)
            {
                _conversationState.CreateIssueModel.issue.subject = message.Text;
                await ChangeState(message, State.AddIssueSetIssueDescription);
                return;
            }

            if (_conversationState.State == State.AddIssueSetIssueDescription)
            {
                _conversationState.CreateIssueModel.issue.description = message.Text;
                await ChangeState(message, State.AddIssueSetTrackerId);
                return;
            }

            if (_conversationState.State == State.AddIssueSetTrackerId)
            {
                _conversationState.CreateIssueModel.issue.tracker_id = int.Parse(message.Text);
                await AddIssue(message);
                await ChangeState(message, State.Command);
                return;
            }

            if (_conversationState.State == State.AssignIssueSetUserId)
            {
                await AssignIssue(message);
                await ChangeState(message, State.Command);
                return;
            }

            await ReplyMessage(message, "Unknown text or command:\n" + message.Text);
        }

        private Task UnregisterUser(Message message)
        {
            _userSettingsRepository.DeleteSettings(_workContext.Username);

            return ReplyMessage(message, "Unregistered.");
        }

        private bool CheckRegistration()
        {
            var userSettings = _userSettingsRepository.GetSettings(_workContext.Username);
            if (userSettings == null || string.IsNullOrEmpty(userSettings.RedmineSecret))
            {
                return false;
            }

            return true;
        }

        private async Task ChangeState(Message message, State state)
        {
            _conversationState.State = state;

            if (state == State.RegisterSecret)
            {
                await ReplyMessage(message, "Redmine Secret:\n");
            }
            else if (state == State.AddIssueSetIssueProjectId)
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
            else if (state == State.AddIssueSetTrackerId)
            {
                await ReplyWithTrackerList(message);
                await ReplyMessage(message, "Tracker Id:\n");
            }
            else if (state == State.SearchProjects)
            {
                await ReplyMessage(message, "Search pattern (type * for all):\n");
            }
            else if (state == State.AssignIssueSetUserId)
            {
                await ReplyWithUserList(message);
                await ReplyMessage(message, "User Id:\n");
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

        private async Task ReplyWithTrackerList(Message message)
        {
            var trackers = await _redmineApiClient.GetTrackers();
            var trackersStr = string.Join("\n", trackers.OrderBy(r => r.Id).Select(r => $"{r.Id}) {r.Name}"));

            await ReplyMessage(message, $"Trackers:\n{trackersStr}");
        }

        private async Task ReplyWithUserList(Message message)
        {
            var users = await _redmineApiClient.GetUsers();
            var trackersStr = string.Join("\n", users.OrderBy(r => r.Id).Select(r => $"{r.Id}) {r.Login} ({r.Firstname} {r.Lastname})"));

            await ReplyMessage(message, $"Users:\n{trackersStr}");
        }

        private async Task AddIssue(Message message)
        {
            var response = await _redmineApiClient.AddIssue(_conversationState.CreateIssueModel);
            if (response.errors != null && response.errors.Count > 0)
            {
                await ReplyMessage(message, string.Join("\n", response.errors));
            }
            else
            {
                _conversationState.LastIssueId = response.issue.id;
                await ReplyMessage(message, "Issue created.");
            }
        }

        private async Task AssignIssue(Message message)
        {
            var model = new AssignIssueModel();
            model.issue.assigned_to_id = int.Parse(message.Text);
            var response = await _redmineApiClient.AssignIssue(_conversationState.LastIssueId, model);
            if (response != null && response.Errors != null && response.Errors.Count > 0)
            {
                await ReplyMessage(message, string.Join("\n", response.Errors));
            }
            else
            {
                await ReplyMessage(message, "Issue assigned to user.");
            }
        }

        public void SaveState()
        {
            _conversationStateRepository.StoreConversationState(_conversationState);
        }
    }
}
