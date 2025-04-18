using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PC_Rodikliai.Components.Clock;

public partial class ClockControl : UserControl
{
    private readonly DispatcherTimer _timer;

    public ClockControl()
    {
        InitializeComponent();

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += Timer_Tick;
        _timer.Start();

        UpdateTime();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        TimeText.Text = DateTime.Now.ToString("HH:mm:ss");
    }
} 