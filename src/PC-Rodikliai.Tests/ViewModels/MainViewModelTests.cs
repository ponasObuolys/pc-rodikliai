using Xunit;
using Moq;
using PC_Rodikliai.ViewModels;
using PC_Rodikliai.Services.HardwareMonitor;

namespace PC_Rodikliai.Tests.ViewModels;

public class MainViewModelTests
{
    private readonly Mock<IHardwareMonitorService> _hardwareMonitorMock;
    private readonly MainViewModel _viewModel;

    public MainViewModelTests()
    {
        _hardwareMonitorMock = new Mock<IHardwareMonitorService>();
        _viewModel = new MainViewModel(_hardwareMonitorMock.Object);
    }

    [Fact]
    public void Constructor_ShouldInitializeMetrics()
    {
        // Assert
        Assert.NotNull(_viewModel.Metrics);
        Assert.Equal(5, _viewModel.Metrics.Count); // CPU, RAM, GPU, Network, Disks
    }

    [Fact]
    public void Constructor_ShouldStartMonitoring()
    {
        // Verify
        _hardwareMonitorMock.Verify(x => x.StartMonitoring(), Times.Once);
    }

    [Fact]
    public void Metrics_ShouldHaveCorrectTitles()
    {
        // Assert
        Assert.Collection(_viewModel.Metrics,
            metric => Assert.Equal("CPU", metric.Title),
            metric => Assert.Equal("RAM", metric.Title),
            metric => Assert.Equal("GPU", metric.Title),
            metric => Assert.Equal("Tinklas", metric.Title),
            metric => Assert.Equal("Diskai", metric.Title)
        );
    }
} 