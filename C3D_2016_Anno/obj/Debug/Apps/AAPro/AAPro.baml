   M S B A M L   `   `   `  ����U  QMahApps.Metro, Version=1.4.0.42, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	   J FC3D_2016_Anno, Version=4.47.1.16, Culture=neutral, PublicKeyToken=null7 clr-namespace:C3D_2016_Anno.AppsC3D_2016_Anno.Apps 5   	      C3D_2016_Anno.Apps.AAPro   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   2local clr-namespace:C3D_2016_Anno.Apps     5   	   $	��400��5	   	   $	��371��6   $	��385��6"     � 	Resources  5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $I Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?�6%   �� 5      $F @pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml?�6%   �� 5      $G Apack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml?�6%   �� 5      $O Ipack://application:,,,/MahApps.Metro;component/Styles/Accents/cobalt.xaml?�6%   �� 5      $R Lpack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml?�6%   �� 5      $3 -/C3D_2016_Anno;component/Resources/Icons.xaml?�6%   %    5      .��5      � .+�5   
   �  � ColumnDefinitions 5      �� 5      �� 5      $	��126 �6#    � RowDefinitions 5      �� 5      $��64 �6    �� 5      $��83 �6    �� 5      .+�5!      �� -   5         ��Name$ tBox_stylemapperFile��5!      $��47��64   $��	10,16,7,0q�6@    �� TextWrapping$
 Wrap=�6S   $	��Top=�6g   *   $MahApps.Metro.Controls.TextBoxHelper   UseFloatingWatermark .    	Watermark$ Select Style Mapper File��6�   �� 5"      -   5        	 ��Name$#	 btn_browse_styleStructureFile��5"   B   $��	0,27,90,0q�6   
������ 
 � ToolTip$
 Browse file��6i   $��1��6�   $��Right=�6�   $��35��6�   $��36��6�   $	��Top=�6�   .��5#      �� $��22��6   $��19��5$      ���	#FF3373F2 J� OpacityMask 5&      !� 5'       j� Stretch$
 Fill=�6&      appbar_folder !� Visual# [  5(   !   5*      5+      �� 5,      -   5        $!	 btn_save_styleStructureFile��5,   B   $��	0,27,50,0q�6   
������ $��35��6g   $��Right=�6r   $
 	Save file��6�   $��1��6�   $��36��6�   $	��Top=�6�   .��5-      �� $��22��6   $��19��5.      ���	#FF3373F2 50      !� 51      $
 Fill=�6&     appbar_save# [ 52   !   54      55      �� 57      -   5         ��Name$ groupBox��57      $��Learn Tools��6)   $��	10,10,0,0q�6>   $��1��6Q   $	��Top=�6^   $��72��6v   $��2��6�   $
��Left=�6�   $	��366��6�   .��58      � .+�59      �� -   5        $	 	btn_learn��59      $��LEARN SELECTED STYLE��60   $
��Left=�6O   $��0,3,0,0q�6j   $	��Top=�6{   $	��160��6�   $
 Learn selected style��6�   5:      5;      �� 5<      -   5        $ 	groupBox1��5<      $��Style Details��6*   $��10q�6A   $��2��6M   $��2��6Z   .��5=      � .+�5>      �� -   5         ��Name$# listView_styleComponentMapper��5>       �� SelectionMode$ Single=�6F    � ContextMenu 5?      �� 5@      .@�5A   "   m� -   5        $��Zoom To��5A   +   6<   m� 5B   "   -	   5        $��Select��5B   +   6;   5C       �� View 5E      �� 5F      .P�5G   "   ��  �� DisplayMemberBinding 61   �� *	styleName+��5H   &   �� 5I   *   -
   5         ��Name$ styleNameHeader��5I   ?   
 � Tag$ 	styleName��6X   .��6�   
Style Name6�   5K   #   �� 5L   "    61   �� *	labelType+��5M   &   �� 5N   *   -   5        $$ NoteType_styleAssingmentHeader��5N   ?   $ 	labelType��6g   .��6�   Style Assignment6�   5P   #   �� 5Q   "   $��Learn Status��61    �� CellTemplate 5R   &   �� 5S   *   .R�5T   .   �� $��learnStatusImage��64   $O��Ipack://application:,,,/C3D_2016_Anno;component/Resources/Icons/Sucess.png��6L   $��10��6�   $��10��6�    �� Triggers 5U   .   �� 5V   2    �� Value$ False��6^    �� Binding 6>   �� *learnStatus+.a�5W   6   ��  �� 
TargetName$ learnStatusImage��6=    �� Property � �� �� Value$M Gpack://application:,,,/C3D_2016_Anno;component/Resources/Icons/fail.png��6n   5X   3   5Z   +   5\   #   5]      5_      5`      5a      �� 5c      -   5        $#	 btn_saveAS_styleStructureFile��5c   B   $��	0,27,10,0q�6   
������ $��35��6i   $��Right=�6t   $
 Save as��6�   $��1��6�   $��36��6�   $	��Top=�6�   .��5d      �� $��22��6   $��19��5e      ���	#FFBFC71A 5g      !� 5h      $
 Fill=�6&   # [ 5i      5k      5l      �� 5m      -   5        $	 btn_clearAll��5m   B   $��	0,12,46,0q�6   
