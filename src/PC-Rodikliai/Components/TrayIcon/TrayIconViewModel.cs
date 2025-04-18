using System;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PC_Rodikliai.Services.HardwareMonitor;

namespace PC_Rodikliai.Components.TrayIcon
{
    public partial class TrayIconViewModel : ObservableObject, IDisposable
    {
        private readonly IHardwareMonitorService _hardwareMonitor;
        private readonly Window _mainWindow;
        private bool _disposed;

        [ObservableProperty]
        private double _cpuUsage;

        [ObservableProperty]
        private double _ramUsage;

        [ObservableProperty]
        private double _diskUsage;

        [ObservableProperty]
        private double _networkSpeed;

        public ICommand ShowWindowCommand { get; }
        public ICommand ShowSettingsCommand { get; }
        public ICommand ShowAboutCommand { get; }
        public ICommand ExitApplicationCommand { get; }

        public TrayIconViewModel(IHardwareMonitorService hardwareMonitor, Window mainWindow)
        {
            _hardwareMonitor = hardwareMonitor;
            _mainWindow = mainWindow;

            ShowWindowCommand = new RelayCommand(ShowWindow);
            ShowSettingsCommand = new RelayCommand(ShowSettings);
            ShowAboutCommand = new RelayCommand(ShowAbout);
            ExitApplicationCommand = new RelayCommand(ExitApplication);

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            if (_disposed) return;

            _hardwareMonitor.CpuMetricsUpdated += OnCpuMetricsUpdated;
            _hardwareMonitor.RamMetricsUpdated += OnRamMetricsUpdated;
            _hardwareMonitor.DriveMetricsUpdated += OnDriveMetricsUpdated;
            _hardwareMonitor.NetworkMetricsUpdated += OnNetworkMetricsUpdated;
        }

        private void UnsubscribeFromEvents()
        {
            if (_hardwareMonitor == null) return;

            _hardwareMonitor.CpuMetricsUpdated -= OnCpuMetricsUpdated;
            _hardwareMonitor.RamMetricsUpdated -= OnRamMetricsUpdated;
            _hardwareMonitor.DriveMetricsUpdated -= OnDriveMetricsUpdated;
            _hardwareMonitor.NetworkMetricsUpdated -= OnNetworkMetricsUpdated;
        }

        private void OnCpuMetricsUpdated(object? sender, CpuMetricsEventArgs e)
        {
            if (_disposed) return;
            CpuUsage = e.TotalLoad;
        }

        private void OnRamMetricsUpdated(object? sender, RamMetricsEventArgs e)
        {
            if (_disposed) return;
            RamUsage = e.MemoryLoad;
        }

        private void OnDriveMetricsUpdated(object? sender, DriveMetricsEventArgs e)
        {
            if (_disposed) return;
            DiskUsage = e.ActivityPercent;
        }

        private void OnNetworkMetricsUpdated(object? sender, NetworkMetricsEventArgs e)
        {
            if (_disposed) return;
            NetworkSpeed = (e.DownloadSpeed + e.UploadSpeed) / (1024 * 1024); // MB/s
        }

        private void ShowWindow()
        {
            if (_disposed) return;
            _mainWindow.Show();
            _mainWindow.WindowState = WindowState.Normal;
            _mainWindow.Activate();
        }

        private void ShowSettings()
        {
            if (_disposed) return;
            MessageBox.Show("Nustatymų langas bus pridėtas vėliau.", "PC Rodikliai");
        }

        private void ShowAbout()
        {
            if (_disposed) return;
            MessageBox.Show(
                "PC Rodikliai v0.2.0\n\n" +
                "Sistemos monitoringo įrankis\n" +
                "© 2024 Visos teisės saugomos",
                "Apie PC Rodikliai",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void ExitApplication()
        {
            if (_disposed) return;
            if (MessageBox.Show(
                "Ar tikrai norite uždaryti programą?",
                "PC Rodikliai",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            
            _disposed = true;
            UnsubscribeFromEvents();
            GC.SuppressFinalize(this);
        }

        ~TrayIconViewModel()
        {
            Dispose();
        }
    }
} 