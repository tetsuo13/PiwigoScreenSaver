using PiwigoScreenSaver.Views;

namespace PiwigoScreenSaver.Tests.Mocks
{
    public class MockSettingsFormView : ISettingsFormView
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
