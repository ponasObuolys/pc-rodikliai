using System.Windows;
using System.Windows.Input;
using PC_Rodikliai.Services.HardwareMonitor;

namespace PC_Rodikliai;

public partial class MainWindow : Window
{
    private readonly IHardwareMonitorService _hardwareMonitor;
    private bool _isDragging;
    private Point _dragStart;

    public MainWindow(IHardwareMonitorService hardwareMonitor)
    {
        InitializeComponent();
        _hardwareMonitor = hardwareMonitor;

        // Lango pozicionavimas
        Left = SystemParameters.WorkArea.Width - Width - 10;
        Top = (SystemParameters.WorkArea.Height - Height) / 2;

        // Įvykių prenumeravimas
        MouseLeftButtonDown += OnMouseLeftButtonDown;
        MouseLeftButtonUp += OnMouseLeftButtonUp;
        MouseMove += OnMouseMove;

        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _hardwareMonitor.StartMonitoring();
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _isDragging = true;
        _dragStart = e.GetPosition(this);
        CaptureMouse();
    }

    private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _isDragging = false;
        ReleaseMouseCapture();
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (_isDragging)
        {
            var currentPos = e.GetPosition(this);
            var offset = currentPos - _dragStart;
            Left += offset.X;
            Top += offset.Y;
        }
    }

    private void OnSettingsClick(object sender, RoutedEventArgs e)
    {
        // TODO: Atidaryti nustatymų langą
    }

    private void OnMinimizeClick(object sender, RoutedEventArgs e)
    {
        Hide();
    }

    private void OnCloseClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void OnShowClick(object sender, RoutedEventArgs e)
    {
        Show();
        Activate();
    }
} 