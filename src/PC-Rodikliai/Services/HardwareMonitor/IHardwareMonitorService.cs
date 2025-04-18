using System;

namespace PC_Rodikliai.Services.HardwareMonitor
{
    public interface IHardwareMonitorService
    {
        void StartMonitoring();
        void StopMonitoring();
        double GetCpuUsage();
        double GetRamUsage();
        double GetDiskUsage();
        double GetNetworkSpeed();

        event EventHandler<CpuMetricsEventArgs> CpuMetricsUpdated;
        event EventHandler<RamMetricsEventArgs> RamMetricsUpdated;
        event EventHandler<GpuMetricsEventArgs> GpuMetricsUpdated;
        event EventHandler<NetworkMetricsEventArgs> NetworkMetricsUpdated;
        event EventHandler<DriveMetricsEventArgs> DriveMetricsUpdated;
    }
} 