using Microsoft.Extensions.DependencyInjection;
using PC_Rodikliai.Services.HardwareMonitor;
using PC_Rodikliai.ViewModels;
using PC_Rodikliai.Views;
using System.Windows;
using System;
using System.Windows.Threading;

namespace PC_Rodikliai;

public partial class App : Application
{
    public IServiceProvider Services { get; }

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();

        this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IHardwareMonitorService, HardwareMonitorService>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
    }

    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show($"Įvyko klaida: {e.Exception.Message}\n\nStack trace:\n{e.Exception.StackTrace}", 
                      "Klaida", 
                      MessageBoxButton.OK, 
                      MessageBoxImage.Error);
        e.Handled = true;
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            MessageBox.Show($"Kritinė klaida: {ex.Message}\n\nStack trace:\n{ex.StackTrace}", 
                          "Kritinė klaida", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Error);
        }
    }
}