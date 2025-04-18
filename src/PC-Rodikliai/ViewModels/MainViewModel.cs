using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using PC_Rodikliai.Services.HardwareMonitor;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PC_Rodikliai.Models;

namespace PC_Rodikliai.ViewModels;

public partial class MainViewModel : ObservableObject, INotifyPropertyChanged
{
    private readonly IHardwareMonitorService _hardwareMonitor;

    [ObservableProperty]
    private ObservableCollection<MetricsViewModel> _metrics = new();

    public MainViewModel(IHardwareMonitorService hardwareMonitor)
    {
        _hardwareMonitor = hardwareMonitor;
        InitializeMetrics();
        _hardwareMonitor.StartMonitoring();
    }

    private void InitializeMetrics()
    {
        Metrics.Add(new MetricsViewModel(_hardwareMonitor, "CPU", "🔲", Brushes.OrangeRed));
        Metrics.Add(new MetricsViewModel(_hardwareMonitor, "RAM", "📊", Brushes.DodgerBlue));
        Metrics.Add(new MetricsViewModel(_hardwareMonitor, "GPU", "🎮", Brushes.LimeGreen));
        Metrics.Add(new MetricsViewModel(_hardwareMonitor, "Tinklas", "🌐", Brushes.MediumPurple));
        Metrics.Add(new MetricsViewModel(_hardwareMonitor, "Diskai", "💾", Brushes.Gold));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 