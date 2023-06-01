namespace PiwigoScreenSaver.Domain;

public interface ISettingsRepository
{
    string GetValue(string name);
    void SetValue(string name, string value);
}
