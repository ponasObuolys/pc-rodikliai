using Xunit;
using PC_Rodikliai.Services.HardwareMonitor;
using PC_Rodikliai.ViewModels;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Linq;
using System;

namespace PC_Rodikliai.Tests.Integration;

public class MetricsIntegrationTests : IDisposable
{
    private readonly HardwareMonitorService _service;
    private readonly MainViewModel _mainViewModel;

    public MetricsIntegrationTests()
    {
        _service = new HardwareMonitorService();
        _mainViewModel = new MainViewModel(_service);
    }

    [Fact]
    public async Task FullDataFlow_ShouldUpdateUICorrectly()
    {
        // Arrange
        var cpuMetrics = _mainViewModel.Metrics.First(m => m.Title == "CPU");
        var ramMetrics = _mainViewModel.Metrics.First(m => m.Title == "RAM");

        // Act
        _service.StartMonitoring();
        
        // Wait for a few updates
        await Task.Delay(2000);

        // Assert
        Assert.NotEmpty(cpuMetrics.Values);
        Assert.NotEmpty(cpuMetrics.Series);
        Assert.NotEmpty(ramMetrics.Values);
        Assert.NotEmpty(ramMetrics.Series);

        // Verify CPU metrics format
        var cpuLoad = cpuMetrics.Values.FirstOrDefault(v => v.Label == "Apkrova");
        Assert.NotNull(cpuLoad);
        Assert.Contains("%", cpuLoad.Value);
        Assert.InRange(cpuLoad.Percentage, 0, 100);

        // Verify RAM metrics format
        var ramUsed = ramMetrics.Values.FirstOrDefault(v => v.Label == "Naudojama");
        Assert.NotNull(ramUsed);
        Assert.Contains("GB", ramUsed.Value);
        Assert.InRange(ramUsed.Percentage, 0, 100);
    }

    [Fact]
    public async Task ServiceRestart_ShouldResumeDataFlow()
    {
        // Arrange
        var cpuMetrics = _mainViewModel.Metrics.First(m => m.Title == "CPU");
        
        // Act
        _service.StartMonitoring();
        await Task.Delay(1000);
        
        var initialValuesCount = cpuMetrics.Values.Count;
        var initialSeriesCount = cpuMetrics.Series.Count;

        _service.StopMonitoring();
        await Task.Delay(500);
        
        _service.StartMonitoring();
        await Task.Delay(1000);

        // Assert
        Assert.True(cpuMetrics.Values.Count >= initialValuesCount);
        Assert.Equal(initialSeriesCount, cpuMetrics.Series.Count);
    }

    [Fact]
    public async Task MultipleMetrics_ShouldUpdateIndependently()
    {
        // Arrange
        var cpuMetrics = _mainViewModel.Metrics.First(m => m.Title == "CPU");
        var ramMetrics = _mainViewModel.Metrics.First(m => m.Title == "RAM");
        var networkMetrics = _mainViewModel.Metrics.First(m => m.Title == "Tinklas");

        // Act
        _service.StartMonitoring();
        await Task.Delay(2000);

        // Assert
        // Verify that each metric type has its own specific values
        Assert.Contains(cpuMetrics.Values, v => v.Label == "Apkrova");
        Assert.Contains(ramMetrics.Values, v => v.Label == "Naudojama");
        Assert.Contains(networkMetrics.Values, v => v.Label == "Atsisiuntimas");

        // Verify that series are updated independently
        Assert.NotEqual(cpuMetrics.Series.Count, networkMetrics.Series.Count);
    }

    [Fact]
    public async Task ChartUpdates_ShouldMaintainDataIntegrity()
    {
        // Arrange
        var cpuMetrics = _mainViewModel.Metrics.First(m => m.Title == "CPU");
        
        // Act
        _service.StartMonitoring();
        await Task.Delay(3000); // Wait for multiple updates

        // Assert
        var series = cpuMetrics.Series.First();
        var values = (System.Collections.ObjectModel.ObservableCollection<double>)series.Values!;

        // Verify that values are maintained within bounds
        Assert.All(values, value => Assert.InRange(value, 0, 100));

        // Verify that data points are sequential
        var previousValue = values[0];
        for (int i = 1; i < values.Count; i++)
        {
            // Values should not have huge jumps (more than 50% difference)
            Assert.True(Math.Abs(values[i] - previousValue) < 50);
            previousValue = values[i];
        }
    }

    public void Dispose()
    {
        _service.StopMonitoring();
        _service.Dispose();
    }
} 