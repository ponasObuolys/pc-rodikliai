using System;
using System.Collections.Generic;
using System.Windows.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace PC_Rodikliai.Components.Graphs
{
    public partial class MetricGraph : UserControl, IDisposable
    {
        private bool _disposed;

        public MetricGraph()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            if (_disposed) return;
            
            _disposed = true;
            DataContext = null;
            GC.SuppressFinalize(this);
        }
    }

    public class MetricGraphViewModel : IDisposable
    {
        private bool _disposed;
        private readonly object _lock = new();
        private readonly Queue<double> _values;
        private readonly int _maxPoints;
        private readonly LineSeries<double> _series;

        public new string Title { get; set; } = string.Empty;
        public ISeries[] Series { get; }
        public Axis[]? XAxes { get; }
        public Axis[]? YAxes { get; }

        public MetricGraphViewModel(string title, int maxPoints = 60)
        {
            Title = title;
            _maxPoints = maxPoints;
            _values = new Queue<double>(_maxPoints);

            _series = new LineSeries<double>
            {
                Values = Array.Empty<double>(),
                Fill = null,
                GeometrySize = 0,
                LineSmoothness = 0.5,
                Stroke = new SolidColorPaint(SKColors.DodgerBlue) { StrokeThickness = 2 }
            };

            Series = new ISeries[] { _series };

            XAxes = new[]
            {
                new Axis
                {
                    IsVisible = false,
                    LabelsPaint = null
                }
            };

            YAxes = new[]
            {
                new Axis
                {
                    IsVisible = false,
                    LabelsPaint = null
                }
            };
        }

        public void AddValue(double value)
        {
            if (_disposed) return;

            lock (_lock)
            {
                if (_values.Count >= _maxPoints)
                {
                    _values.Dequeue();
                }

                _values.Enqueue(value);
                _series.Values = _values.ToArray();
            }
        }

        public void Clear()
        {
            if (_disposed) return;

            lock (_lock)
            {
                _values.Clear();
                _series.Values = Array.Empty<double>();
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            Clear();
            GC.SuppressFinalize(this);
        }

        ~MetricGraphViewModel()
        {
            Dispose();
        }
    }
} 