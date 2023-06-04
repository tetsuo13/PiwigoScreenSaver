namespace PiwigoScreenSaver.Presenters;

public interface ISettingsFormPresenter
{
    string Url { get; set; }
    string Username { get; set; }
    string Password { get; set; }

    void Initialize();
    bool ValidateSettings();
    void SaveSettings();
}
