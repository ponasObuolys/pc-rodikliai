using System.Windows.Controls;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;

namespace PC_Rodikliai.Components.Graphs
{
    public partial class DiskUsageChart : UserControl
    {
        private ObservableCollection<double> Values { get; set; }
        private int MaxDataPoints { get; set; } = 60;

        public DiskUsageChart()
        {
            InitializeComponent();
            Values = new ObservableCollection<double>();
            InitializeChart();
        }

        private void InitializeChart()
        {
            DiskChart.Series = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = Values,
                    Fill = null,
                    GeometrySize = 0,
                    LineSmoothness = 0.5,
                    Stroke = new SolidColorPaint(SKColors.Gold) { StrokeThickness = 2 }
                }
            };

            DiskChart.XAxes = new[]
            {
                new Axis
                {
                    IsVisible = false,
                    LabelsPaint = new SolidColorPaint(SKColors.Gray),
                    TextSize = 10
                }
            };

            DiskChart.YAxes = new[]
            {
                new Axis
                {
                    Name = "Aktyvumas %",
                    NamePaint = new SolidColorPaint(SKColors.White),
                    LabelsPaint = new SolidColorPaint(SKColors.LightGray),
                    TextSize = 12,
                    MinLimit = 0,
                    MaxLimit = 100
                }
            };
        }

        public void UpdateValue(double value)
        {
            if (Values.Count >= MaxDataPoints)
            {
                Values.RemoveAt(0);
            }
            Values.Add(value);
        }

        public void Clear()
        {
            Values.Clear();
        }
    }
} 