using PiwigoScreenSaver.Models;

namespace PiwigoScreenSaver.Domain
{
    public interface ISettingsService
    {
        void Save(SettingKey key, string value);
        string Get(SettingKey key);
        bool ValidateSettings(string url, string username, string password);
    }
}
