   M S B A M L   `   `   `  ����U  QMahApps.Metro, Version=1.4.0.42, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	   J FC3D_2016_Anno, Version=4.47.1.16, Culture=neutral, PublicKeyToken=null7 clr-namespace:C3D_2016_Anno.AppsC3D_2016_Anno.Apps 5   	      C3D_2016_Anno.Apps.Editor   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:C3D_2016_Anno.Apps     5   	     � Title$  Edit Manager��5	   	   $	��850��6   $
��1500��6+   $
��1500��68   $
��1500��6H   $	��850��6X   $	��850��6h    � 	Resources 5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $I Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?�6%   �� 5      $F @pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml?�6%   �� 5      $G Apack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml?�6%   �� 5      $O Ipack://application:,,,/MahApps.Metro;component/Styles/Accents/cobalt.xaml?�6%   �� 5      $R Lpack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml?�6%   �� 5      $3 -/C3D_2016_Anno;component/Resources/Icons.xaml?�6%   %    5      .��5      � 
�������.+�5   
   �� -   5         ��Name$ 
tabControl��5      .@�5      �� $��Definition Files��6   .��5      � ���	#FFE5E5E5 � ColumnDefinitions 5      �� 5      $
��154* �6+   �� 5      $
��351* �6+    � RowDefinitions 5      �� 5      .+�5!      �� $��
5,10,10,10q�6   $��Files��63   .��5"      � .+�5%      �� -   5         ��Name$ textBox��5%   &   $��C:\AANNO\DefinitionFiles\��67   $��47��6X   $��
0,10,170,0q�6d    �� TextWrapping$
 Wrap=�6x   $	��Top=�6�   *   $MahApps.Metro.Controls.TextBoxHelper	   	Watermark$	 Location��6�   
   UseFloatingWatermark
 . 6�   �� 5&      -   5         ��Name$ btn_fetchtamplateFile��5&   R   $��	0,22,90,0q�6%   
������ $��35��6q   $	��Top=�6}   $��35��6�   $��Right=�6�    � ToolTip$ 
Scan Files��6�   .��5'   "   �� $��22��6,   $��19��5(      ���	#FF3373F2 J� OpacityMask 5*   &   !� 5+   *    j� Stretch$
 Fill=�66      appbar_refresh !� Visual# [  5,      5.   #   5/      �� 50      -   5        $ btn_browse_templateFile��50   R   $��	0,21,50,0q�6%   
������ $��36��6s   $	��Top=�6   $��35��6�   $��Right=�6�   $# Select a folder to scan files��6�   .��51   "   �� $��22��6,   $��19��52      ���	#FF3373F2 54   &   !� 55   *   $
 Fill=�66     appbar_folder# [ 56   !   58   #   59      �� 5:      $��	0,62,4,10q�6&   .@�5;   "   �� $4��.C:\AANNO\DefinitionFiles\ProjectA-Template.xml��6.    �� 
IsSelected . 6g   �� 5<   "   $4��.C:\AANNO\DefinitionFiles\ProjectB-Template.xml��6.   5=      �� 5>      -   5        $" btn_browse_templateFile_Copy��5>   R   $��	0,21,10,0q�6%   
������ $��36��6x   $	��Top=�6�   $��35��6�   $��Right=�6�   $ Zip selected files��6�   .��5?   "   �� $��22��6,   $��19��5@   %   ���	#FF3373F2 5B   &   !� 5C   *   $
 Fill=�66     appbar_download# [ 5D   -   5F   #   5G      �� 5H      -   5        $  btn_fetchtamplateFile_Copy��5H   S   $��
0,22,130,0q�6%   
������ $��35��6w   $	��Top=�6�   $��35��6�   $��Right=�6�   $ 	add files��6�   .��5I   "   �� $��22��6,   $��19��5J   %   ���	#FF3373F2 5L   &   !� 5M   *   $
 Fill=�66     
appbar_add# [ 5N   -   5P   #   5Q      5R      5S      �� 5T      -   5         ��Name$ groupBox��5T      $��1��61   $��Editor��6A   $��5,10,6,5q�6Q   .��5U      �  5V      �� 5W   "   .+�5Y      �� -   5        $ 	groupBox1��5Y   '   $��
Note Types��6:   $
��Left=�6N   $��0,0,0,10q�6i   $	��532��6{   .��5Z   "   � .+�5[   &   �� -	   5        $ textBox1��5[   .   $
��Left=�6@   $��47��6[   $��0,10,0,0q�6g   $
 Wrap=�6y   $	��Top=�6�   $	��346��6�   $	 Add Note Type��6�   
 . 6�   �� 5\   &   -
   5        $ btn_fetchtamplateFile1��5\   Z   $��	0,22,51,0q�6-   
������ $��35��6z   $	��Top=�6�   $��36��6�   $��Right=�6�   $ Add Note Type��6�   .��5]   *   �� $��22��64   $��19��5^   %   ���	#FF3373F2 5`   .   !� 5a   2   $
 Fill=�6>   # [ 5b   -   5d   +   5e   '   �� 5f   &   -   5         ��Name$ listBox��5f   .   $��0,62,0,0q�6?   .@�5g   *   �� $��
