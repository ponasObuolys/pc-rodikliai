   M S B A M L   `   `   `  ���� G  CPC-Rodikliai, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:PC_Rodikliai.Components.GraphsPC_Rodikliai.Components.Graphs  1    +PC_Rodikliai.Components.Metrics.MetricsView   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ?graphs,clr-namespace:PC_Rodikliai.Components.Graphs     .��� .+��� $
��Auto=�.��� .+��� $	��300��.+�0   *PC_Rodikliai.Components.Graphs.MetricGraph    � DataContext  �� *CpuGraphViewModel+�� $��16(�$
��10,5q����� *
CpuUsage+ �� StringFormat$ 	CPU: {0}%���� $	��300��.+�    �� *RamGraphViewModel+�� $��16(�$
��10,5q����� *
RamUsage+$ 	RAM: {0}%���� $	��300��.+�    �� *DiskGraphViewModel+�� $��16(�$
��10,5q����� *	DiskUsage+$ 
Disk: {0}%���� $	��300��.+�    �� *NetworkGraphViewModel+�� $��16(�$
��10,5q����� *NetworkSpeed+$ Network: {0} MB/s��