using System.Windows;
using PC_Rodikliai.ViewModels;
using PC_Rodikliai.Services.HardwareMonitor;

namespace PC_Rodikliai.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(IHardwareMonitorService hardwareMonitor)
        {
            InitializeComponent();
            DataContext = new MainViewModel(hardwareMonitor);
        }
    }
} 