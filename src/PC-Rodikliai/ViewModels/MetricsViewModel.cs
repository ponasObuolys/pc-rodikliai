using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using PC_Rodikliai.Services.HardwareMonitor;
using SkiaSharp;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PC_Rodikliai.Components.Graphs;
using PC_Rodikliai.Services;

namespace PC_Rodikliai.ViewModels;

public partial class MetricsViewModel : ObservableObject, IDisposable
{
    private readonly IHardwareMonitorService _hardwareMonitor;
    private readonly string _icon;
    private readonly Brush _progressBrush;
    private double _cpuUsage;
    private double _ramUsage;
    private double _diskUsage;
    private double _networkSpeed;
    private bool _disposed;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private ObservableCollection<MetricValue> _values = new();

    [ObservableProperty]
    private ObservableCollection<ISeries> _series = new();

    public MetricGraphViewModel CpuGraphViewModel { get; }
    public MetricGraphViewModel RamGraphViewModel { get; }
    public MetricGraphViewModel DiskGraphViewModel { get; }
    public MetricGraphViewModel NetworkGraphViewModel { get; }

    public double CpuUsage
    {
        get => _cpuUsage;
        set
        {
            if (Math.Abs(_cpuUsage - value) < 0.01) return;
            _cpuUsage = value;
            CpuGraphViewModel.AddValue(value);
            OnPropertyChanged();
        }
    }

    public double RamUsage
    {
        get => _ramUsage;
        set
        {
            if (Math.Abs(_ramUsage - value) < 0.01) return;
            _ramUsage = value;
            RamGraphViewModel.AddValue(value);
            OnPropertyChanged();
        }
    }

    public double DiskUsage
    {
        get => _diskUsage;
        set
        {
            if (Math.Abs(_diskUsage - value) < 0.01) return;
            _diskUsage = value;
            DiskGraphViewModel.AddValue(value);
            OnPropertyChanged();
        }
    }

    public double NetworkSpeed
    {
        get => _networkSpeed;
        set
        {
            if (Math.Abs(_networkSpeed - value) < 0.01) return;
            _networkSpeed = value;
            NetworkGraphViewModel.AddValue(value);
            OnPropertyChanged();
        }
    }

    public Axis[]? XAxes { get; } =
    {
        new Axis
        {
            IsVisible = false,
            LabelsPaint = new SolidColorPaint(SKColors.Transparent)
        }
    };

    public Axis[]? YAxes { get; } =
    {
        new Axis
        {
            IsVisible = false,
            LabelsPaint = new SolidColorPaint(SKColors.Transparent)
        }
    };

    public MetricsViewModel(
        IHardwareMonitorService hardwareMonitor,
        string title,
        string icon,
        Brush progressBrush)
    {
        _hardwareMonitor = hardwareMonitor ?? throw new ArgumentNullException(nameof(hardwareMonitor));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        _icon = icon ?? throw new ArgumentNullException(nameof(icon));
        _progressBrush = progressBrush ?? throw new ArgumentNullException(nameof(progressBrush));

        // Initialize graph view models
        CpuGraphViewModel = new MetricGraphViewModel("CPU Usage");
        RamGraphViewModel = new MetricGraphViewModel("RAM Usage");
        DiskGraphViewModel = new MetricGraphViewModel("Disk Usage");
        NetworkGraphViewModel = new MetricGraphViewModel("Network Speed");

        SubscribeToEvents();
        StartMonitoring();
    }

    private void SubscribeToEvents()
    {
        if (_disposed || _hardwareMonitor == null) return;

        switch (Title)
        {
            case "CPU":
                _hardwareMonitor.CpuMetricsUpdated += OnCpuMetricsUpdated;
                break;
            case "RAM":
                _hardwareMonitor.RamMetricsUpdated += OnRamMetricsUpdated;
                break;
            case "GPU":
                _hardwareMonitor.GpuMetricsUpdated += OnGpuMetricsUpdated;
                break;
            case "Network":
                _hardwareMonitor.NetworkMetricsUpdated += OnNetworkMetricsUpdated;
                break;
            case "Drive":
                _hardwareMonitor.DriveMetricsUpdated += OnDriveMetricsUpdated;
                break;
        }
    }

    private void UnsubscribeFromEvents()
    {
        if (_hardwareMonitor == null) return;

        switch (Title)
        {
            case "CPU":
                _hardwareMonitor.CpuMetricsUpdated -= OnCpuMetricsUpdated;
                break;
            case "RAM":
                _hardwareMonitor.RamMetricsUpdated -= OnRamMetricsUpdated;
                break;
            case "GPU":
                _hardwareMonitor.GpuMetricsUpdated -= OnGpuMetricsUpdated;
                break;
            case "Network":
                _hardwareMonitor.NetworkMetricsUpdated -= OnNetworkMetricsUpdated;
                break;
            case "Drive":
                _hardwareMonitor.DriveMetricsUpdated -= OnDriveMetricsUpdated;
                break;
        }
    }

    private void OnCpuMetricsUpdated(object? sender, CpuMetricsEventArgs e)
    {
        if (_disposed || e == null) return;

        AddOrUpdateValue("Apkrova", $"{e.TotalLoad:F1}%", e.TotalLoad);
        AddOrUpdateValue("Temperatūra", $"{e.Temperature:F1}°C", e.Temperature);
        AddOrUpdateValue("Dažnis", $"{e.Frequency:F1} GHz", e.Frequency);
        AddOrUpdateValue("Galia", $"{e.Power:F1} W", e.Power);

        UpdateSeries(e.TotalLoad, SKColors.OrangeRed);
    }

