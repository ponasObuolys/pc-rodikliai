using System.Windows;
using System.Windows.Input;
using PC_Rodikliai.Services.HardwareMonitor;
using System;

namespace PC_Rodikliai;

public partial class MainWindow : Window
{
    private readonly IHardwareMonitorService _hardwareMonitor;
    private bool _isDragging;
    private Point _dragStart;
    private System.Windows.Threading.DispatcherTimer _updateTimer;

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
        Closed += OnClosed;

        // Grafiko atnaujinimo laikmatis
        _updateTimer = new System.Windows.Threading.DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _updateTimer.Tick += UpdateTimer_Tick;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _hardwareMonitor.StartMonitoring();
        _updateTimer.Start();
    }

    private void OnClosed(object sender, EventArgs e)
    {
        _updateTimer.Stop();
        _hardwareMonitor.StopMonitoring();
    }

    private void UpdateTimer_Tick(object sender, EventArgs e)
    {
        try
        {
            var cpuUsage = _hardwareMonitor.GetCpuUsage();
            CpuChart.UpdateValue(cpuUsage);
        }
        catch (Exception ex)
        {
            // TODO: Pridėti tinkamą klaidų apdorojimą
            System.Diagnostics.Debug.WriteLine($"Klaida atnaujinant grafiką: {ex.Message}");
        }
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