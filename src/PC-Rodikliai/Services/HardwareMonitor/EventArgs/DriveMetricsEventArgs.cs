using System;

namespace PC_Rodikliai.Services.HardwareMonitor
{
    public class DriveMetricsEventArgs : EventArgs
    {
        public string DriveLetter { get; set; } = string.Empty;
        public float UsedSpace { get; set; }
        public float TotalSpace { get; set; }
        public float Temperature { get; set; }
        public float ActivityPercent { get; set; }
    }
} 