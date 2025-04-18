using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace PC_Rodikliai.Components.Metrics;

public partial class MetricsControl : UserControl
{
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(MetricsControl), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(string), typeof(MetricsControl), new PropertyMetadata(string.Empty));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public ObservableCollection<MetricValue> Values { get; } = new();
    public ObservableCollection<ISeries> Series { get; } = new();

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

    public MetricsControl()
    {
        InitializeComponent();
        DataContext = this;
    }

    public void AddOrUpdateValue(string label, string value, double percentage, Brush progressBrush)
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
                ProgressBrush = progressBrush
            });
        }
    }

    public void UpdateSeries(ISeries series)
    {
        var existingSeries = Series.FirstOrDefault(s => s.Name == series.Name);
        if (existingSeries != null)
        {
            Series[Series.IndexOf(existingSeries)] = series;
        }
        else
        {
            Series.Add(series);
        }
    }
}

public class MetricValue
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public double Percentage { get; set; }
    public Brush ProgressBrush { get; set; } = Brushes.Blue;
} 