    private void OnRamMetricsUpdated(object? sender, RamMetricsEventArgs e)
    {
        if (_disposed || e == null) return;

        AddOrUpdateValue("Apkrova", $"{e.MemoryLoad:F1}%", e.MemoryLoad);
        AddOrUpdateValue("Naudojama", $"{e.UsedMemory:F1} GB", e.UsedMemory);
        AddOrUpdateValue("Viso", $"{e.TotalMemory:F1} GB", e.TotalMemory);

        UpdateSeries(e.MemoryLoad, SKColors.DodgerBlue);
    }

    private void OnGpuMetricsUpdated(object? sender, GpuMetricsEventArgs e)
    {
        if (_disposed || e == null) return;

        AddOrUpdateValue("Apkrova", $"{e.CoreLoad:F1}%", e.CoreLoad);
        AddOrUpdateValue("Temperatūra", $"{e.Temperature:F1}°C", e.Temperature);
        AddOrUpdateValue("VRAM", $"{e.MemoryUsed:F1}/{e.MemoryTotal:F1} GB", (e.MemoryUsed / e.MemoryTotal) * 100);
        AddOrUpdateValue("Ventiliatorius", $"{e.FanSpeed:F0} RPM", (e.FanSpeed / 3000) * 100);

        UpdateSeries(e.CoreLoad, SKColors.MediumPurple);
    }

    private void OnNetworkMetricsUpdated(object? sender, NetworkMetricsEventArgs e)
    {
        if (_disposed || e == null) return;

        AddOrUpdateValue("Atsisiuntimas", FormatSpeed(e.DownloadSpeed), (e.DownloadSpeed / 1000000) * 100);
        AddOrUpdateValue("Įkėlimas", FormatSpeed(e.UploadSpeed), (e.UploadSpeed / 1000000) * 100);
        AddOrUpdateValue("Iš viso atsisiųsta", FormatBytes(e.DownloadTotal), 100);
        AddOrUpdateValue("Iš viso įkelta", FormatBytes(e.UploadTotal), 100);

        UpdateSeries((e.DownloadSpeed + e.UploadSpeed) / (2 * 1000000) * 100, SKColors.LimeGreen);
    }

    private void OnDriveMetricsUpdated(object? sender, DriveMetricsEventArgs e)
    {
        if (_disposed || e == null) return;

        AddOrUpdateValue("Naudojama", FormatBytes(e.UsedSpace), (e.UsedSpace / e.TotalSpace) * 100);
        AddOrUpdateValue("Iš viso", FormatBytes(e.TotalSpace), 100);
        AddOrUpdateValue("Temperatūra", $"{e.Temperature:F1}°C", e.Temperature);
        AddOrUpdateValue("Aktyvumas", $"{e.ActivityPercent:F1}%", e.ActivityPercent);

        UpdateSeries(e.ActivityPercent, SKColors.Gold);
    }

    private void AddOrUpdateValue(string label, string value, double percentage)
    {
        if (Values == null) return;

        var existingValue = Values.FirstOrDefault(v => v.Label == label);
        if (existingValue != null)
        {
            existingValue.Value = value;
            existingValue.Percentage = percentage;
        }
        else
        {
            Values.Add(new MetricValue
            {
                Label = label,
                Value = value,
                Percentage = percentage,
                ProgressBrush = _progressBrush
            });
        }
    }

    private void UpdateSeries(double value, SKColor color)
    {
        if (Series == null) return;

        if (Series.Count == 0)
        {
            Series.Add(new LineSeries<double>
            {
                Values = new ObservableCollection<double> { value },
                Fill = null,
                GeometrySize = 0,
                LineSmoothness = 0.5,
                Stroke = new SolidColorPaint(color) { StrokeThickness = 2 }
            });
        }
        else
        {
            var series = (LineSeries<double>)Series[0];
            var values = (ObservableCollection<double>)series.Values!;
            if (values.Count > 60)
            {
                values.RemoveAt(0);
            }
            values.Add(value);
        }
    }

    private static string FormatBytes(float bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        int order = 0;
        double size = bytes;

        while (size >= 1024 && order < sizes.Length - 1)
        {
            order++;
            size /= 1024;
        }

        return $"{size:F2} {sizes[order]}";
    }

    private static string FormatSpeed(float bytesPerSecond)
    {
        return $"{FormatBytes(bytesPerSecond)}/s";
    }

    private async void StartMonitoring()
    {
        if (_disposed || _hardwareMonitor == null) return;

        try
        {
            while (!_disposed)
            {
                CpuUsage = _hardwareMonitor.GetCpuUsage();
                RamUsage = _hardwareMonitor.GetRamUsage();
                DiskUsage = _hardwareMonitor.GetDiskUsage();
                NetworkSpeed = _hardwareMonitor.GetNetworkSpeed();

                await Task.Delay(1000);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Klaida monitoringo cikle: {ex.Message}");
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        
        _disposed = true;
        UnsubscribeFromEvents();
        
        CpuGraphViewModel?.Dispose();
        RamGraphViewModel?.Dispose();
        DiskGraphViewModel?.Dispose();
        NetworkGraphViewModel?.Dispose();
    }
}

public class MetricValue
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public double Percentage { get; set; }
    public Brush ProgressBrush { get; set; } = Brushes.Blue;
} 