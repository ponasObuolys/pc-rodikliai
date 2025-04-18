using System;

namespace PC_Rodikliai.Services.HardwareMonitor;

public interface IHardwareMonitorService
{
    void StartMonitoring();
    void StopMonitoring();

    event EventHandler<CpuMetricsEventArgs> CpuMetricsUpdated;
    event EventHandler<RamMetricsEventArgs> RamMetricsUpdated;
    event EventHandler<GpuMetricsEventArgs> GpuMetricsUpdated;
    event EventHandler<NetworkMetricsEventArgs> NetworkMetricsUpdated;
    event EventHandler<DriveMetricsEventArgs> DriveMetricsUpdated;
}

public class CpuMetricsEventArgs : EventArgs
{
    public float TotalLoad { get; set; }
    public float Temperature { get; set; }
    public float ClockSpeed { get; set; }
    public float[] CoreLoads { get; set; } = Array.Empty<float>();
}

public class RamMetricsEventArgs : EventArgs
{
    public float UsedMemory { get; set; }
    public float TotalMemory { get; set; }
    public float MemoryLoad { get; set; }
}

public class GpuMetricsEventArgs : EventArgs
{
    public float CoreLoad { get; set; }
    public float Temperature { get; set; }
    public float MemoryUsed { get; set; }
    public float MemoryTotal { get; set; }
    public float FanSpeed { get; set; }
}

public class NetworkMetricsEventArgs : EventArgs
{
    public float UploadSpeed { get; set; }
    public float DownloadSpeed { get; set; }
    public float UploadTotal { get; set; }
    public float DownloadTotal { get; set; }
}

public class DriveMetricsEventArgs : EventArgs
{
    public string DriveLetter { get; set; } = string.Empty;
    public float UsedSpace { get; set; }
    public float TotalSpace { get; set; }
    public float Temperature { get; set; }
    public float ActivityPercent { get; set; }
} 