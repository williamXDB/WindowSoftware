   M S B A M L   `   `   `  ����R  NLib.Controls.MyControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullL;clr-namespace:Lib.Controls;assembly=Lib.Controls.MyControlsLib.Controls  5       MatrixSystemEditor+   %MatrixSystemEditor.pageTab.DuckerPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Joc;clr-namespace:Lib.Controls;assembly=Lib.Controls.MyControls     5        N� Title$  
DuckerPage��5	      .��5      � $	��500��6    � ShowGridLines .  ��5   
   �� 5       �� ImageSource$: 4/MatrixSystemEditor;component/Resources/mid_bimg.png��6   6^    � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   '   !Lib.Controls.CDuckerSourceControl  -   5          Name$ 	duckerCtl��5   "   $��0��65   �� 5   
   -   5         ��Name$ right_duckerControlBox��5      $��1��62   $��
48,10,5,15q�5   	    �� Padding$ 10,30,10,10q�6   $	��330��63   
������ ��5      �� 5      $: 4/MatrixSystemEditor;component/Resources/mid_bimg.png��6   6b   ._�5      �� .+�5      �� $��Ducking Controller��6   
�������$��19,0,0,0q�6I   6]   � 5      -   5         �Name$ gridDuckerControler��5      $��0,25,1,1q�64    .   � RowDefinitions 5      �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   �� 5       �� 5!      $
��Auto �6(   �� 5"      $
��Auto �6(    5%      �� 5&      �� 5'      �� 5(      �� 5)      �� 5*      �� 5+      �� 5,      .+�5.      �� $��Center=�6    $��	Threshold��6=   $��1��6N   
�������6^   �� 5/      $��1��6    $��Center=�6-   $��5,15,5,5q�6J   $��1��6]   
��������� 6�   $
�(dB)��6�   6�   ]� 50      $��2��6    $��1��6-   ��51      �� 52      $A ;/MatrixSystemEditor;component/Resources/vbs_sliderPanel.png��6)   6u   .+�54         Lib.Controls.CSlider  -   5        	  Name$	 	dslider_0��54   P   $��Center=�6&   $��1q�6C   
   iTag$
 0��6d      thumb$= 7/MatrixSystemEditor;component/Resources/Image/thumb.png��55      $��30��6L   $	��260��6W      Maximum$ 80��56         Posvalue$ 0��6   57      �� 59      -   5         ��Name$ 
edDucker_0��59   �   $��3��6   $��60��6+    �� 
IsReadOnly . $��-60dB��6L    �� TextAlignment$ Center=�6Y   $��1��6q   6�   �� 5<      $��2��6    $��Center=�60   $
��Dept��6M   
�������6Y   �� 5>      $��1��6    $��Center=�6-   $��2��6J   $��5,15,5,5q�6Z   
��������� 6�   $
�(dB)��6�   6�   ]� 5?      $��2��6    $��2��6-   ��5@      �� 5A      $A ;/MatrixSystemEditor;component/Resources/vbs_sliderPanel.png��6)   6u   .+�5C        -   5        $	 	dslider_1��5C   [   $��2��6%   $��Center=�62   $��1q�6O   $
 1��6o   $= 7/MatrixSystemEditor;component/Resources/Image/thumb.png��5D      $��30��6L   $	��260��6W   $ 60��5E      $ 0��6   5F      �� 5G      -   5        $ 
edDucker_1��5G   �   $��3��6   $��2��6+   $��60��6;    . $	��0dB��6]   $ Center=�6h   6�   �� 5J      $��3��6    $��Center=�60   $��Attack��6M   
�������6\   �� 5K      $��1��6    $��Center=�6-   $��3��6J   $��5,15,5,5q�6Z   
��������� 6�   $
�(mS)��6�   6�   ]� 5M      $��2��6    $��3��6-   ��5N      �� 5O      $A ;/MatrixSystemEditor;component/Resources/vbs_sliderPanel.png��6)   6u   .+�5Q        -   5        $	 	dslider_2��5R   g   $��2��5Q   %   $��Center=�62   $��1q�6O   $= 7/MatrixSystemEditor;component/Resources/Image/thumb.png��5R      $��30��6L   $	��260��6W   $
 2��6{   $ 24��5S      $ 0��6   5T      �� 5U      -	   5        $ 
edDucker_2��5U   �   $��3��6   $��3��6+   $��60��6;    . $
��10mS��6]   $ Center=�6i   6�   �� 5W      $��4��6    $��Center=�60   $��Release��6M   
�������6\   �� 5X      $��1��6    $��Center=�6-   $��4��6J   $��5,15,5,5q�6Z   
��������� 6�   $
�(mS)��6�   6�   ]� 5Z      $��2��6    $��4��6-   ��5[      �� 5\      $A ;/MatrixSystemEditor;component/Resources/vbs_sliderPanel.png��6)   6u   .+�5^        -
   5        $	 	dslider_4��5_   g   $��2��5^   %   $��Center=�62   $��1q�6O   $��4��6[   $= 7/MatrixSystemEditor;component/Resources/Image/thumb.png��5_      $��30��6L   $	��260��6W   $
 4��6{   $ 24��5`      $ 0��6   5a      �� 5d      -   5        $ 
edDucker_4��5d   �   $��3��6   $��4��6+   $��60��6<    . $
��10mS��6]   $ Center=�6i   6�   ]� 5e      $��4��6    $��7��6-   $��Center=�6A   $��8q�6^   .+�5f      ��  �� Orientation$ 
Horizontal=�6%   .+�5g         Lib.Controls.CSwitcher  -   5          Name$ duckerBypas��5g   7   $��5q�6+      unActiveBrush �	#FF454545   activeBrush
 �  ��$��60��6y   $��30��6�   $��Bypass��5h   '   �� 5i      -   5         ��Name$ 
duckerFlat��5i   T   $��5q�6%   $��10q�60   $��60��6=   $��30��6H   $��Default��6�   5j      5k      5m      5n      5o      5q      5r      