   M S B A M L   `   `   `  ���� G  CPC-Rodikliai, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:PC_RodikliaiPC_Rodikliai  M+clr-namespace:PC_Rodikliai.Components.ClockPC_Rodikliai.Components.Clock  Q-clr-namespace:PC_Rodikliai.Components.MetricsPC_Rodikliai.Components.Metrics  O,clr-namespace:PC_Rodikliai.Components.GraphsPC_Rodikliai.Components.Graphs  S.clr-namespace:PC_Rodikliai.Components.TrayIcon PC_Rodikliai.Components.TrayIcon      PC_Rodikliai.MainWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,localclr-namespace:PC_Rodikliai     =clock+clr-namespace:PC_Rodikliai.Components.Clock     Ametrics-clr-namespace:PC_Rodikliai.Components.Metrics     ?graphs,clr-namespace:PC_Rodikliai.Components.Graphs     _ [Hardcodet.NotifyIcon.Wpf, Version=1.1.0.0, Culture=neutral, PublicKeyToken=682384a853a08aad)tb http://www.hardcodet.net/taskbar  =trayIcon.clr-namespace:PC_Rodikliai.Components.TrayIcon    � Title$  PC Rodikliai��$	��800��$	��400�� � Icon$/ )pack://application:,,,/Resources/icon.ico�� � WindowStyle$
 None=� � AllowsTransparency$
 True��
������  � Topmost$
 True�� � ShowInTaskbar$ False�� � 
ResizeMode$ 	CanResize=� � 	Resources    ShowSidebar �� TargetProperty	 W� From	
 W� To g� Duration  HideSidebar��%�   &	        &	 9     �� .��W� $ Opacity�	 0.0
 1.0$ 0:0:0.2D��� .��W� $ Opacity�	 1.0
 0.0$ 0:0:0.2D�.��� .+���   BackgroundBrush#��[   BorderBrush#��[ $��1q� �� CornerRadius$ 10��$��5q� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     ShadowDepth$ 0Q�   
BlurRadius$ 10Q�   Opacity$	 0.5Q�._��  � RowDefinitions �� $
��Auto ��� $��* �.+�� $��0��$��40�� � ColumnDefinitions �� $��* ��� $
��Auto �.+�0   *PC_Rodikliai.Components.Clock.ClockControl  $��0��$��10,0,0,0q��� $��1�� �� Orientation$ 
Horizontal=�$��0,0,10,0q�.+��� -     WindowControlButton#��[ .���� $	��⚙��$��16(��� -   #��[ .���� $	��—��$��16(��� -   #��[ .���� $��×��$��16(�� $��1�� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�2   ,PC_Rodikliai.Components.Graphs.CpuUsageChart  -     Name$ CpuChart��$��0��$	��150��$��10q�2   ,PC_Rodikliai.Components.Graphs.RamUsageChart  -     Name$ RamChart��$��1��$	��150��$��10q�6   0PC_Rodikliai.Components.Graphs.NetworkSpeedChart  -     Name$ NetworkChart��$��2��$	��150��$��10q�3   -PC_Rodikliai.Components.Graphs.DiskUsageChart  -     Name$ 	DiskChart��$��3��$	��150��$��10q�3  -Hardcodet.Wpf.TaskbarNotification.TaskbarIcon  -     Name$ 
NotifyIcon��   
IconSource$/ )pack://application:,,,/Resources/icon.ico��   ToolTipText$ PC Rodikliai�� � ContextMenu �� .@�m� -	   $��Rodyti��m� -
   $��
Nustatymai���� m� -   $��	Uždaryti��3   -PC_Rodikliai.Components.TrayIcon.TrayIconView   � DataContext �� *TrayIconViewModel+