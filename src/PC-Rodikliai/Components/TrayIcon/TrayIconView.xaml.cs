using System.Windows;
using System.Windows.Controls;

namespace PC_Rodikliai.Components.TrayIcon
{
    public partial class TrayIconView : UserControl
    {
        public TrayIconView()
        {
            InitializeComponent();
        }

        private void OnOpenClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Show();
            Application.Current.MainWindow?.Activate();
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
} 