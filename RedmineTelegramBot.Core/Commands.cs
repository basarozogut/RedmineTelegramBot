using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace RedmineTelegramBot.Core
{
    public static class Commands
    {
        public const string Cancel = "cancel";
        public const string ProjectList = "projectlist";
        public const string AddIssue = "addissue";

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
                    Command = ProjectList,
                    Description = "Returns list of projects ids and project names."
                 },
                 new BotCommand()
                 {
                    Command = AddIssue,
                    Description = "Add an issue to a project."
                 },
            };
        }
    }
}
