   M S B A M L   `   `   `  ����  MatrixSystemEditor&     MatrixSystemEditor.ChangeLockPWD   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        ���#EFEFEF  � WindowStartupLocation$  CenterScreen=�5   7    � Title$ Password Setting��5   	   $	��200��6"   $	��365��6/    � 
ResizeMode$ NoResize=�6;   .��5      �  � RowDefinitions 5   
   �� 5	      $
��Auto �6   6,   �� 5
      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $��15 �6   6*   �� 5      $
��Auto �6   6,   �� 5      6   .+�5   
   ]� $��0��6   $��1,8,8,0q�6"   .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��Current Password: ��6   $	��120��66    �� TextAlignment$ Right=�6F   $��Center=�6\   6y   G� 5      -   5        $��edCpwd��5      $	��150��6,   $	��160��6;    G� 	MaxLength$ 4��6J   6s   5      5      ]� 5   
   $��1��6   $��1,8,8,0q�6"   .+�5      �� $ 
Horizontal=�6   .+�5      �� $��New Password: ��6   $	��120��63   $��Center=�6B   $ Right=�6^   6v   G� 5      -   5        $��edNpwd��5      $	��150��6,   $	��160��6<   $ 4��6K   6t   5      5      ]� 5   
   $��2��6   $��1,8,8,0q�6!   .+�5      �� $ 
Horizontal=�6   .+�5      �� $��Confirm Password: ��6   $	��120��67   $ Right=�6F   $��Center=�6\   6y   G� 5      -   5        $��edCfpwd��5      $	��150��6-   $	��160��6=   $ 4��6L   6v   5       5!      ]� 5"   
   $��4��6   $��Center=�6!   .+�5#      �� $ 
Horizontal=�6   .+�5$      �� -   5        $��okBtn��5$   T   $��OK��6   $��5q�6&   $��5q�62   $��60��6=   $��30��6H   6x   �� 5%      -   5        $��	cancelBtn��5%   X   $��Cancel��6   $��5q�6*   $��5q�66   $��60��6A   $��30��6L   6�   5&      5'      5)      5*      