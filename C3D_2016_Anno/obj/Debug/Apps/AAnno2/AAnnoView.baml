   M S B A M L   `   `   `  ����J  FC3D_2016_Anno, Version=4.47.1.16, Culture=neutral, PublicKeyToken=nullE'clr-namespace:C3D_2016_Anno.Apps.AAnno2C3D_2016_Anno.Apps.AAnno2  5      U QMahApps.Metro, Version=1.4.0.42, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5      H DVirtuaLicense, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullD2clr-namespace:VirtuaLicense;assembly=VirtuaLicenseVirtuaLicense 5      )    #C3D_2016_Anno.Apps.AAnno2.AAnnoView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:C3D_2016_Anno.Apps.AAnno2     5      PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5      Glocalsdk2clr-namespace:VirtuaLicense;assembly=VirtuaLicense     5      $	��353��5      $	��634��6   $	��500��6'     � 	Resources  5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $I Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?�6%   �� 5      $F @pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml?�6%   �� 5      $G Apack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml?�6%   �� 5      $O Ipack://application:,,,/MahApps.Metro;component/Styles/Accents/cobalt.xaml?�6%   �� 5      $R Lpack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml?�6%   �� 5      $3 -/C3D_2016_Anno;component/Resources/Icons.xaml?�6%   %    5      .��5      �  � RowDefinitions 5   
   �� 5      $	��154 �6   �� 5      $��87 �6   �� 5      �� 5      $��95 �6   .+�5   
   �� $��Select Files��6   $��0,5,5,0q�6)   $	��147��6:   $	��Top=�6G   .��5       �  5!      �� 5"      $	��112 �6$   �� 5#      .+�5%      �� -   5         ��Name$ tBox_StyleStructureFile��5%      $��	0,0,175,0q�6;   $"��50                          ��6N   $	��Top=�5&       � ToolTip$ Style Structure File��64   *  $MahApps.Metro.Controls.TextBoxHelper   	Watermark$ Style Stucture File��6S   6�   �� 5'      -   5         ��Name$ btn_browse_SSTFile��5'   G   $��
0,15,135,0q�6   
������ $��35��6c   $	��Top=�6o   $��35��6�   $��Right=�6�   $ Browse a file.��6�   .��5(      �� $��22��6    $��19��5)      ���	#FF3373F2 J� OpacityMask 5+      !� 5,      	 j� Stretch$
	 Fill=�6*      appbar_folder
 !� Visual#
 [  5-   !   5/      50      �� 51      -   5        $��btn_openLearnTool��51      $��Open Learn Tool��62   $��353,22,16,0q�6L   $��28��6a   $	��Top=�6m   �� 52      -   5         ��Name$ cBox_template��52       �� SelectedValuePath$ fileName��6[   $��
0,55,175,0q�6x   $	��Top=�6�   $��50��6�   $��35��6�      UseFloatingWatermark . $ 	Note List��6$   �� SelectedValue 63   ��  �� Path$ fileName���53      �� 54      .R�55      ��  �� Orientation$ Vertical=�6)   .+�56   "   �� $��8(�6K   ��6,   �� $ filePath�6K   �� 57   "   ��6,   �� $ fileName� 6K   �� $ filePath�58      59      5;      �� 5<      -   5        $ btn_open_templateFile��5<   G   $��
0,70,135,0q�6   
������ $��35��6f   $	��Top=�6r   $��35��6�   $��Right=�6�   $# Open selected file in notepad��6�   .��5=      �� $��22��6    $��19��5>      ���	#FF3373F2 5@      !� 5A      $
	 Fill=�6*     appbar_edit#
 [ 5B   !   5D      5E      �� 5F      -   5        $# btn_fetchSelectedtamplateFile��5F   F   $��	0,70,96,0q�6   
������ $��35��6m   $	��Top=�6y   $��35��6�   $��Right=�6�   $ Refresh selected file��6�   .��5G      �� $��22��6    $��19��5H      ���	#FF3373F2 5J      !� 5K      $
	 Fill=�6*     appbar_refresh#
 [ 5L      5N      5O      �� 5P      -   5        $ btn_browse_templateFile��5P   F   $��	0,70,56,0q�6   
������ $��35��6g   $	��Top=�6s   $��35��6�   $��Right=�6�   $ Browse a file.��6�   .��5Q      �� $��22��6    $��19��5R      ���	#FF3373F2 5T      !� 5U      $
	 Fill=�6*   #
 [  5V   !   5X      5Y      �� 5Z      -   5        $" btn_browse_templateFile_Copy��5Z   F   $��	0,70,16,0q�6   
������ $��35��6l   $	��Top=�6x   $��35��6�   $��Right=�6�   $= 7Scan and get all files from the default template folder��6�   .��5[      �� $��22��6    $��19��5\      ���	#FF3373F2 5^      !� 5_      $
	 Fill=�6*     appbar_download#
 [ 5`   !   5b      5c      5d      5f      �� 5g   
   -	   5         ��Name$ groupBox��5g      $��	Selection��6%   $
