using Microsoft.Extensions.Logging;
using PiwigoScreenSaver.Domain;
using PiwigoScreenSaver.Models;
using System;

namespace PiwigoScreenSaver.Presenters;

public class SettingsFormPresenter : ISettingsFormPresenter
{
    public string Url { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    private readonly ILogger<SettingsFormPresenter> _logger;
    private readonly ISettingsService _settingsService;

    public SettingsFormPresenter(ILogger<SettingsFormPresenter> logger, ISettingsService settingsService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
    }

    public void Initialize()
    {
        try
        {
            Url = _settingsService.Get(SettingKey.Url);
            Username = _settingsService.Get(SettingKey.Username);
            Password = _settingsService.Get(SettingKey.Password);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving settings: {errorMessage}", e.Message);
        }
    }

    public bool ValidateSettings()
    {
        return _settingsService.ValidateSettings(Url, Username, Password);
    }

    public void SaveSettings()
    {
        _settingsService.Save(SettingKey.Url, Url);
        _settingsService.Save(SettingKey.Username, Username);
        _settingsService.Save(SettingKey.Password, Password);
    }
}
