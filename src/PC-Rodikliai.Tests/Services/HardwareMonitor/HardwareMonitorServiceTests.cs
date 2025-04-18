using Xunit;
using Moq;
using PC_Rodikliai.Services.HardwareMonitor;
using System;
using System.Security.Principal;
using System.Diagnostics;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace PC_Rodikliai.Tests.Services.HardwareMonitor;

public class HardwareMonitorServiceTests : IDisposable
{
    private readonly HardwareMonitorService _service;
    private readonly Mock<IComputer> _computerMock;
    private readonly Mock<IHardware> _cpuMock;
    private readonly Mock<IHardware> _ramMock;
    private readonly Mock<IHardware> _gpuMock;
    private readonly Mock<IHardware> _networkMock;
    private readonly Mock<IHardware> _driveMock;
    private bool _isAdmin;

    public HardwareMonitorServiceTests()
    {
        _computerMock = new Mock<IComputer>();
        _cpuMock = new Mock<IHardware>();
        _ramMock = new Mock<IHardware>();
        _gpuMock = new Mock<IHardware>();
        _networkMock = new Mock<IHardware>();
        _driveMock = new Mock<IHardware>();

        _service = new HardwareMonitorService();
        CheckAdminRights();
    }

    private void CheckAdminRights()
    {
        using var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        _isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    [Fact]
    public void StartMonitoring_WithoutAdminRights_ShouldThrowException()
    {
        if (_isAdmin)
        {
            Debug.WriteLine("Testas praleistas, nes vykdoma su administratoriaus teisėmis");
            return;
        }

        // Act & Assert
        var exception = Assert.Throws<UnauthorizedAccessException>(() => _service.StartMonitoring());
        Assert.Contains("administratoriaus teisėmis", exception.Message);
    }

    [Fact]
    public void StartMonitoring_WithAdminRights_ShouldNotThrowException()
    {
        if (!_isAdmin)
        {
            Debug.WriteLine("Testas praleistas, nes vykdoma be administratoriaus teisių");
            return;
        }

        // Act & Assert
        var exception = Record.Exception(() => _service.StartMonitoring());
        Assert.Null(exception);
    }

    [Fact]
    public void GetCpuUsage_WithoutStartingMonitoring_ShouldReturnZero()
    {
        // Act
        var usage = _service.GetCpuUsage();

        // Assert
        Assert.Equal(0, usage);
    }

    [Fact]
    public void GetRamUsage_WithoutStartingMonitoring_ShouldReturnZero()
    {
        // Act
        var usage = _service.GetRamUsage();

        // Assert
        Assert.Equal(0, usage);
    }

    [Fact]
    public void GetDiskUsage_WithoutStartingMonitoring_ShouldReturnZero()
    {
        // Act
        var usage = _service.GetDiskUsage();

        // Assert
        Assert.Equal(0, usage);
    }

    [Fact]
    public void GetNetworkSpeed_WithoutStartingMonitoring_ShouldReturnZero()
    {
        // Act
        var speed = _service.GetNetworkSpeed();

        // Assert
        Assert.Equal(0, speed);
    }

    [Fact]
    public void CpuMetricsUpdated_ShouldProvideValidData()
    {
        if (!_isAdmin)
        {
            Debug.WriteLine("Testas praleistas, nes vykdoma be administratoriaus teisių");
            return;
        }

        // Arrange
        CpuMetricsEventArgs? receivedArgs = null;
        _service.CpuMetricsUpdated += (sender, args) => receivedArgs = args;

        // Act
        _service.StartMonitoring();
        System.Threading.Thread.Sleep(2000); // Laukiame duomenų

        // Assert
        Assert.NotNull(receivedArgs);
        Assert.InRange(receivedArgs.TotalLoad, 0, 100);
        Assert.True(receivedArgs.Temperature >= 0);
        Assert.True(receivedArgs.Power >= 0);
        Assert.True(receivedArgs.Frequency > 0);
    }

    [Fact]
    public void RamMetricsUpdated_ShouldProvideValidData()
    {
        if (!_isAdmin)
        {
            Debug.WriteLine("Testas praleistas, nes vykdoma be administratoriaus teisių");
            return;
        }

        // Arrange
        RamMetricsEventArgs? receivedArgs = null;
        _service.RamMetricsUpdated += (sender, args) => receivedArgs = args;

        // Act
        _service.StartMonitoring();
        System.Threading.Thread.Sleep(2000); // Laukiame duomenų

        // Assert
        Assert.NotNull(receivedArgs);
        Assert.True(receivedArgs.UsedMemory > 0);
        Assert.True(receivedArgs.TotalMemory > 0);
        Assert.True(receivedArgs.MemoryLoad >= 0 && receivedArgs.MemoryLoad <= 100);
    }

    [Fact]
    public void StartMonitoring_ShouldInitializeHardware()
    {
        // Act
        _service.StartMonitoring();

        // Assert
        Assert.True(_service.IsMonitoring);
    }

    [Fact]
    public void StopMonitoring_ShouldStopUpdates()
    {
        // Arrange
        _service.StartMonitoring();

        // Act
        _service.StopMonitoring();

        // Assert
        Assert.False(_service.IsMonitoring);
    }

    [Fact]
    public void GetCpuUsage_WhenNotMonitoring_ShouldReturnZero()
    {
        // Act
        var usage = _service.GetCpuUsage();

        // Assert
        Assert.Equal(0, usage);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void UpdateCpuMetrics_WithInvalidValues_ShouldNormalize(double invalidLoad)
    {
        // Arrange
        var cpuLoadSensor = new Mock<ISensor>();
        cpuLoadSensor.Setup(s => s.SensorType).Returns(SensorType.Load);
        cpuLoadSensor.Setup(s => s.Name).Returns("CPU Total");
        cpuLoadSensor.Setup(s => s.Value).Returns((float)invalidLoad);

        _cpuMock.Setup(h => h.Sensors).Returns(new[] { cpuLoadSensor.Object });

        // Act & Assert
        var eventRaised = false;
        _service.CpuMetricsUpdated += (s, e) =>
        {
            eventRaised = true;
            Assert.InRange(e.TotalLoad, 0, 100);
        };

        // Simulate update
        _service.StartMonitoring();
        _computerMock.Raise(c => c.HardwareUpdated += null, new HardwareEventArgs(_cpuMock.Object));

        Assert.True(eventRaised);
    }

    [Fact]
    public void UpdateRamMetrics_WithZeroTotal_ShouldHandleGracefully()
    {
        // Arrange
        var ramUsedSensor = new Mock<ISensor>();
        ramUsedSensor.Setup(s => s.SensorType).Returns(SensorType.Data);
        ramUsedSensor.Setup(s => s.Name).Returns("Memory Used");
        ramUsedSensor.Setup(s => s.Value).Returns(0f);

        var ramAvailableSensor = new Mock<ISensor>();
        ramAvailableSensor.Setup(s => s.SensorType).Returns(SensorType.Data);
        ramAvailableSensor.Setup(s => s.Name).Returns("Memory Available");
        ramAvailableSensor.Setup(s => s.Value).Returns(0f);

        _ramMock.Setup(h => h.Sensors).Returns(new[] { ramUsedSensor.Object, ramAvailableSensor.Object });

        // Act & Assert
        var eventRaised = false;
        _service.RamMetricsUpdated += (s, e) =>
        {
            eventRaised = true;
            Assert.Equal(0, e.MemoryLoad);
            Assert.Equal(0, e.TotalMemory);
        };

        // Simulate update
        _service.StartMonitoring();
        _computerMock.Raise(c => c.HardwareUpdated += null, new HardwareEventArgs(_ramMock.Object));

        Assert.True(eventRaised);
    }

    [Fact]
    public void UpdateNetworkMetrics_WithExtremeValues_ShouldHandleGracefully()
    {
        // Arrange
        var uploadSensor = new Mock<ISensor>();
        uploadSensor.Setup(s => s.SensorType).Returns(SensorType.Throughput);
        uploadSensor.Setup(s => s.Name).Returns("Upload Speed");
        uploadSensor.Setup(s => s.Value).Returns(float.MaxValue);

        var downloadSensor = new Mock<ISensor>();
        downloadSensor.Setup(s => s.SensorType).Returns(SensorType.Throughput);
        downloadSensor.Setup(s => s.Name).Returns("Download Speed");
        downloadSensor.Setup(s => s.Value).Returns(float.MaxValue);

        _networkMock.Setup(h => h.Sensors).Returns(new[] { uploadSensor.Object, downloadSensor.Object });

        // Act & Assert
        var eventRaised = false;
        _service.NetworkMetricsUpdated += (s, e) =>
        {
            eventRaised = true;
            Assert.True(e.UploadSpeed >= 0);
            Assert.True(e.DownloadSpeed >= 0);
        };

        // Simulate update
        _service.StartMonitoring();
        _computerMock.Raise(c => c.HardwareUpdated += null, new HardwareEventArgs(_networkMock.Object));

        Assert.True(eventRaised);
    }

    [Fact]
    public void UpdateDriveMetrics_WithMissingSensors_ShouldHandleGracefully()
    {
        // Arrange
        _driveMock.Setup(h => h.Sensors).Returns(Array.Empty<ISensor>());
        _driveMock.Setup(h => h.Name).Returns("C:");

        // Act & Assert
        var eventRaised = false;
        _service.DriveMetricsUpdated += (s, e) =>
        {
            eventRaised = true;
            Assert.Equal("C:", e.DriveLetter);
            Assert.Equal(0, e.UsedSpace);
            Assert.Equal(0, e.TotalSpace);
            Assert.Equal(0, e.Temperature);
            Assert.Equal(0, e.ActivityPercent);
        };

        // Simulate update
        _service.StartMonitoring();
        _computerMock.Raise(c => c.HardwareUpdated += null, new HardwareEventArgs(_driveMock.Object));

        Assert.True(eventRaised);
    }

    [Fact]
    public async Task MonitoringLoop_ShouldHandleExceptions()
    {
        // Arrange
        _computerMock.Setup(c => c.Hardware).Throws<Exception>();

        // Act & Assert
        _service.StartMonitoring();
        await Task.Delay(100); // Give time for monitoring loop to run

        // Should not throw and service should still be running
        Assert.True(_service.IsMonitoring);
    }

    public void Dispose()
    {
        try
        {
            _service.StopMonitoring();
        }
        catch
        {
            // Ignoruojame klaidas uždarant
        }
    }
} 