using System.Windows.Controls;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;

namespace PC_Rodikliai.Components.Graphs
{
    public partial class NetworkSpeedChart : UserControl
    {
        private ObservableCollection<double> DownloadValues { get; set; }
        private ObservableCollection<double> UploadValues { get; set; }
        private int MaxDataPoints { get; set; } = 60;

        public NetworkSpeedChart()
        {
            InitializeComponent();
            DownloadValues = new ObservableCollection<double>();
            UploadValues = new ObservableCollection<double>();
            InitializeChart();
        }

        private void InitializeChart()
        {
            NetworkChart.Series = new ISeries[]
            {
                new LineSeries<double>
                {
                    Name = "Atsisiuntimas",
                    Values = DownloadValues,
                    Fill = null,
                    GeometrySize = 0,
                    LineSmoothness = 0.5,
                    Stroke = new SolidColorPaint(SKColors.LimeGreen) { StrokeThickness = 2 }
                },
                new LineSeries<double>
                {
                    Name = "Įkėlimas",
                    Values = UploadValues,
                    Fill = null,
                    GeometrySize = 0,
                    LineSmoothness = 0.5,
                    Stroke = new SolidColorPaint(SKColors.OrangeRed) { StrokeThickness = 2 }
                }
            };

            NetworkChart.XAxes = new[]
            {
                new Axis
                {
                    IsVisible = false,
                    LabelsPaint = new SolidColorPaint(SKColors.Gray),
                    TextSize = 10
                }
            };

            NetworkChart.YAxes = new[]
            {
                new Axis
                {
                    Name = "MB/s",
                    NamePaint = new SolidColorPaint(SKColors.White),
                    LabelsPaint = new SolidColorPaint(SKColors.LightGray),
                    TextSize = 12,
                    MinLimit = 0
                }
            };
        }

        public void UpdateValues(double downloadSpeed, double uploadSpeed)
        {
            // Konvertuojame į MB/s
            downloadSpeed = downloadSpeed / (1024 * 1024);
            uploadSpeed = uploadSpeed / (1024 * 1024);

            if (DownloadValues.Count >= MaxDataPoints)
            {
                DownloadValues.RemoveAt(0);
                UploadValues.RemoveAt(0);
            }
            DownloadValues.Add(downloadSpeed);
            UploadValues.Add(uploadSpeed);
        }

        public void Clear()
        {
            DownloadValues.Clear();
            UploadValues.Clear();
        }
    }
} 