using System;

namespace PC_Rodikliai.Services.HardwareMonitor
{
    public class GpuMetricsEventArgs : EventArgs
    {
        public float CoreLoad { get; set; }
        public float Temperature { get; set; }
        public float MemoryUsed { get; set; }
        public float MemoryTotal { get; set; }
        public float FanSpeed { get; set; }
    }
} 