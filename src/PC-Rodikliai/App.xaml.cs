using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PC_Rodikliai.Services;
using System.Windows;
using PC_Rodikliai.Views;
using PC_Rodikliai.Services.HardwareMonitor;

namespace PC_Rodikliai;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<IHardwareMonitorService, HardwareMonitorService>();
                services.AddSingleton<AlertService>();
                services.AddSingleton<HotkeyService>();
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host.Dispose();
        base.OnExit(e);
    }
} 