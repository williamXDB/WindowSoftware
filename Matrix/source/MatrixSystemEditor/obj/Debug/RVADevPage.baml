   M S B A M L   `   `   `  ����R  NLib.Controls.MyControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullL;clr-namespace:Lib.Controls;assembly=Lib.Controls.MyControlsLib.Controls  5   	    MatrixSystemEditor#   MatrixSystemEditor.RVADevPage   5   
   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Joc;clr-namespace:Lib.Controls;assembly=Lib.Controls.MyControls     5   	   -   5        ���#604A7B  � Title$  RVA200 Editor page��5   	   $	��200��6$   $	��350��61    � 
ResizeMode$ NoResize=�5   
    � WindowStartupLocation$ CenterScreen=�6    
�������.��5
      �  � ShowGridLines .   � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      6    � ColumnDefinitions 5   
   �� 5      $	��180 �6   6-   �� 5      6!   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��5q�6.   $��1��69   .+�5      �� $��App ID:��6   $��Center=�6'   6D   �� 5      -   5        $��	labelAPID��5   A   $��0x0A00��6   $��Center=�6&   6T   5      ]� 5   
   $��0��6   $��0��6"   $��5,5,0,5q�62   .+�5      �� $ 
Horizontal=�6   .+�5      �� $��Connect Status:��6   $��Center=�63   6P      Lib.Controls.CLed  5       -   5        $��conLed��5    1   $��30��6   $��15��6%   $��10,0,0,0q�6?   6S   5!      5"      ]� 5#   
   $��0��6   $��1��6"   .+�5$      �� -   5        $��aesPanel��5$   2   $ 
Horizontal=�6   $y�Hidden=�6B   .+�5%      �� $��AES Status:��6   $��Center=�6/   6L     5&      -   5        $��aesLed��5&   C   $��30��6   $��15��6%   $��10,0,0,0q�61   6S   5'      5(      ]� 5)   
   $��2��6   .+�5*      �� $ 
Horizontal=�6   $	��163��62   .+�5+      �� $��Device Name:��6   $��0,0,0,0q�60   $��Center=�6A   6^   �� 5,      -   5        $��	lbDevName��5,   `   $��RVA200��6   $��1,0,0,0q�6(   $��28��69   $��Center=�6E   $��80��5-      $	��100��6(    �� 
IsReadOnly . 
�������5.      5/      50      ]� 53   
   $��1��6   $��1��6$   .+�54      �� $ 
Horizontal=�6   .+�55      �� $��
Device ID:��6   $��Center=�6.   $��0,0,0,0q�6I   6\   �� 56      -   5        $��
labelDevID��56   D   $��10000��6   $��Center=�6)   6X   57      58      ]� 59   
   $
��Left=�6   $��1��6/   $��2��6?   $��0,5,5,5q�6L   .+�5:      �� $ 
Horizontal=�6   .+�5;      �� $��Firmware Version:��6   $��Center=�65   $
��Left=�6P   6m   �� 5<      -   5        $
��fmLb��5<   V   $��v1.2.3��6   $��Center=�6*   $��5,1,1,1q�6E   6d   5=      5>      �� 5?   
   -	   5        $��cgDevNameBtn��5@   
   $��3��5?      $��5,1,5,1q�6   ���#B1B1B1$��2��6F   $��5q�6[   $��40��6g   
���   �.��5@   L   CHANGE DEVICE NAME6`   5B      5C      