using PiwigoScreenSaver.Models;

namespace PiwigoScreenSaver.Events
{
    public class SettingsMessage : IApplicationEvent
    {
        public SettingsModel Settings { get; private set; }

        public SettingsMessage(SettingsModel settings)
        {
            Settings = settings;
        }
    }
}