WATERNOTES��66   �� 5h   *   $��IRRIGATIONNOTES��66    . 6P   �� 5i   *   $��PAVINGNOTES��66   �� 5j   *   $��GRADINGNOTES��66   5k   '   �� 5l   &   -   5        $! btn_fetchtamplateFile1_Copy��5l   Z   $��	0,22,10,0q�6-   
������ $��35��6   $	��Top=�6�   $��36��6�   $��Right=�6�   $ Delete Note Type��6�   .��5m   *   �� $��22��64   $��19��5n   -   ���	#FF3373F2 5p   .   !� 5q   2   $
 Fill=�6>     appbar_delete# [ 5r   5   5t   +   5u   '   �� 5v   &   -   5        $" btn_fetchtamplateFile1_Copy2��5v   Z   $��	0,22,92,0q�6-   
������ $��35��6�   $	��Top=�6�   $��36��6�   $��Right=�6�   $ Import to CSV��6�   .��5w   *   �� $��22��64   $��19��5x   -   ���	#FF3373F2 5z   .   !� 5{   2   $
 Fill=�6>   # [ 5|   5   5~   +   5   '   �� 5�   &   -   5        $" btn_fetchtamplateFile1_Copy3��5�   [   $��
0,22,133,0q�6-   
������ $��35��6�   $	��Top=�6�   $��36��6�   $��Right=�6�   $ Export to CSV��6�   .��5�   *   �� $��22��64   $��19��5�   -   ���	#FF3373F2 5�   .   !� 5�   2   $
 Fill=�6>     appbar_upload# [ 5�   5   5�   +   5�   '   5�   #   5�      �� 5�      -   5        $ groupBox1_Copy��5�   '   $��
