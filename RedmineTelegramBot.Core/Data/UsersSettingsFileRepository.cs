using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core.Data
{
    public class UsersSettingsFileRepository : IUserSettingsRepository
    {
        private readonly string _dataDirectoryPath;

        public UsersSettingsFileRepository(string dataDirectoryPath)
        {
            _dataDirectoryPath = dataDirectoryPath;
        }

        public UserSettings GetSettings(string username)
        {
            var path = MakePath(username);
            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                return JsonSerializer.Deserialize<UserSettings>(text);
            }

            return null;
        }

        public void StoreSettings(string username, UserSettings userSettings)
        {
            var path = MakePath(username);
            var json = JsonSerializer.Serialize(userSettings);
            File.WriteAllText(path, json);
        }

        private string MakePath(string username)
        {
            if (!Directory.Exists(_dataDirectoryPath))
            {
                Directory.CreateDirectory(_dataDirectoryPath);
            }

            return Path.Combine(_dataDirectoryPath, $"user_settings_{username}.json");
        }
    }
}
