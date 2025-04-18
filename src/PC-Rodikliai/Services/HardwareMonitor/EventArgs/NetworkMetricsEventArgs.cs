using System;

namespace PC_Rodikliai.Services.HardwareMonitor
{
    public class NetworkMetricsEventArgs : EventArgs
    {
        public float UploadSpeed { get; set; }
        public float DownloadSpeed { get; set; }
        public float UploadTotal { get; set; }
        public float DownloadTotal { get; set; }
    }
} 