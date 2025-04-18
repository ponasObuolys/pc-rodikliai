using Xunit;
using Moq;
using PC_Rodikliai.Views;
using PC_Rodikliai.Services.HardwareMonitor;
using System.Windows;
using System.Windows.Input;

namespace PC_Rodikliai.Tests.Views;

public class MainWindowTests
{
    private readonly Mock<IHardwareMonitorService> _hardwareMonitorMock;

    public MainWindowTests()
    {
        _hardwareMonitorMock = new Mock<IHardwareMonitorService>();
    }

    [StaFact]
    public void Constructor_ShouldInitializeBasicProperties()
    {
        // Arrange
        var window = new MainWindow(_hardwareMonitorMock.Object);

        // Assert
        Assert.NotNull(window);
        Assert.NotNull(window.DataContext);
    }

    [StaFact]
    public void OnLoaded_ShouldStartMonitoring()
    {
        // Arrange
        _hardwareMonitorMock.Setup(x => x.StartMonitoring()).Verifiable();
        var window = new MainWindow(_hardwareMonitorMock.Object);

        // Act
        window.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

        // Assert
        _hardwareMonitorMock.Verify(x => x.StartMonitoring(), Times.Once);
    }

    [StaFact]
    public void OnClosed_ShouldStopMonitoring()
    {
        // Arrange
        _hardwareMonitorMock.Setup(x => x.StopMonitoring()).Verifiable();
        var window = new MainWindow(_hardwareMonitorMock.Object);

        // Act
        window.Close();

        // Assert
        _hardwareMonitorMock.Verify(x => x.StopMonitoring(), Times.Once);
    }
} 