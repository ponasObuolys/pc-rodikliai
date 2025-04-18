using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PC_Rodikliai.Services.HardwareMonitor;
using PC_Rodikliai.Services.AlertService;
using PC_Rodikliai.Services.HotkeyService;
using System.Windows;
using PC_Rodikliai.Views;

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
                services.AddSingleton<IAlertService, AlertService>();
                services.AddSingleton<IHotkeyService, HotkeyService>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync();
        }

        base.OnExit(e);
    }
} 