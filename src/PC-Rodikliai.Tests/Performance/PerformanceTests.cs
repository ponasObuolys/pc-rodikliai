using Xunit;
using PC_Rodikliai.Services.HardwareMonitor;
using PC_Rodikliai.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace PC_Rodikliai.Tests.Performance;

public class PerformanceTests : IDisposable
{
    private readonly HardwareMonitorService _service;
    private readonly MainViewModel _mainViewModel;
    private readonly Process _currentProcess;

    public PerformanceTests()
    {
        _service = new HardwareMonitorService();
        _mainViewModel = new MainViewModel(_service);
        _currentProcess = Process.GetCurrentProcess();
    }

    [Fact]
    public async Task MemoryUsage_ShouldRemainStable()
    {
        // Arrange
        var memoryReadings = new List<long>();
        var startMemory = _currentProcess.WorkingSet64;

        // Act
        _service.StartMonitoring();

        // Collect memory readings over 10 seconds
        for (int i = 0; i < 10; i++)
        {
            await Task.Delay(1000);
            _currentProcess.Refresh();
            memoryReadings.Add(_currentProcess.WorkingSet64);
        }

        // Assert
        var memoryGrowth = memoryReadings.Last() - startMemory;
        var maxGrowthMB = memoryGrowth / (1024 * 1024); // Convert to MB

        // Memory growth should be less than 50MB over 10 seconds
        Assert.True(maxGrowthMB < 50, $"Memory growth ({maxGrowthMB}MB) exceeded acceptable limit");

        // Check for memory leaks (continuous growth)
        var growthRate = memoryReadings.Zip(memoryReadings.Skip(1), (a, b) => b - a).Average();
        Assert.True(growthRate < 1024 * 1024, "Memory usage shows signs of continuous growth");
    }

    [Fact]
    public async Task CpuUsage_ShouldBeReasonable()
    {
        // Arrange
        var cpuReadings = new List<TimeSpan>();
        var startTime = _currentProcess.TotalProcessorTime;

        // Act
        _service.StartMonitoring();

        // Collect CPU usage readings over 5 seconds
        for (int i = 0; i < 5; i++)
        {
            await Task.Delay(1000);
            _currentProcess.Refresh();
            cpuReadings.Add(_currentProcess.TotalProcessorTime);
        }

        // Assert
        var cpuTimeIncrease = cpuReadings.Last() - startTime;
        var cpuUsagePercent = (cpuTimeIncrease.TotalMilliseconds / (Environment.ProcessorCount * 5000)) * 100;

        // CPU usage should be less than 10% per core on average
        Assert.True(cpuUsagePercent < 10, $"CPU usage ({cpuUsagePercent:F1}%) exceeded acceptable limit");
    }

    [Fact]
    public async Task UIResponseTime_ShouldBeFast()
    {
        // Arrange
        var stopwatch = new Stopwatch();
        var responseTimes = new List<long>();

        // Act
        _service.StartMonitoring();

        // Measure UI update response times
        for (int i = 0; i < 10; i++)
        {
            stopwatch.Restart();
            
            // Trigger UI updates
            foreach (var metric in _mainViewModel.Metrics)
            {
                var valuesBefore = metric.Values.Count;
                await Task.Delay(100); // Wait for potential updates
                var valuesAfter = metric.Values.Count;
                
                if (valuesAfter > valuesBefore)
                {
                    responseTimes.Add(stopwatch.ElapsedMilliseconds);
                    break;
                }
            }
        }

        // Assert
        var averageResponseTime = responseTimes.Average();
        var maxResponseTime = responseTimes.Max();

        // Average response time should be under 100ms
        Assert.True(averageResponseTime < 100, $"Average UI response time ({averageResponseTime:F1}ms) too high");
        
        // Maximum response time should be under 200ms
        Assert.True(maxResponseTime < 200, $"Maximum UI response time ({maxResponseTime}ms) too high");
    }

    [Fact]
    public async Task DataCollection_ShouldBeEfficient()
    {
        // Arrange
        var metrics = _mainViewModel.Metrics;
        var dataPoints = new List<int>();
        var startTime = DateTime.Now;

        // Act
        _service.StartMonitoring();
        await Task.Delay(5000); // Collect data for 5 seconds

        // Count total data points collected
        foreach (var metric in metrics)
        {
            dataPoints.Add(metric.Values.Count);
            if (metric.Series.Any())
            {
                var series = metric.Series.First();
                var values = (System.Collections.ObjectModel.ObservableCollection<double>)series.Values!;
                dataPoints.Add(values.Count);
            }
        }

        // Assert
        var totalDataPoints = dataPoints.Sum();
        var timeElapsed = (DateTime.Now - startTime).TotalSeconds;
        var dataPointsPerSecond = totalDataPoints / timeElapsed;

        // Should collect reasonable amount of data points (not too many, not too few)
        Assert.True(dataPointsPerSecond >= 1, "Data collection rate too low");
        Assert.True(dataPointsPerSecond <= 100, "Data collection rate too high");
    }

    public void Dispose()
    {
        _service.StopMonitoring();
        _service.Dispose();
    }
} 