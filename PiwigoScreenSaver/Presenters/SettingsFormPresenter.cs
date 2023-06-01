using PiwigoScreenSaver.Domain;
using PiwigoScreenSaver.Models;
using PiwigoScreenSaver.Views;

namespace PiwigoScreenSaver.Presenters
{
    public class SettingsFormPresenter
    {
        private readonly ISettingsService _settingsService;
        private readonly ISettingsFormView _settingsFormView;

        public SettingsFormPresenter(ISettingsFormView settingsFormView, ISettingsService settingsService)
        {
            _settingsFormView = settingsFormView;
            _settingsService = settingsService;
        }

        public void Initialize()
        {
            try
            {
                _settingsFormView.Url = _settingsService.Get(SettingKey.Url);
                _settingsFormView.Username = _settingsService.Get(SettingKey.Username);
                _settingsFormView.Password = _settingsService.Get(SettingKey.Password);
            }
            catch
            {
            }
        }

        public bool ValidateSettings()
        {
            return _settingsService.ValidateSettings(_settingsFormView.Url,
                _settingsFormView.Username, _settingsFormView.Password);
        }

        public void SaveSettings()
        {
            _settingsService.Save(SettingKey.Url, _settingsFormView.Url);
            _settingsService.Save(SettingKey.Username, _settingsFormView.Username);
            _settingsService.Save(SettingKey.Password, _settingsFormView.Password);
        }
    }
}
