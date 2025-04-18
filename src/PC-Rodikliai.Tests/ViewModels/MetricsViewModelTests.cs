using Xunit;
using Moq;
using PC_Rodikliai.ViewModels;
using PC_Rodikliai.Services.HardwareMonitor;
using System.Windows.Media;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace PC_Rodikliai.Tests.ViewModels;

public class MetricsViewModelTests
{
    private readonly Mock<IHardwareMonitorService> _hardwareMonitorMock;
    private readonly MetricsViewModel _viewModel;
    private readonly string _title = "CPU";
    private readonly string _icon = "🔲";
    private readonly Brush _progressBrush = Brushes.OrangeRed;

    public MetricsViewModelTests()
    {
        _hardwareMonitorMock = new Mock<IHardwareMonitorService>();
        _viewModel = new MetricsViewModel(_hardwareMonitorMock.Object, _title, _icon, _progressBrush);
    }

    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Assert
        Assert.Equal(_title, _viewModel.Title);
        Assert.NotNull(_viewModel.Values);
        Assert.NotNull(_viewModel.Series);
        Assert.Empty(_viewModel.Values);
        Assert.Empty(_viewModel.Series);
    }

    [Fact]
    public void Constructor_ShouldInitializeAxes()
    {
        // Assert
        Assert.NotNull(_viewModel.XAxes);
        Assert.NotNull(_viewModel.YAxes);
        Assert.Single(_viewModel.XAxes);
        Assert.Single(_viewModel.YAxes);
        Assert.False(_viewModel.XAxes[0].IsVisible);
        Assert.False(_viewModel.YAxes[0].IsVisible);
    }

    [Theory]
    [InlineData("CPU", typeof(CpuMetricsEventArgs))]
    [InlineData("RAM", typeof(RamMetricsEventArgs))]
    [InlineData("GPU", typeof(GpuMetricsEventArgs))]
    [InlineData("Tinklas", typeof(NetworkMetricsEventArgs))]
    [InlineData("Diskai", typeof(DriveMetricsEventArgs))]
    public void Constructor_ShouldSubscribeToCorrectEvents(string title, Type eventArgsType)
    {
        // Arrange
        var hardwareMonitorMock = new Mock<IHardwareMonitorService>();
        
        // Act
        var viewModel = new MetricsViewModel(hardwareMonitorMock.Object, title, _icon, _progressBrush);

        // Assert
        if (title == "CPU")
            hardwareMonitorMock.Verify(x => x.CpuMetricsUpdated += It.IsAny<EventHandler<CpuMetricsEventArgs>>(), Times.Once);
        else if (title == "RAM")
            hardwareMonitorMock.Verify(x => x.RamMetricsUpdated += It.IsAny<EventHandler<RamMetricsEventArgs>>(), Times.Once);
        else if (title == "GPU")
            hardwareMonitorMock.Verify(x => x.GpuMetricsUpdated += It.IsAny<EventHandler<GpuMetricsEventArgs>>(), Times.Once);
        else if (title == "Tinklas")
            hardwareMonitorMock.Verify(x => x.NetworkMetricsUpdated += It.IsAny<EventHandler<NetworkMetricsEventArgs>>(), Times.Once);
        else if (title == "Diskai")
            hardwareMonitorMock.Verify(x => x.DriveMetricsUpdated += It.IsAny<EventHandler<DriveMetricsEventArgs>>(), Times.Once);
    }

    [Fact]
    public void OnCpuMetricsUpdated_ShouldUpdateValuesAndSeries()
    {
        // Arrange
        var cpuMetrics = new CpuMetricsEventArgs
        {
            TotalLoad = 50.5,
            Temperature = 65.3,
            Frequency = 3600,
            Power = 65.7
        };

        // Act
        _hardwareMonitorMock.Raise(x => x.CpuMetricsUpdated += null, cpuMetrics);

        // Assert
        Assert.Equal(4, _viewModel.Values.Count);
        Assert.Single(_viewModel.Series);

        var loadValue = _viewModel.Values.First(v => v.Label == "Apkrova");
        Assert.Equal("50.5%", loadValue.Value);
        Assert.Equal(50.5, loadValue.Percentage);

        var tempValue = _viewModel.Values.First(v => v.Label == "Temperatūra");
        Assert.Equal("65.3°C", tempValue.Value);
        Assert.Equal(65.3, tempValue.Percentage);

        var series = (LineSeries<double>)_viewModel.Series[0];
        var values = (ObservableCollection<double>)series.Values!;
        Assert.Contains(50.5, values);
    }

    [Fact]
    public void OnRamMetricsUpdated_ShouldUpdateValuesAndSeries()
    {
        // Arrange
        var ramMetrics = new RamMetricsEventArgs
        {
            UsedMemory = 8192, // 8 GB
            TotalMemory = 16384, // 16 GB
            MemoryLoad = 50
        };

        // Act
        var viewModel = new MetricsViewModel(_hardwareMonitorMock.Object, "RAM", _icon, _progressBrush);
        _hardwareMonitorMock.Raise(x => x.RamMetricsUpdated += null, ramMetrics);

        // Assert
        Assert.Equal(3, viewModel.Values.Count);
        Assert.Single(viewModel.Series);

        var usedValue = viewModel.Values.First(v => v.Label == "Naudojama");
        Assert.Equal("8.0 GB", usedValue.Value);
        Assert.Equal(50, usedValue.Percentage);

        var series = (LineSeries<double>)viewModel.Series[0];
        var values = (ObservableCollection<double>)series.Values!;
        Assert.Contains(50, values);
    }

    [Fact]
    public void OnNetworkMetricsUpdated_ShouldUpdateValuesAndSeries()
    {
        // Arrange
        var networkMetrics = new NetworkMetricsEventArgs
        {
            DownloadSpeed = 1048576, // 1 MB/s
            UploadSpeed = 524288, // 0.5 MB/s
            DownloadTotal = 1073741824, // 1 GB
            UploadTotal = 536870912 // 0.5 GB
        };

        // Act
        var viewModel = new MetricsViewModel(_hardwareMonitorMock.Object, "Tinklas", _icon, _progressBrush);
        _hardwareMonitorMock.Raise(x => x.NetworkMetricsUpdated += null, networkMetrics);

        // Assert
        Assert.Equal(4, viewModel.Values.Count);
        Assert.Equal(2, viewModel.Series.Count);

        var downloadValue = viewModel.Values.First(v => v.Label == "Atsisiuntimas");
        Assert.Contains("MB/s", downloadValue.Value);

        var uploadValue = viewModel.Values.First(v => v.Label == "Įkėlimas");
        Assert.Contains("KB/s", uploadValue.Value);

        var downloadSeries = (LineSeries<double>)viewModel.Series[0];
        var uploadSeries = (LineSeries<double>)viewModel.Series[1];
        Assert.Equal("Atsisiuntimas", downloadSeries.Name);
        Assert.Equal("Įkėlimas", uploadSeries.Name);
    }
} 