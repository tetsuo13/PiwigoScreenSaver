using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PiwigoScreenSaver.Configuration;
using PiwigoScreenSaver.Domain;
using System;
using System.Threading;
using System.Windows.Forms;

namespace PiwigoScreenSaver;

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
        var host = Host.CreateDefaultBuilder()
            .ConfigureAppServices()
            .Build();

        ApplicationConfiguration.Initialize();

        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += Application_ThreadException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        logger = new Logger();

        var launcher = host.Services.GetRequiredService<LaunchManager>();
        launcher.LaunchForm(args);
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
