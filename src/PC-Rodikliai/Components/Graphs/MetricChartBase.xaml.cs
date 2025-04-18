using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WPF;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Windows.Media;

namespace PC_Rodikliai.Components.Graphs
{
    public class MetricChartBase : UserControl
    {
        public required CartesianChart Chart { get; set; }
        protected ObservableCollection<double> Values { get; set; }
        protected int MaxDataPoints { get; set; } = 60; // 1 minute of data at 1 second intervals

        public MetricChartBase()
        {
            Values = new ObservableCollection<double>();
            InitializeChart();
        }

        protected virtual void InitializeChart()
        {
            Chart = new CartesianChart
            {
                Series = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Values = Values,
                        Fill = null,
                        GeometrySize = 0,
                        LineSmoothness = 0.5,
                        Stroke = new SolidColorPaint(SKColors.DodgerBlue) { StrokeThickness = 2 }
                    }
                },
                XAxes = new[]
                {
                    new Axis
                    {
                        IsVisible = false,
                        LabelsPaint = new SolidColorPaint(SKColors.Gray),
                        TextSize = 10
                    }
                },
                YAxes = new[]
                {
                    new Axis
                    {
                        LabelsPaint = new SolidColorPaint(SKColors.Gray),
                        TextSize = 10,
                        MinLimit = 0,
                        MaxLimit = 100
                    }
                }
            };
        }

        public virtual void UpdateValue(double value)
        {
            if (Values.Count >= MaxDataPoints)
            {
                Values.RemoveAt(0);
            }
            Values.Add(value);
        }

        public virtual void Clear()
        {
            Values.Clear();
        }
    }
} 