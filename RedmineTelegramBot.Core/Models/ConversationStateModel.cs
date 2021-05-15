using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core.Models
{
    public class ConversationStateModel
    {
        public long ChatId { get; set; }

        public string Username { get; set; }

        public State State { get; set; } = State.Command;

        public AddIssueModel CreateIssueModel { get; set; }

        public int LastIssueId { get; set; }
    }

    public enum State
    {
        Command,
        // register
        RegisterSecret,
        // search projects
        SearchProjects,
        // add issue
        AddIssueSetIssueProjectId,
        AddIssueSetIssueSubject,
        AddIssueSetIssueDescription,
        AddIssueSetTrackerId,
        // assign issue
        AssignIssueSetUserId
    }
}
