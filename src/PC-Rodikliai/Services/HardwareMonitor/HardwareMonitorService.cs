using System;
using System.Linq;
using System.Timers;
using LibreHardwareMonitor.Hardware;

namespace PC_Rodikliai.Services.HardwareMonitor;

public class HardwareMonitorService : IHardwareMonitorService
{
    private readonly Computer _computer;
    private readonly Timer _updateTimer;
    private readonly UpdateVisitor _updateVisitor;

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
            IsGpuEnabled = true,
            IsMemoryEnabled = true,
            IsMotherboardEnabled = true,
            IsStorageEnabled = true,
            IsNetworkEnabled = true
        };

        _updateVisitor = new UpdateVisitor();
        _updateTimer = new Timer(1000); // 1 sekundė
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

    private void OnUpdateTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _computer.Accept(_updateVisitor);

        foreach (var hardware in _computer.Hardware)
        {
            switch (hardware.HardwareType)
            {
                case HardwareType.Cpu:
                    UpdateCpuMetrics(hardware);
                    break;
                case HardwareType.Memory:
                    UpdateRamMetrics(hardware);
                    break;
                case HardwareType.GpuNvidia:
                case HardwareType.GpuAmd:
                    UpdateGpuMetrics(hardware);
                    break;
                case HardwareType.Network:
                    UpdateNetworkMetrics(hardware);
                    break;
                case HardwareType.Storage:
                    UpdateDriveMetrics(hardware);
                    break;
            }
        }
    }

    private void UpdateCpuMetrics(IHardware cpu)
    {
        var totalLoad = cpu.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name == "CPU Total")?.Value ?? 0;
        var temperature = cpu.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Temperature && s.Name == "CPU Package")?.Value ?? 0;
        var clockSpeed = cpu.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Clock && s.Name == "CPU Core #1")?.Value ?? 0;
        var coreLoads = cpu.Sensors.Where(s => s.SensorType == SensorType.Load && s.Name.StartsWith("CPU Core #"))
                                 .Select(s => s.Value ?? 0)
                                 .ToArray();

        CpuMetricsUpdated?.Invoke(this, new CpuMetricsEventArgs
        {
            TotalLoad = totalLoad,
            Temperature = temperature,
            ClockSpeed = clockSpeed,
            CoreLoads = coreLoads
        });
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