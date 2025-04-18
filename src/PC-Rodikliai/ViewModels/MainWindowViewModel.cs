using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PC_Rodikliai.Components.TrayIcon;
using PC_Rodikliai.Services.HardwareMonitor;
using PC_Rodikliai.Views;
using System.Windows;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;

namespace PC_Rodikliai.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject, IDisposable
    {
        private readonly IHardwareMonitorService _hardwareMonitorService;
        private readonly MainWindow _mainWindow;
        private bool _disposed;

        [ObservableProperty]
        private TrayIconViewModel _trayIconViewModel;

        [ObservableProperty]
        private MetricsViewModel _metricsViewModel;

        public MainWindowViewModel(IHardwareMonitorService hardwareMonitorService, MainWindow mainWindow)
        {
            _hardwareMonitorService = hardwareMonitorService;
            _mainWindow = mainWindow;
            _metricsViewModel = new MetricsViewModel(
                hardwareMonitorService,
                "Pagrindinė",
                "📊",
                new SolidColorBrush(Colors.DodgerBlue));

            _mainWindow.Closing += MainWindow_Closing;

            // Inicializuojame TrayIcon
            TrayIconViewModel = new TrayIconViewModel(hardwareMonitorService, mainWindow);
        }

        [RelayCommand]
        private void ShowSettings()
        {
            // TODO: Implementuoti nustatymų langą
            MessageBox.Show("Nustatymai bus pridėti vėliau", "Nustatymai", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        [RelayCommand]
        private void Minimize()
        {
            _mainWindow.WindowState = WindowState.Minimized;
        }

        [RelayCommand]
        private void Close()
        {
            _mainWindow.Close();
        }

        public void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_disposed) return;

            if (_hardwareMonitorService != null)
            {
                _hardwareMonitorService.StopMonitoring();
            }

            // Vietoj uždarymo, minimizuojame į sistemos dėklą
            e.Cancel = true;
            _mainWindow.Hide();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            
            // Išvalome event handler'ius
            _mainWindow.Closing -= MainWindow_Closing;
            
            // Išvalome TrayIcon
            if (TrayIconViewModel is IDisposable disposableTrayIcon)
            {
                disposableTrayIcon.Dispose();
            }

            _metricsViewModel?.Dispose();

            GC.SuppressFinalize(this);
        }

        ~MainWindowViewModel()
        {
            Dispose();
        }
    }
} 