using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PiwigoScreenSaver.Domain;
using PiwigoScreenSaver.Presenters;
using PiwigoScreenSaver.Views;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;

namespace PiwigoScreenSaver
{
    public static class Program
    {
        private static ILogger logger;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        public static void Main(string[] args)
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            logger = new Logger();

            var modeManager = new ModeManager();
            var mode = modeManager.GetMode(args);

            var settingsService = new SettingsService(new MemoryCache(new MemoryCacheOptions()),
                new RegistryRepository());

            if (mode == ModeManager.Mode.Configuration)
            {
                var settingsForm = new SettingsForm();

                settingsForm.Tag = new SettingsFormPresenter(settingsForm, settingsService);

                Application.Run(settingsForm);
            }
            else if (mode == ModeManager.Mode.Preview)
            {
                // No preview mode implemented. Maybe we could use the demo
                // site at https://piwigo.org/demo/ and show photos from there?
                Application.Exit();
            }
            else
            {
                var galleryService = new PiwigoService(logger, settingsService, new HttpClient());

                var mainForm = new MainForm();
                var allScreensBoundaries = Screen.AllScreens.Select(x => x.Bounds);

                mainForm.Tag = new MainFormPresenter(logger, mainForm,
                    galleryService, allScreensBoundaries);

                Application.Run(mainForm);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            DisplayExceptionMessage(((Exception)e.ExceptionObject).Message);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            DisplayExceptionMessage(e.Exception.Message);
        }

        private static void DisplayExceptionMessage(string error)
        {
            logger.LogError("Unexpected program error: {errorMessage}", error);
            var message = $"Something has gone seriously wrong:\n{error}";
            MessageBox.Show(message, "Unexpected error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }
    }
}
