using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core.Data
{
    public interface IUserSettingsRepository
    {
        void StoreSettings(string username, UserSettings userSettings);

        UserSettings GetSettings(string username);
    }
}