������ $��22��6X   $��Right=�6c   $
 	Clear all��6   $��1��6�   $��2��6�   
�������$��25��6�   $	��Top=�6�   .��5n      �� $��9��6   $��12��5o      
������� 5q      !� 5r      $
 Fill=�6&     appbar_reset# [ 5s      5u      5v      �� 5w      -   5        $	 btn_delete_style_item��5w   B   $��	0,12,19,0q�6   
������ $��22��6a   $��Right=�6l   $
 Delete selected item��6�   $��1��6�   $��2��6�   
�������$��25��6�   $	��Top=�6  .��5x      �� $��9��6   $��12��5y      
������� 5{      !� 5|      $
 Fill=�6&     appbar_delete# [ 5}   !   5      5�      5�      � 5�   
   -   5         �Name$ grid_addStyle��5�      $��0��6'   $��0��68   $��
10,15,5,10q�6E   ���	#FFEEE8E8$y�Hidden=�6p   .+�5�      �� $��3q�6    �� CornerRadius$ 4��6)   ��5�      �� 5�       �� Color$ #000000��6&     �� Opacity$  .7Q�66   ._�5�      � $��3,46,3,0q�6   $	��162��6)   $	��Top=�66    5�      �� 5�      $	��231 �6(   .+�5�      �� $��	10,0,0,71q�6   $��	Add Style��62   $	��220��6E   .��5�      � $��	0,85,0,-7q�6    5�      �� 5�   "   $��39 �60   �� 5�   "   .+�5�      �� -   5        $ tBox_styleName��5�   &   $��	0,-87,0,0q�6>   $ 
Style Name��6Q   $��34��6   $��40��6�   $	��Top=�6�   $
 Enter style name��6�    . 6  �� 5�      -   5        ! ��Name$! cBox_styleType��5�   '   $��
0,-42,54,0q�6?   $ 
Style Type��6S   $��40��6�   $��40��6�   $	��Top=�6�   $
 Enter style type��6�   " �� 
IsEditable" .  . 5�      �� 5�      -   5        $	 btn_del_styletype_item��5�   S   $��
0,-27,27,0q�6%   
������ $��22��6s   $��Right=�6~   $
 Delete note type��6�   ���	#FF306EED$��25��6�   $	��Top=�6�   .��5�   "   �� $��9��6,   $��12��66   ���	#FF306EED 5�   &   !� 5�   *   $
 Fill=�66   # [ 5�   !   5�   #   5�      �� 5�      -   5        $	 btn_add_styletype_item��5�   R   $��	0,-27,0,0q�6%   
������ $��22��6r   $��Right=�6}   $
 Add note type��6�   ���	#FF306EED$��25��6�   $	��Top=�6�   .��5�   "   �� $��9��6,   $��12��66   ���	#FF306EED 5�   &   !� 5�   *   $
 Fill=�66     
appbar_add# [ 5�   !   5�   #   5�      �� 5�      -   5        # ��Name$# lbl_addStyleError��5�   $   
���  ��$��
0,10,202,4q�6P   $ J� RenderTransformOrigin$$ 0.532,0.543&�6d   �� 5�      -   5        $	 btn_addStyle��5�   %   $��0��6;   $��Add Learned Style��6K   $��0,0,76,4q�6g   $��Right=�6�   $	��121��6�   $��25��6�   $��Bottom=�6�   % �� 	IsDefault% . $
 Add to list��6�   �� 5�      -   5        $	 
btn_cancel��5�   %   $��0��69   $��Cancel��6I   $��0,0,0,4.333q�6Z   $��Right=�6�   $��71��6�   $��25��6�   $��Bottom=�6�   & �� IsCancel& . $
 Discard��6�   5�      5�      �� 5�      -   5        $	 btn_previousLabel��5�   I   $��0,2,33,0q�6   
������ $��22��6d   $��Right=�6o   $
 Previous��6�   
�������$��25��6�   $	��Top=�6�   .��5�      �� $��9��6$   $��12��6.   
������� 5�      !� 5�   "   $
 Fill=�6.     appbar_arrow_left# [ 5�   -   5�      5�      �� 5�      -   5        $	 btn_nextLabel��5�   H   $��0,2,6,0q�6   
������ $��22��6_   $��Right=�6j   $

 Next��6�   
�������$��25��6�   $	��Top=�6�   .��5�      �� $��9��6$   $��12��6.   
������� 5�      !� 5�   "   $
 Fill=�6.     appbar_arrow_right# [ 5�   -   5�      5�      5�      5�      5�      5�      5�      