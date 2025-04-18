using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using PC_Rodikliai.ViewModels;

namespace PC_Rodikliai.Views;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer _updateTimer;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = ((App)Application.Current).Services.GetRequiredService<MainWindowViewModel>();

        _updateTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _updateTimer.Tick += UpdateTimer_Tick;
        _updateTimer.Start();

        Closed += OnClosed;
    }

    private void OnClosed(object sender, EventArgs e)
    {
        _updateTimer.Stop();
        if (DataContext is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private void UpdateTimer_Tick(object sender, EventArgs e)
    {
        Title = $"PC Rodikliai - {DateTime.Now:HH:mm:ss}";
    }
} 