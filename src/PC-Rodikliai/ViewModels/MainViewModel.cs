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
    private ObservableCollection<MetricViewModel> _metrics = new();

    public MainViewModel(IHardwareMonitorService hardwareMonitor)
    {
        _hardwareMonitor = hardwareMonitor;
        InitializeMetrics();
    }

    private void InitializeMetrics()
    {
        Metrics.Add(new MetricViewModel { Title = "CPU Apkrova", Value = 0, Unit = "%" });
        Metrics.Add(new MetricViewModel { Title = "RAM Naudojimas", Value = 0, Unit = "GB" });
        Metrics.Add(new MetricViewModel { Title = "Disko Naudojimas", Value = 0, Unit = "GB" });
        Metrics.Add(new MetricViewModel { Title = "Tinklo Greitis", Value = 0, Unit = "Mbps" });
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 