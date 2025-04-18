using System;

namespace PC_Rodikliai.Services.HardwareMonitor
{
    public class CpuMetricsEventArgs : EventArgs
    {
        public float TotalLoad { get; set; }
        public float Temperature { get; set; }
        public float ClockSpeed { get; set; }
        public float[] CoreLoads { get; set; } = Array.Empty<float>();
    }
} 