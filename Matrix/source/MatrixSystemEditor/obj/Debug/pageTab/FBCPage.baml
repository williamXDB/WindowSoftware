   M S B A M L   `   `   `  ����R  NLib.Controls.MyControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullL;clr-namespace:Lib.Controls;assembly=Lib.Controls.MyControlsLib.Controls  5      bFclr-namespace:Lib.Controls.FFTControl;assembly=Lib.Controls.MyControlsLib.Controls.FFTControl  5       MatrixSystemEditor(   "MatrixSystemEditor.pageTab.FBCPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Joc;clr-namespace:Lib.Controls;assembly=Lib.Controls.MyControls     5      UomFclr-namespace:Lib.Controls.FFTControl;assembly=Lib.Controls.MyControls     5        N� Title$  FBCPage��5
      .��5      � ��5   
   �� 5       �� ImageSource$: 4/MatrixSystemEditor;component/Resources/mid_bimg.png��6   6^    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5       � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      .+�5   
   %   Lib.Controls.FBCSwitcherControl  -   5          Name$ FbcSwitcher��5       $��0��66   $	��180��6F   $
��1048��6W   $
��Left=�6g   �� 5   
   $��1��6   $��0��6    �� Padding$ 5q�60   $	��998��5   '   $
��Left=�63   $��5q�6N   ._�5      �  5      �� 5      6&   �� 5      6&   �� 5       6&   �� 5!      6&    5#      �� 5$      $
��Auto �6'   67   �� 5%      6*   .+�5'      �� $ 1,5,2,5q�6   $��0,0,5,0q�6-   
������ ��5(      �� 5)      $? 9/MatrixSystemEditor;component/Resources/rightDown_img.png��6%   ._�5+      ��  �� Orientation$ 
Horizontal=�6!   .+�5,      �� $��FBC Setting��6$   $��15,5,5,5q�67   
�������6I   � 5-      $��20,15,15,10q�6    5.      �� 5/   "   65   �� 50   "   65   .+�52      �� $��FBC Mode��6(   $��Center=�68   $��Center=�6S   
�������6�   !   Lib.Controls.SpinnerControl  53      -   5        $��
fbcModSpin��55   P   $��1��53   0      UpeqType$ 	OtherType=�6@      Maximum$ 1��6U   	   valueTxt$	 Speech��6a   
   iTag$
 4��54   P   $��2,5,5,5q�6Y   55   d   56      � 57      $��5,0,5,0q�6    58      �� 59   "   66   �� 5:   "   65   .+�5<      �� $��Fiter Release��6(   $��Center=�6=   $��Center=�6X   
�������6�     5=      -   5        $��fbcRelesSpin��5?   P   $��1��5=   0   $ 	OtherType=�6A   $ 2��6V   $
	 Fast��6b   $
 5��5>   P   $��2,5,5,5q�6Y   5?   f   5@      5A      5B      �� 5D      $��1��6   $ 1,1,2,5q�6'   $��0,10,5,0q�69   
������ ��5E      �� 5F      $? 9/MatrixSystemEditor;component/Resources/rightDown_img.png��6%   ._�5H      �� .+�5I      �� $��	FBC Steup��6$   $��5,5,0,1q�65   
�������6]   � 5J      -   5        $��fbcSetupGrid��5J      $��0,15,0,0q�63    � ShowGridLines .   5K      �� 5L   "   62   �� 5M   "   62   �� 5N   "   62    5P      �� 5Q   "   $
��Auto �63   6B   �� 5R   "   $	��318��63   6D   .+�5T      �� -   5         ��Name$ fbcSetLabel��5U      $	��0dB��5T   &   $��Center=�61   $
��Left=�6L   $y�Visible=�5U   "    �� 
IsReadOnly . $��10,2,0,0q�5V   6   $��40��5W      $��40��6   ]� 5Y      $��1��6)   .+�5Z   "      Lib.Controls.CSlider  -   5          Name$ 	fbcSlider��5]   l      isVerticalSlider .  $��40��5Z   G   $	��256��6S   $��0,0,15,0q�6`   $y�Visible=�5[         isSupportMark .    	LineColor$ #303030��6:      	lineWidth$ 5Q�6N      	lineStyle .    MarkStep$ 6Q�5\      $
��Left=�6      thumb$@ :/MatrixSystemEditor;component/Resources/Image/fader_hr.png��5]         Maximum$ 80��6R      Posvalue$ 0��6_   6l   5^      �� 5`      -   5         ��Name$ fbcMarkPanel��5`   l   $ 
Horizontal=�6*   $��10,1,1,1q�6D   $y�Visible=�6W   $��1��5a   :   $��1��6J   $	��248��6X   $
��Left=�6d   .+�5c   "   !   Lib.Controls.CFBC_horMarker  $	��208��64   $��15��6@   6N   5d      ]� 5f      $��2��6(   $��0��66   $��2��6G   $��	25,10,5,5q�6[   .+�5g   "   �� $ 
Horizontal=�6-   .+�5i   &      Lib.Controls.CSwitcher  -   5          Name$ fbcAutoSetBtn��5i   P   $��5,5,2,5q�63   $��1q�6D      unActiveBrush �#454545$��65��5k   ?   $��72��6M   $��35��6[   .��5l   *   ��  �� TextAlignment$ Center=�64   $��11(�6L   Static�� 6a   Filters Setup5m   +   5n   '     5q   &   -	   5        $ fbcClearDynamicFilterBtn��5q   Q   $��2,5,2,5q�63   $��1q�6D    �#454545   activeBrush
 �  ��$��72��5s   !   $��85��6/   .��5t   *   �� $ Center=�64   $��11(�6K   Clear�� 6_   Dynamic Fiters5u   +   5v   '     5x   &   -
   5        $ fbcClearAllFilterBtn��5z      $��1,5,1,5q�5x   3   $��1q�6D    �#454545
 �  ��$��65��5y   "   $��80��60   .��5}   *   �� $ Center=�64   $��11(�6L   Clear�� 6`   All Filters5~   +   5   '     5�   &   -   5        $ fbcBypasBtn��5�   P   $��2,5,2,5q�63   $��1q�6D   
 �  ��$��65��5�   #   $��65��61   $��11(�6?    �#454545$��Bypass��5�   ;   5�   #   5�      5�      5�      5�      �� 5�      ���	#FF1E1E1E$��1��60   $��3��6A   $	��588��6R   $��5,0,1,0q�6^   ._�5�      (   "Lib.Controls.FFTControl.FBCFFTDraw  -   5          Name$ 
fbcFFTDraw��5�   F   $��	15,1,5,15q�6$   $	��200��68      Lib.Controls.CommControl   	BackBrush$ 	#FF1E1E1E��5�   8   $	��538��6O      
masterGain$ 0Q�5�      5�          Lib.Controls.FBCStatusLeds  5�      -   5           Name$  	FBCledCTL��5�   6   $��	50,8,50,1q�6#   $��3��6I   $��2��6V   5�      5�      5�      5�      