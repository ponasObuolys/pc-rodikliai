using System;

namespace PC_Rodikliai.Services.HardwareMonitor
{
    public class CpuMetricsEventArgs : EventArgs
    {
        public double TotalLoad { get; set; }
        public double Temperature { get; set; }
        public double Power { get; set; }
        public double Frequency { get; set; }
    }
} 