using RedmineTelegramBot.Core.Config;
using RedmineTelegramBot.Core.Data;
using RedmineTelegramBot.Core.Models;
using Telegram.Bot;

namespace RedmineTelegramBot.Core
{
    public class ConversationHandlerFactory : IConversationHandlerFactory
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly BotOptions _botOptions;
        private readonly IUserSettingsRepository _userSettingsRepository;

        public ConversationHandlerFactory(ITelegramBotClient telegramBotClient, BotOptions botOptions, IUserSettingsRepository userSettingsRepository)
        {
            _telegramBotClient = telegramBotClient;
            _botOptions = botOptions;
            _userSettingsRepository = userSettingsRepository;
        }

        public IConversationHandler CreateConversationHandler(ConversationStateModel state)
        {
            var userSettings = _userSettingsRepository.GetSettings(state.Username);
            if (userSettings == null)
            {
                throw new BotException("User settings not found!");
            }

            var restClientFactory = new RestClientFactory(_botOptions, userSettings);

            return new ConversationHandler(_telegramBotClient, new RedmineApiClient(restClientFactory), state);
        }
    }
}
