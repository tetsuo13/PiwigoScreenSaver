using PiwigoScreenSaver.Domain;
using PiwigoScreenSaver.Models;
using PiwigoScreenSaver.Views;

namespace PiwigoScreenSaver.Presenters
{
    public class SettingsFormPresenter
    {
        private readonly ISettingsService settingsService;
        private readonly ISettingsFormView settingsFormView;

        public SettingsFormPresenter(ISettingsFormView settingsFormView, ISettingsService settingsService)
        {
            this.settingsFormView = settingsFormView;
            this.settingsService = settingsService;
        }

        public void Initialize()
        {
            try
            {
                settingsFormView.Url = settingsService.Get(SettingKey.Url);
                settingsFormView.Username = settingsService.Get(SettingKey.Username);
                settingsFormView.Password = settingsService.Get(SettingKey.Password);
            }
            catch
            {
            }
        }

        public bool ValidateSettings()
        {
            return settingsService.ValidateSettings(settingsFormView.Url,
                settingsFormView.Username, settingsFormView.Password);
        }

        public void SaveSettings()
        {
            settingsService.Save(SettingKey.Url, settingsFormView.Url);
            settingsService.Save(SettingKey.Username, settingsFormView.Username);
            settingsService.Save(SettingKey.Password, settingsFormView.Password);
        }
    }
}
