   M S B A M L   `   `   `  ����R  NLib.Controls.MyControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullL;clr-namespace:Lib.Controls;assembly=Lib.Controls.MyControlsLib.Controls  5   	    MatrixSystemEditor,   &MatrixSystemEditor.commom.CNewDeveName   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Joc;clr-namespace:Lib.Controls;assembly=Lib.Controls.MyControls     5   	   -   5        ���#B1B1B1  � Title$  Change new device name��5   	   $	��160��6(   $	��240��65    � 
ResizeMode$ NoResize=�6A    � WindowStartupLocation$ CenterScreen=�6W   .��5
      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,    � ColumnDefinitions 5   
   �� 5      6!   �� 5      $��60��6   6/   .+�5   
   �� -   5        $��
subTitleLB��5      $��Input New Device Name:��5      $��Center=�62   $��5q�5      
�������5      �� 5   
   -   5        $��edInput��5      $��1��5      $��5q�6   $��28��6*    �� 	MaxLength$ 16��5      
�������5   P   ]� 5   
   $��2��6   $��0,10,0,0q�6!   .+�5      ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   .+�5      �� -   5        $��	submitBtn��5   n   $��0,0,5,0q�6   $��5,5,1,5q�6*   ���#B1B1B1$��72��6Q   $��28��6_   
���   �.��6�   Submit6�   �� 5      -   5        $��exitBtn��5   n   $��5,0,0,0q�6   $��1,5,5,5q�6*   ���#B1B1B1$��72��6Q   $��28��6_   
���   �.��6�   Exit6�   5      5      "   Lib.Controls.SprocketControl  5    
   -   5          Name$ 
rotateSpin��5!      $��1��5       $��2��6-   $��0,5,0,0q�5"   '   $	��Top=�5#   '      	TickColor$ #0000A0��5$   '   $��60��5%   '   $��60��5&   '   	   	TickWidth$	 4Q�5'   '   
   	TickCount$
 10��5(   '      
StartAngle$	 -90Q�5*   '      IsIndeterminate .     Interval$	 450Q�5,   '      	TickStyle$ Triangle=�5-   '   5/       50      51      