Note Types��6?   $��537,0,10,10q�6S   .��5�   "   � .+�5�   &   &   System.Windows.Controls.DataGrid  -   5          Name$ dataGrid��5�   /      Columns 5�   *   4  .System.Windows.Controls.DataGridCheckBoxColumn  5�   .   ,  &System.Windows.Controls.DataGridColumn   Header$ Note Number��6{   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6E   Z�    ClipboardContentBinding 6X   Z� 6{     5�   .   $
 Note��6{    6E   Z�  6X   Z� 6{   5�   '   5�   #   5�      5�      5�      5�      5�      �� 5�      $��
Note Files��6   .��5�      � ���	#FFE5E5E5 5�      �� 5�      $
��154* �6+   �� 5�      $
��351* �6+    5�      �� 5�      .+�5�      �� $��
5,10,10,10q�6   $��Files��63   .��5�      � .+�5�      �� -   5        $ textBox5��5�   &   $��C:\AANNO\NoteFiles\��68   $��47��6S   $��
0,10,170,0q�6_   $
 Wrap=�6s   $	��Top=�6�   $	 Location��6�   
 . 6�   �� 5�      -   5        $ btn_fetchtamplateFile5��5�   R   $��	0,22,90,0q�6%   
������ $��35��6r   $	��Top=�6~   $��35��6�   $��Right=�6�   $ 
Scan Files��6�   .��5�   "   �� $��22��6,   $��19��5�      ���	#FF3373F2 5�   &   !� 5�   *   $
 Fill=�66   # [  5�      5�   #   5�      �� 5�      -   5        $ btn_browse_templateFile4��5�   R   $��	0,21,50,0q�6%   
������ $��36��6t   $	��Top=�6�   $��35��6�   $��Right=�6�   $# Select a folder to scan files��6�   .��5�   "   �� $��22��6,   $��19��5�      ���	#FF3373F2 5�   &   !� 5�   *   $
 Fill=�66   # [ 5�   !   5�   #   5�      �� 5�      $��	0,62,4,10q�6&   .@�5�   "   �� $&�� C:\AANNO\NoteFiles\Common.Mapper��6.    . 6Y   �� 5�   "   $(��"C:\AANNO\NoteFiles\ProjectB.Mapper��6.   5�      �� 5�      -   5        $# btn_browse_templateFile_Copy3��5�   R   $��	0,21,10,0q�6%   
������ $��36��6y   $	��Top=�6�   $��35��6�   $��Right=�6�   $ Zip selected files��6�   .��5�   "   �� $��22��6,   $��19��5�   %   ���	#FF3373F2 5�   &   !� 5�   *   $
 Fill=�66   # [ 5�   -   5�   #   5�      �� 5�      -   5        $! btn_fetchtamplateFile_Copy2��5�   S   $��
0,22,130,0q�6%   
������ $��35��6x   $	��Top=�6�   $��35��6�   $��Right=�6�   $ 	add files��6�   .��5�   "   �� $��22��6,   $��19��5�   %   ���	#FF3373F2 5�   &   !� 5�   *   $
 Fill=�66   # [ 5�   -   5�   #   5�      5�      5�      �� 5�      -   5        $ 
groupBox11��5�      $��1��63   $��Editor��6C   $��0,10,6,5q�6S   .��5�      �  5�      �� 5�   "   .+�5�      �� -   5        $ groupBox111��5�   '   $��
Note Types��6<   $
��Left=�6P   $��0,0,0,10q�6k   $	��532��6}   .��5�   "   � .+�5�   &   �� -   5        $ 	textBox11��5�   .   $
��Left=�6A   $��47��6\   $��0,10,0,0q�6h   $
 Wrap=�6z   $	��Top=�6�   $	��346��6�   $	 Add Note Type��6�   
 . 6�   �� 5�   &   -   5        $ btn_fetchtamplateFile11��5�   Z   $��	0,22,51,0q�6-   
������ $��35��6{   $	��Top=�6�   $��36��6�   $��Right=�6�   $ Add Note Type��6�   .��5�   *   �� $��22��64   $��19��5�   %   ���	#FF3373F2 5�   .   !� 5�   2   $
 Fill=�6>   # [ 5�   -   5�   +   5�   '   �� 5�   &   -   5        $ 	listBox12��5�   .   $��0,62,0,0q�6A   .@�5�   *   �� $��
WATERNOTES��66   �� 5�   *   $��IRRIGATIONNOTES��66    . 6P   �� 5�   *   $��PAVINGNOTES��66   �� 5�   *   $��GRADINGNOTES��66   5�   '   �� 5�   &   -   5        $# btn_fetchtamplateFile1_Copy13��5�   Z   $��	0,22,10,0q�6-   
������ $��35��6�   $	��Top=�6�   $��36��6�   $��Right=�6�   $ Delete Note Type��6�   .��5�   *   �� $��22��64   $��19��5�   -   ���	#FF3373F2 5�   .   !� 5�   2   $
 Fill=�6>   # [ 5�   5   5�   +   5�   '   �� 5�   &   -   5        $# btn_fetchtamplateFile1_Copy21��5�   Z   $��	0,22,92,0q�6-   
������ $��35��6�   $	��Top=�6�   $��36��6�   $��Right=�6�   $ Import to CSV��6�   .��5�   *   �� $��22��64   $��19��5�   -   ���	#FF3373F2 5�   .   !� 5   2   $
 Fill=�6>   # [ 5  5   5  +   5  '   �� 5  &   -   5        $# btn_fetchtamplateFile1_Copy31��5  [   $��
0,22,133,0q�6-   
������ $��35��6�   $	��Top=�6�   $��36��6�   $��Right=�6�   $ Export to CSV��6�   .��5  *   �� $��22��64   $��19��5  -   ���	#FF3373F2 5	  .   !� 5
  2   $
 Fill=�6>   # [ 5  5   5  +   5  '   5  #   5     �� 5     -   5        $ groupBox1_Copy1��5  '   $��	Note Keys��6@   $��537,0,10,10q�6S   .��5  "   � .+�5  &   �� -   5        $ 	textBox21��5  .   $��47��6A   $��
0,10,183,0q�6M   $
 Wrap=�6a   $	��Top=�6u   $	 Add Note Keys��6�   
 . 6�   �� 5  &   -    5        $ btn_fetchtamplateFile12��5  Z   $��	0,22,61,0q�6-   
������ $��35��6{   $	��Top=�6�   $��35��6�   $��Right=�6�   $ Add Note Type��6�   .��5  *   �� $��22��64   $��19��5  -   ���	#FF3373F2 5  .   !� 5  2   $
 Fill=�6>   # [ 5  5   5  +   5  '   �� 5  &   -!   5        $$ btn_fetchtamplateFile1_Copy241��5  [   $��
0,22,101,0q�6-   
������ $��35��6�   $	��Top=�6�   $��36��6�   $��Right=�6�   $ Import to CSV��6�   .��5   *   �� $��22��64   $��19��5!  -   ���	#FF3373F2 5#  .   !� 5$  2   $
 Fill=�6>   # [ 5%  5   5'  +   5(  '   �� 5)  &   -"   5        $$ btn_fetchtamplateFile1_Copy341��5)  [   $��
0,22,142,0q�6-   
������ $��35��6�   $	��Top=�6�   $��36��6�   $��Right=�6�   $ Export to CSV��6�   .��5*  *   �� $��22��64   $��19��5+  -   ���	#FF3373F2 5-  .   !� 5.  2   $
 Fill=�6>   # [ 5/  5   51  +   52  '   �� 54  &   -#   5        $ 	listBox11��54  .   $��0,62,0,0q�6A   .@�55  *   �� $	��IRR��66   �� 56  *   $
��IRRI��66    . 6E   �� 57  *   $��IR��66   58  '   �� 59  &   -$   5        $# btn_fetchtamplateFile1_Copy11��59  Z   $��	0,22,21,0q�6-   
������ $��35��6�   $	��Top=�6�   $��35��6�   $��Right=�6�   $ Delete Note Type��6�   .��5:  *   �� $��22��64   $��19��5;  -   ���	#FF3373F2 5=  .   !� 5>  2   $
 Fill=�6>   # [ 5?  5   5A  +   5B  '   5D  #   5E     5G     5H     5I     5J     �� 5K     $�� ��6   $��Settings��6!   5L     5N     5O     