��Left=�68   $��0,4,0,0q�6S   $��1��6d   $	��Top=�6q   $��81��6�   $	��495��6�   .��5h      � .+�5i      �� -
   5         ��Name$ label1��5i      $��Create boundary��6(   $
��Left=�6B   $��	10,10,0,0q�6]   $	��Top=�6p   $	��103��6�   �� 5j      -   5        $ btn_selectLabels��5j   G   $��
118,10,0,0q�6   
������ $
��Left=�6a   $��26��6|   $��26��6�   $	��Top=�6�   $! Selected labels from screen��6�   .��5k      �� $��14��6    $��11��5l      ���	#FF3373F2 5n      !� 5o      $
	 Fill=�6*     appbar_image_ants#
 [ 5p      5r      5s      �� 5t      -   5        $ label1_Copy��5t      $��Pick Viewport��6-   $
��Left=�6E   $��
149,10,0,0q�6`   $	��Top=�6t   $��82��6�   �� 5u      -   5        $ btn_selectViewport��5u   G   $��
236,11,0,0q�6   
������ $
��Left=�6c   $��26��6~   $��26��6�   $	��Top=�6�   $B <Selected viewport to process the labels within the viewport.��6�   .��5v      �� $��14��6    $��11��5w      ���	#FF3373F2 5y      !� 5z      $
	 Fill=�6*     appbar_image_focus#
 [ 5{   !   5}      5~      �� 5      -   5        $ lbl_statusCount��5      $�� ��61   $��	0,10,10,0q�6<   $	��Top=�6O    J� RenderTransformOrigin$ 
0.509,0.59&�6g   $ Labels processed.��6�   $��Right=�6�   $	��171��6�   ���	#FF0646C56�   -  'MahApps.Metro.Controls.MetroProgressBar  5�      -   5          Name$ proBar��5�   ,   $��-5,40,-5,-5q�6<    � Maximum$	 100Q�6Q   $	��Top=�6_   $��7��6w   ���	#FF3373F2 � Cursor$
 Hand��6�   5�      5�      �� 5�   
   $��0,3,5,5q�6   $��2��6$   $��Preview��61   .��5�      �  5�      �� 5�      $��36 �6$   �� 5�      .+�5�      �� $��
95,7,344,3q�6   .��6,   Filter64   �� 5�      -   5        $ cBox_objectType��5�      $
��Left=�64   $ �� IsSynchronizedWithCurrentItem$
 TrueY�6O   $	��Top=�6v   $	��239��6�   $��35��6�   $ Object Type��6�    . $��35��6<   �� MaxDropDownHeight$	 351��6K  $��	139,0,0,0q�6c  �� 5�      -   5        $ btn_clearCollectionandUI��5�   E   $��0,0,65,0q�6   
������ $��35��6g   $	��Top=�6s   $��35��6�   $��Right=�6�   $ 	Clear All��6�   .��5�      �� $��22��6    $��19��5�      ���	#FF3373F2 5�      !� 5�      $
	 Fill=�6*   #
 [ 5�      5�      5�      �� 5�      -   5         ��Name$ lBox_CurrentNotes��5�      $��0,2,0,0q�66   $��1��6G    �� View 5�      �� 5�      .P�5�      ��  �� Width$	 120��6-    �� DisplayMemberBinding 69   �� *	NoteNum+��5�   "   �� 5�   &   -   5        
 � Tag$ NoteNum��5�   ;   .��6l   Note Number6y   5�      �� 5�       6-   �� *
NoteName+��5�   "   �� 5�   &   -   5        $ NoteName��5�   ;   .��6m   Note6s   5�      5�      5�      5�      5�      �� 5�   
   $��0,0,5,6q�6   $��3��6$   $��Create Keynote��61   .��5�      � .+�5�      �� -   5        $ tBox_Heading��5�      $��35��60      ClearTextButton . $��
10,8,315,0q�6j    �� TextWrapping$
 Wrap=�6~   $	��Top=�6�   $ Heading��6�    . $��35��6  $ Specify heading here.��6  d� 5�      -   5          d�Name$  btn_HeadingBold��5�   B   $
��Bold��6   $��	0,9,250,0q�6.   $��34��6[   $	��Top=�6g   $��60��6   $��35��6�   $��Right=�6�   d� 5�      -   5        $  btn_HeadingUnderline��5�   F   $��	Underline��6   $��	0,9,185,0q�63   $��34��6d   $	��Top=�6p   $��35��6�   $��Right=�6�   $��60��6�   �� 5�      -   5        $ tBox_textHeight��5�      $��35��63   $��	0,9,115,0q�6?   $
 Wrap=�6R   $��.1��6f   $	��Top=�6p   $��35��6�   $ Text Height��6�    . $��Right=�6�   $��65��6  �� 5�      -   5        $ btn_CreateKeyNote��5�      $��Create Keynote��64   $��0,9,10,0q�6M   $	��Top=�6_   $��35��6�   $��35��6�   
����������	#FF3373F2$��Right=�6�   $	��100��6�   $ Add keynote to drawing.��6  5�      5�         VirtuaLicense.LoginControl  5�   
   -   5        !  Name$! loginUserControl��5�       $	��500��6:   $y�Hidden=�6G   $��	0,23,0,28q�6[   $��1��6n   $��2��6{   6�   5�      5�      