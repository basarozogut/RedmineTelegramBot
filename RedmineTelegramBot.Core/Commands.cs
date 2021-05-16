using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core
{
    public static class Commands
    {
        public const string Cancel = "cancel";
        public const string Register = "register";
        public const string Unregister = "unregister";
        public const string ProjectList = "projectlist";
        public const string AddIssue = "addissue";
        public const string AssignIssue = "assignissue";

        public static List<BotCommand> GetBotCommands()
        {
            return new List<BotCommand>() {
                 new BotCommand()
                 {
                    Command = Cancel,
                    Description = "Cancel current command."
                 },
                 new BotCommand()
                 {
                    Command = Register,
                    Description = "Register user & secret."
                 },
                 new BotCommand()
                 {
                    Command = Unregister,
                    Description = "Unregister user & secret."
                 },
                 new BotCommand()
                 {
                    Command = ProjectList,
                    Description = "Returns list of projects ids and project names."
                 },
                 new BotCommand()
                 {
                    Command = AddIssue,
                    Description = "Add an issue to a project."
                 },
                 new BotCommand()
                 {
                    Command = AssignIssue,
                    Description = "Assign last added issue to a user."
                 }
            };
        }
    }
}
