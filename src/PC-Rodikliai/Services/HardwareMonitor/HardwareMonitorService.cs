using System;
using System.Linq;
using System.Timers;
using LibreHardwareMonitor.Hardware;

namespace PC_Rodikliai.Services.HardwareMonitor;

public class HardwareMonitorService : IHardwareMonitorService
{
    private readonly Computer _computer;
    private readonly Timer _updateTimer;
    private double _lastCpuUsage;
    private double _lastRamUsage;
    private double _lastDiskUsage;
    private double _lastNetworkSpeed;

    public event EventHandler<CpuMetricsEventArgs>? CpuMetricsUpdated;
    public event EventHandler<RamMetricsEventArgs>? RamMetricsUpdated;
    public event EventHandler<GpuMetricsEventArgs>? GpuMetricsUpdated;
    public event EventHandler<NetworkMetricsEventArgs>? NetworkMetricsUpdated;
    public event EventHandler<DriveMetricsEventArgs>? DriveMetricsUpdated;

    public HardwareMonitorService()
    {
        _computer = new Computer
        {
            IsCpuEnabled = true,
            IsMemoryEnabled = true,
            IsStorageEnabled = true,
            IsNetworkEnabled = true
        };

        _updateTimer = new Timer(1000); // Update every second
        _updateTimer.Elapsed += OnUpdateTimerElapsed;
    }

    public void StartMonitoring()
    {
        _computer.Open();
        _updateTimer.Start();
    }

    public void StopMonitoring()
    {
        _updateTimer.Stop();
        _computer.Close();
    }

    public double GetCpuUsage() => _lastCpuUsage;
    public double GetRamUsage() => _lastRamUsage;
    public double GetDiskUsage() => _lastDiskUsage;
    public double GetNetworkSpeed() => _lastNetworkSpeed;

    private void OnUpdateTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        try
        {
            _computer.Accept(new UpdateVisitor());

            // Update CPU usage
            var cpuLoad = _computer.Hardware
                .Where(h => h.HardwareType == HardwareType.Cpu)
                .SelectMany(cpu => cpu.Sensors)
                .Where(sensor => sensor.SensorType == SensorType.Load)
                .Select(sensor => sensor.Value)
                .DefaultIfEmpty(0)
                .Average() ?? 0;

            _lastCpuUsage = cpuLoad;

            // Update RAM usage
            var ramUsage = _computer.Hardware
                .Where(h => h.HardwareType == HardwareType.Memory)
                .SelectMany(ram => ram.Sensors)
                .Where(sensor => sensor.SensorType == SensorType.Load)
                .Select(sensor => sensor.Value)
                .DefaultIfEmpty(0)
                .First() ?? 0;

            _lastRamUsage = ramUsage;

            // Raise event with updated metrics
            CpuMetricsUpdated?.Invoke(this, new CpuMetricsEventArgs { TotalLoad = (float)_lastCpuUsage });
        }
        catch (Exception ex)
        {
            // Log error
            System.Diagnostics.Debug.WriteLine($"Error updating metrics: {ex.Message}");
        }
    }

    private void UpdateRamMetrics(IHardware ram)
    {
        var used = ram.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Data && s.Name == "Memory Used")?.Value ?? 0;
        var available = ram.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Data && s.Name == "Memory Available")?.Value ?? 0;
        var total = used + available;
        var load = (used / total) * 100;

        RamMetricsUpdated?.Invoke(this, new RamMetricsEventArgs
        {
            UsedMemory = used,
            TotalMemory = total,
            MemoryLoad = load
        });
    }

    private void UpdateGpuMetrics(IHardware gpu)
    {
        var load = gpu.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name == "GPU Core")?.Value ?? 0;
        var temp = gpu.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Temperature && s.Name == "GPU Core")?.Value ?? 0;
        var memUsed = gpu.Sensors.FirstOrDefault(s => s.SensorType == SensorType.SmallData && s.Name == "GPU Memory Used")?.Value ?? 0;
        var memTotal = gpu.Sensors.FirstOrDefault(s => s.SensorType == SensorType.SmallData && s.Name == "GPU Memory Total")?.Value ?? 0;
        var fan = gpu.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Fan && s.Name == "GPU Fan")?.Value ?? 0;

        GpuMetricsUpdated?.Invoke(this, new GpuMetricsEventArgs
        {
            CoreLoad = load,
            Temperature = temp,
            MemoryUsed = memUsed,
            MemoryTotal = memTotal,
            FanSpeed = fan
        });
    }

    private void UpdateNetworkMetrics(IHardware network)
    {
        var upload = network.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Throughput && s.Name.Contains("Upload"))?.Value ?? 0;
        var download = network.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Throughput && s.Name.Contains("Download"))?.Value ?? 0;
        var uploadTotal = network.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Data && s.Name.Contains("Upload"))?.Value ?? 0;
        var downloadTotal = network.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Data && s.Name.Contains("Download"))?.Value ?? 0;

        NetworkMetricsUpdated?.Invoke(this, new NetworkMetricsEventArgs
        {
            UploadSpeed = upload,
            DownloadSpeed = download,
            UploadTotal = uploadTotal,
            DownloadTotal = downloadTotal
        });
    }

    private void UpdateDriveMetrics(IHardware drive)
    {
        var used = drive.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Data && s.Name == "Used Space")?.Value ?? 0;
        var total = drive.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Data && s.Name == "Total Space")?.Value ?? 0;
        var temp = drive.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Temperature)?.Value ?? 0;
        var activity = drive.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name == "Total Activity")?.Value ?? 0;

        DriveMetricsUpdated?.Invoke(this, new DriveMetricsEventArgs
        {
            DriveLetter = drive.Name,
            UsedSpace = used,
            TotalSpace = total,
            Temperature = temp,
            ActivityPercent = activity
        });
    }
}

public class UpdateVisitor : IVisitor
{
    public void VisitComputer(IComputer computer)
    {
        computer.Traverse(this);
    }

    public void VisitHardware(IHardware hardware)
    {
        hardware.Update();
        foreach (var subHardware in hardware.SubHardware)
        {
            subHardware.Accept(this);
        }
    }

    public void VisitSensor(ISensor sensor) { }
    public void VisitParameter(IParameter parameter) { }
} 