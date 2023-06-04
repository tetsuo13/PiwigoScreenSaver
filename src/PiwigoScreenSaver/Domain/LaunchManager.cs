using Microsoft.Extensions.DependencyInjection;
using PiwigoScreenSaver.Views;
using System;
using System.Windows.Forms;

namespace PiwigoScreenSaver.Domain;

internal class LaunchManager
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public LaunchManager(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
    }

    public void LaunchForm(string[] args)
    {
        var modeManager = new ModeManager();
        var mode = modeManager.GetMode(args);
        mode = ModeManager.Mode.FullScreen;

        if (mode == ModeManager.Mode.Configuration)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            Application.Run((Form)scope.ServiceProvider.GetRequiredService<ISettingsFormView>());
        }
        else if (mode == ModeManager.Mode.Preview)
        {
            // No preview mode implemented. Maybe we could use the demo
            // site at https://piwigo.org/demo/ and show photos from there?
            Application.Exit();
        }
        else
        {
            using var scope = _serviceScopeFactory.CreateScope();
            Application.Run((Form)scope.ServiceProvider.GetRequiredService<IMainFormView>());
        }
    }
}
