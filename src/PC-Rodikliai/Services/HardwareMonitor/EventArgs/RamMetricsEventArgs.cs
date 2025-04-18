using System;

namespace PC_Rodikliai.Services.HardwareMonitor
{
    public class RamMetricsEventArgs : EventArgs
    {
        public float UsedMemory { get; set; }
        public float TotalMemory { get; set; }
        public float MemoryLoad { get; set; }
    }
} 