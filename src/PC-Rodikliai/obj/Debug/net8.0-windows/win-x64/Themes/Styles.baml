   M S B A M L   `   `   `  ����G  CPC-Rodikliai, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:PC_Rodikliai.ConvertersPC_Rodikliai.Converters  5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   BaseButtonStyle  �� 
TargetType �� Property �� Value  	TextBrush �� 
TargetType �� CornerRadius �� Triggers P� Property P� Value  SecondaryBrush  WindowControlButton �� BasedOn  AccentBrush  MetricHeaderStyle  PrimaryTextBrush  MetricValueStyle  MetricLabelStyle 	 SecondaryTextBrush 
 BorderBrush  MetricCardStyle3   -System.Windows.Media.Effects.DropShadowEffect	    
BlurRadius
    ShadowDepth    Opacity    Color  MetricProgressBar ��Name `� 	Converter7   1PC_Rodikliai.Converters.ProgressBarWidthConverter �� Path �� RelativeSource�� 5      H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml     5      /local%clr-namespace:PC_Rodikliai.Converters  5      %�
  &	        7  7  &	 (    7   7  &	 '    7  &	 �    7  &	 u    7 	 '� �    7 
 &	 g    7  7 
 &	 �    7  �� 5      "  ��6$   .�5   
   ��  � ��
 ���� 6'   �� 5	   
    � ��$ 0q�6,   �� 5
   
    � ��8   6'   �� 5   
    � ��$
 10,5q�6$   �� 5   
    �@�Cursor$
 Hand��6#   �� 5   
    � �� 5      �� 5      " ��6"   .R�5      �� #��z��6   #��z��5      #��z��5      $ 5��5      ._�5      �� $��Center=�6+   $��Center=�5   )   5      5      5       5   
   P� 5       �@J�IsMouseOver$
 True��6-   . �5      ��  � ��8  6/   5      5      �� 5"      "  ��6(   8   .�5#   
   ��  � ��$ 30��6"   �� 5$   
    � ��$ 30��6#   �� 5%   
    � ��$ 2q�6#    5&   
   P� 5'       �@J�IsMouseOver$
 True��6-   . �5(      ��  � ��8  6/   5)      5+      �� 5.      "  ��6&   .�5/   
   ��  � ��$ 20(�6%   �� 50   
    � ��$ SemiBold"�6'   �� 51   
    � ��8   6'   �� 52   
    � ��$ 0,0,0,10q�6#   53      �� 55      "  ��6%   .�56   
   ��  � ��$ 16(�6%   �� 57   
    � ��$ SemiBold"�6'   �� 58   
    � ��8   6'   59      �� 5;      "  ��6%   .�5<   
   ��  � ��$ 14(�6%   �� 5=   
    � ��8   6'   5>      �� 5A      "  �6   .�5B   
   ��  � ��$ 2��6#   �� 5C   
    � ��8   6'   �� 5D   
    � ��$ 0q�6,   5E      �� 5H      "  ��6$   .�5I   
   ��  � ��8   6'   �� 5J   
    � ��8  6(   �� 5K   
    � ��$ 1q�6,   �� 5L   
    �@��CornerRadius$ 10��6)   �� 5M   
    � ��$ 10q�6#   �� 5N   
    �@��Padding$ 15q�6$   �� 5O   
    �@J�Effect 5P         5Q      $	 10Q�6#   $
 1Q�5R   !   $	 0.2Q�5S   !   $
 Gray��5T   !   5V      5W      �� 5Z      "  �6&   .�5[   
   ��  � ��$ 4��6#   �� 5\   
    � ��$	 0,2q�6#   �� 5]   
    � ��8   6'   �� 5^   
    � ��$ 0q�6,   �� 5_   
    � �� 5`      �� 5a      " �6"   .R�5b      �� #��z��6   #��z��5c      #��z��5d      $ 2��5e      ._�5f      �� $ PART_Indicator��6!   #��z��5g   !   #��z��5h   !   #��z��5i   !   $
��Left=�5j   !   $ 2��5k   !   ��5l      `� 5m   "    5n   &     5o   *   .1�5q   &   �� $ Value�5r   .    5q   .   �� *TemplatedParent+5r   .   �� 5s   &   $ Minimum�5t   .    5s   .   �� *TemplatedParent+5t   .   �� 5u   &   $ Maximum�5v   .    5u   .   �� *TemplatedParent+5v   .   �� 5w   &   $ ActualWidth�5x   .    5w   .   �� *TemplatedParent+5x   .   5y   #   5{      5|      5}      5      5�      5�      