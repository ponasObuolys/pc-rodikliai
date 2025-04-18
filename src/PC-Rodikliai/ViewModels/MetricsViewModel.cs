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

namespace PC_Rodikliai.ViewModels;

public partial class MetricsViewModel : ObservableObject
{
    private readonly IHardwareMonitorService _hardwareMonitor;
    private readonly string _icon;
    private readonly Brush _progressBrush;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private ObservableCollection<MetricValue> _values = new();

    [ObservableProperty]
    private ObservableCollection<ISeries> _series = new();

    public Axis[] XAxes { get; } =
    {
        new Axis
        {
            IsVisible = false,
            LabelsPaint = new SolidColorPaint(SKColors.Transparent)
        }
    };

    public Axis[] YAxes { get; } =
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
        _hardwareMonitor = hardwareMonitor;
        Title = title;
        _icon = icon;
        _progressBrush = progressBrush;

        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        switch (_title)
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
            case "Tinklas":
                _hardwareMonitor.NetworkMetricsUpdated += OnNetworkMetricsUpdated;
                break;
            case "Diskai":
                _hardwareMonitor.DriveMetricsUpdated += OnDriveMetricsUpdated;
                break;
        }
    }

    private void OnCpuMetricsUpdated(object? sender, CpuMetricsEventArgs e)
    {
        AddOrUpdateValue("Apkrova", $"{e.TotalLoad:F1}%", e.TotalLoad);
        AddOrUpdateValue("Temperatūra", $"{e.Temperature:F1}°C", e.Temperature);
        AddOrUpdateValue("Dažnis", $"{e.Frequency:F1} MHz", e.Frequency / 50); // Normalizuojame iki 100%
        AddOrUpdateValue("Galia", $"{e.Power:F1} W", e.Power / 2); // Normalizuojame iki 100%

        // Atnaujinti grafiką
        if (Series.Count == 0)
        {
            Series.Add(new LineSeries<double>
            {
                Values = new ObservableCollection<double> { e.TotalLoad },
                Fill = null,
                GeometrySize = 0,
                LineSmoothness = 0.5,
                Stroke = new SolidColorPaint(SKColors.OrangeRed) { StrokeThickness = 2 }
            });
        }
        else
        {
            var series = (LineSeries<double>)Series[0];
            var values = (ObservableCollection<double>)series.Values!;
            if (values.Count > 60) // Laikome 1 minutės duomenis
            {
                values.RemoveAt(0);
            }
            values.Add(e.TotalLoad);
        }
    }

    private void OnRamMetricsUpdated(object? sender, RamMetricsEventArgs e)
    {
        var usedGB = e.UsedMemory / 1024;
        var totalGB = e.TotalMemory / 1024;
        
        AddOrUpdateValue("Naudojama", $"{usedGB:F1} GB", e.MemoryLoad);
        AddOrUpdateValue("Iš viso", $"{totalGB:F1} GB", 100);
        AddOrUpdateValue("Laisva", $"{(totalGB - usedGB):F1} GB", 100 - e.MemoryLoad);

        // Atnaujinti grafiką
        if (Series.Count == 0)
        {
            Series.Add(new LineSeries<double>
            {
                Values = new ObservableCollection<double> { e.MemoryLoad },
                Fill = null,
                GeometrySize = 0,
                LineSmoothness = 0.5,
                Stroke = new SolidColorPaint(SKColors.DodgerBlue) { StrokeThickness = 2 }
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
            values.Add(e.MemoryLoad);
        }
    }

    private void OnGpuMetricsUpdated(object? sender, GpuMetricsEventArgs e)
    {
        AddOrUpdateValue("Apkrova", $"{e.CoreLoad:F1}%", e.CoreLoad);
        AddOrUpdateValue("Temperatūra", $"{e.Temperature:F1}°C", e.Temperature);
        AddOrUpdateValue("VRAM", $"{e.MemoryUsed:F1}/{e.MemoryTotal:F1} GB", (e.MemoryUsed / e.MemoryTotal) * 100);
        AddOrUpdateValue("Ventiliatorius", $"{e.FanSpeed:F0} RPM", (e.FanSpeed / 3000) * 100); // Normalizuojame iki 100%

        // TODO: Atnaujinti grafikus
    }

    private void OnNetworkMetricsUpdated(object? sender, NetworkMetricsEventArgs e)
    {
        AddOrUpdateValue("Atsisiuntimas", $"{FormatSpeed(e.DownloadSpeed)}", (e.DownloadSpeed / 1000000) * 100); // Normalizuojame iki 100 MB/s
        AddOrUpdateValue("Įkėlimas", $"{FormatSpeed(e.UploadSpeed)}", (e.UploadSpeed / 1000000) * 100);
        AddOrUpdateValue("Iš viso atsisiųsta", $"{FormatBytes(e.DownloadTotal)}", 100);
        AddOrUpdateValue("Iš viso įkelta", $"{FormatBytes(e.UploadTotal)}", 100);

        // Atnaujinti grafiką
        if (Series.Count == 0)
        {
            Series.Add(new LineSeries<double>
            {
                Name = "Atsisiuntimas",
                Values = new ObservableCollection<double> { e.DownloadSpeed / (1024 * 1024) },
                Fill = null,
                GeometrySize = 0,
                LineSmoothness = 0.5,
                Stroke = new SolidColorPaint(SKColors.LimeGreen) { StrokeThickness = 2 }
            });
            Series.Add(new LineSeries<double>
            {
                Name = "Įkėlimas",
                Values = new ObservableCollection<double> { e.UploadSpeed / (1024 * 1024) },
                Fill = null,
                GeometrySize = 0,
                LineSmoothness = 0.5,
                Stroke = new SolidColorPaint(SKColors.OrangeRed) { StrokeThickness = 2 }
            });
        }
        else
        {
            var downloadSeries = (LineSeries<double>)Series[0];
            var uploadSeries = (LineSeries<double>)Series[1];
            var downloadValues = (ObservableCollection<double>)downloadSeries.Values!;
            var uploadValues = (ObservableCollection<double>)uploadSeries.Values!;

            if (downloadValues.Count > 60)
            {
                downloadValues.RemoveAt(0);
                uploadValues.RemoveAt(0);
            }
            downloadValues.Add(e.DownloadSpeed / (1024 * 1024));
            uploadValues.Add(e.UploadSpeed / (1024 * 1024));
        }
    }

    private void OnDriveMetricsUpdated(object? sender, DriveMetricsEventArgs e)
    {
        var usedGB = e.UsedSpace / 1024;
        var totalGB = e.TotalSpace / 1024;
        var usedPercent = (e.UsedSpace / e.TotalSpace) * 100;

        AddOrUpdateValue($"{e.DriveLetter} naudojama", $"{usedGB:F1}/{totalGB:F1} GB", usedPercent);
        if (e.Temperature > 0)
        {
            AddOrUpdateValue($"{e.DriveLetter} temperatūra", $"{e.Temperature:F1}°C", e.Temperature);
        }
        AddOrUpdateValue($"{e.DriveLetter} aktyvumas", $"{e.ActivityPercent:F1}%", e.ActivityPercent);

        // Atnaujinti grafiką
        if (Series.Count == 0)
        {
            Series.Add(new LineSeries<double>
            {
                Values = new ObservableCollection<double> { e.ActivityPercent },
                Fill = null,
                GeometrySize = 0,
                LineSmoothness = 0.5,
                Stroke = new SolidColorPaint(SKColors.Gold) { StrokeThickness = 2 }
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
            values.Add(e.ActivityPercent);
        }
    }

    private void AddOrUpdateValue(string label, string value, double percentage)
    {
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
}

public class MetricValue
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public double Percentage { get; set; }
    public Brush ProgressBrush { get; set; } = Brushes.Blue;
} 