using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PiwigoScreenSaver.Domain;
using PiwigoScreenSaver.Presenters;
using PiwigoScreenSaver.Views;
using System.Linq;
using System.Windows.Forms;

namespace PiwigoScreenSaver.Configuration;

/// <summary>
/// Extension methods for <see cref="IHostBuilder"/> that adds the application
/// services.
/// </summary>
internal static class ServiceConfiguration
{
    /// <summary>
    /// Adds the application services into the service collection.
    /// </summary>
    /// <param name="builder">The <see cref="IHostBuilder"/> instance.</param>
    /// <returns>The <see cref="IHostBuilder"/>.</returns>
    public static IHostBuilder ConfigureAppServices(this IHostBuilder builder)
    {
        return builder.ConfigureServices((context, services) =>
        {
            services.AddMemoryCache();
            services.AddHttpClient();

            services.AddTransient<IMainFormPresenter, MainFormPresenter>(x =>
            {
                var allScreensBoundaries = Screen.AllScreens.Select(x => x.Bounds);

                // When debugging locally, it may be useful to restrict to a
                // small window to avoid mouse movements triggering exit.
                //allScreensBoundaries = new System.Collections.Generic.List<System.Drawing.Rectangle>
                //{
                //    System.Drawing.Rectangle.FromLTRB(10, 10, 1000, 1000)
                //};

                return new MainFormPresenter(x.GetRequiredService<ILogger<MainFormPresenter>>(),
                    x.GetRequiredService<IGalleryService>(), allScreensBoundaries);
            });

            services.AddTransient<ISettingsFormPresenter, SettingsFormPresenter>();

            services.AddTransient<IMainFormView, MainForm>();
            services.AddTransient<ISettingsFormView, SettingsForm>();

            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ISettingsRepository, RegistryRepository>();
            services.AddTransient<IGalleryService, PiwigoService>();

            services.AddTransient<LaunchManager>();
        });
    }
}
