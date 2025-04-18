using Xunit;
using PC_Rodikliai.Components.Graphs;
using System.Windows.Media;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using SkiaSharp;

namespace PC_Rodikliai.Tests.Components.Graphs;

public class ChartsTests
{
    [Fact]
    public void MetricChartBase_Constructor_ShouldInitializeCorrectly()
    {
        // Act
        var chart = new MetricChartBase();

        // Assert
        Assert.NotNull(chart.Chart);
        Assert.NotNull(chart.Values);
        Assert.Empty(chart.Values);
        Assert.Equal(60, chart.MaxDataPoints);
    }

    [Fact]
    public void MetricChartBase_InitializeChart_ShouldSetupCorrectAxes()
    {
        // Act
        var chart = new MetricChartBase();

        // Assert
        var cartesianChart = chart.Chart;
        Assert.NotNull(cartesianChart.XAxes);
        Assert.NotNull(cartesianChart.YAxes);
        Assert.Single(cartesianChart.XAxes);
        Assert.Single(cartesianChart.YAxes);

        var xAxis = cartesianChart.XAxes[0];
        Assert.False(xAxis.IsVisible);
        Assert.NotNull(xAxis.LabelsPaint);

        var yAxis = cartesianChart.YAxes[0];
        Assert.Equal(0, yAxis.MinLimit);
        Assert.Equal(100, yAxis.MaxLimit);
        Assert.NotNull(yAxis.LabelsPaint);
    }

    [Fact]
    public void MetricChartBase_Series_ShouldBeConfiguredCorrectly()
    {
        // Act
        var chart = new MetricChartBase();

        // Assert
        Assert.NotNull(chart.Chart.Series);
        Assert.Single(chart.Chart.Series);

        var series = chart.Chart.Series[0] as LineSeries<double>;
        Assert.NotNull(series);
        Assert.Null(series.Fill);
        Assert.Equal(0, series.GeometrySize);
        Assert.Equal(0.5, series.LineSmoothness);
        
        var stroke = series.Stroke as SolidColorPaint;
        Assert.NotNull(stroke);
        Assert.Equal(2, stroke.StrokeThickness);
        Assert.Equal(SKColors.DodgerBlue, stroke.Color);
    }

    [Fact]
    public void MetricChartBase_AddValue_ShouldRespectMaxDataPoints()
    {
        // Arrange
        var chart = new MetricChartBase();
        var series = chart.Chart.Series[0] as LineSeries<double>;
        var values = series!.Values as ObservableCollection<double>;

        // Act
        for (int i = 0; i < 70; i++) // Add more than MaxDataPoints
        {
            values!.Add(i);
        }

        // Assert
        Assert.Equal(60, values!.Count); // Should be limited to MaxDataPoints
        Assert.Equal(10, values[0]); // First value should be shifted
        Assert.Equal(69, values[59]); // Last value should be newest
    }

    [Theory]
    [InlineData(-10)]
    [InlineData(150)]
    public void MetricChartBase_AddValue_ShouldHandleOutOfRangeValues(double value)
    {
        // Arrange
        var chart = new MetricChartBase();
        var series = chart.Chart.Series[0] as LineSeries<double>;
        var values = series!.Values as ObservableCollection<double>;

        // Act
        values!.Add(value);

        // Assert
        Assert.Single(values);
        // Values should still be plotted even if outside normal range
        Assert.Equal(value, values[0]);
    }
} 