   M S B A M L   `   `   `  ����M  IMatrixSystemEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:MatrixSystemEditorMatrixSystemEditor  5   	   R NLib.Controls.MyControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullL;clr-namespace:Lib.Controls;assembly=Lib.Controls.MyControlsLib.Controls 5   	   #    MatrixSystemEditor.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   2local clr-namespace:MatrixSystemEditor     5   	   Joc;clr-namespace:Lib.Controls;assembly=Lib.Controls.MyControls     5   	   -   5        ���#EFEFEF  � Title$!  Matrix System Editor V1.3.9��5   	   $	��768��6-   $
��1024��6:    � WindowStartupLocation$ CenterScreen=�5   	    � WindowState$ 	Maximized=�5	        � 	Resources 5
         PopMenu��%�   &	        �� 5   
   .@�5      m� $��Change DeviceID��6   �� 5      m� 5      $��Delete Module��6   5      5      .��5      � -   5        $��rootGrid��5       � ShowGridLines .   � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,    � ColumnDefinitions 5   
   �� 5      6"   �� 5       $
��Auto �6   6.   .+�5"   
   ��  �� Orientation$ 
Horizontal=�6   .+�5#      o� -   5        $��topMenu��5#   J   $��0��6   
������ $��5,5,5,8q�69   .@�5$      m� $��System��6   $��21��6+   .@�5%      m� -   5        $
��exit��5%   -   $
��Exit��6   5&      5'      �� 5(      6   m� 5)      $��About��6   $��21��6*   .@�5*      m� -   5        $��verDlg��5*   0   $��Version��6   5+      5,      �� 5-      6   54      �� 55      $ 
Horizontal=�6   $��80,0,0,0q�62   .+�56      �� -   5        $��
connetItem��56      $��5q�6,   $��30��68   ���#EFEFEF$��60��57   /   .��58   2   	Connect6;   �� 59      -   5        $��netConfigItem��59      $��5q�6/   $��30��6;   ���#EFEFEF$��10,0,0,0q�5:   .   $��60��6@   .��5;   5   Setup6<   �� 5<      6   �� 5=      $��status:��6   $��Center=�6+   $��20,0,2,0q�5>       5?   #     Lib.Controls.CLed  5@      -   5        $��conLed��5@   1   $��30��6   $��15��6%      
GreenBrush$
 Lime��6?   6S   5B      5C      ]� 5D   
   -	   5        $��	mainPanel��5D      $��1��6%   ��. .+�5E      � -
   5        $��mainGrid��5E       .   5F      �� 5G      $
��Auto �6'   66   �� 5H      $��* �6'   63   .+�5J        Lib.Controls.CTreeView  -   5        $��devieListTree��5J   .   $	��178��6   ��5K      �� 5L      	 �� ImageSource$	 Resources/i_LeftList.png��6%   .@�5N      Q� $��Config Device List��6#   
 Q� 
IsExpanded
 . .@�5O      Q� $��0��6(   ��5P         Lib.Controls.CTreeViewItem  5Q   "      
itemSource$ Resources/folder.png��64      itemText$
 CL-4��6W   $��16��6g   $��0,2,5,2q�6v   6�   5S      Q� 5T      $��1��6(   ��5U        5V   "   $ Resources/folder.png��64   $ Lan Interface��6W   $��16��6p   $��0,2,5,2q�6   6�   5X      Q� 5Y      $��2��6'   ��5Z        5[   "   $ Resources/folder.png��64   $ MatrixA8��6W   $��16��6k   $��0,2,5,2q�6z   6�   5]      Q� 5^      $��3��6'   ��5_        5`   "   $ Resources/folder.png��64   $ 	Text-Left��6W   $��16��6l   $��0,2,5,2q�6{   6�   5b      Q� 5c      $��4��6'   ��5d        5e   "   $ Resources/folder.png��64   $ 
Text-Right��6W   $��16��6m   $��0,2,5,2q�6|   6�   5g      Q� 5h      $��5��6'   ��5i        5j   "   $ Resources/folder.png��64   $ RVA200��6W   $��16��6i   $��0,2,5,2q�6x   6�   5l      Q� 5m      $��6��6'   ��5n        5o   "   $ Resources/folder.png��64   $ RIO200��6W   $��16��6i   $��0,2,5,2q�6x   6�   5q      Q� 5r      $��7��6'   ��5s        5t   "   $ Resources/folder.png��64   $ RVC1000��6W   $��16��6j   $��0,2,5,2q�6y   6�   5v      Q� 5w      $��8��6'   ��5x        5y   "   $ Resources/folder.png��64   $ RPM200��6W   $��16��6i   $��0,2,5,2q�6x   6�   5{      5|      5~        Lib.Controls.DrawCanvas  5�      -   5          Name$ dwpanel��5�   !   $��1��63   
�������6q   5�      5�      �� 5�   
   -   5        $��botomStatusBar��5�      $��2��6*   
������ $��5q�6Q   .@�5�      �� $��Center=�6   .��6?   Connect status:6P   �� 5�      -   5        $��itemConStatus��5�      $��Center=�61   .��6S   Disconnected6a   �� 5�      $��30,0,0,0q�6   $��Center=�6.   .��6P   	Local IP:6[   �� 5�      -   5        $��cLocalIP��5�      $��Center=�6,   $��127.0.1��6N   6b   �� 5�      $��30,0,0,0q�6   $��Center=�6.   .��6P   Remoted IP:6]   �� 5�      -   5        $��edRemotDevIP��5�      $��Center=�60   $��	127.0.0.1��6R   6i   �� 5�      $��30,0,0,0q�6   $��Center=�6.   .��6P   	Device:6Y   �� 5�      -   5        $��cdevName��5�      $��Center=�6,   6Q   5�      5�      5�      