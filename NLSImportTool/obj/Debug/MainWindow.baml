   M S B A M L   `   `   `  ����H  DNLSImportTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:NLSImportToolNLSImportTool  5   	       NLSImportTool.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:NLSImportTool     5   	     � Title$  
MainWindow��5   	   $	��600��6   $	��800��6)   .��5E      �  � RowDefinitions 5F   
   �� 5G      $��* �6   6)   �� 5H      $
��auto �6   6,   .+�5J   
   � -   5         �Name$ FirstPageGrid��5J      $��0��6   $y�Visible=�63    5K      �� 5L      $
��auto �6    60   �� 5M      $
��auto �6    60   .+�5O      �� $��0��6   $��10,5,0,0q�6%   $��14(�67   $��Center=�6E   $b��\copy your keys to here and please ensure the order is correct with your tanslation resource.��6`   6�   �� 5P      -   5         ��Name$ ClearBtn��5P      $��0��6'   $��50��64   $��30��6?   $��	10,5,10,0q�6K   $��Right=�6^   $��Clear��6z   6�   �� 5Q      -   5         ��Name$ KeyContentRtb��5Q      $��1��61   $	��400��6>   $��10q�6K   $
��Auto=�6W   6�   5R      � 5S   
   -   5        $ SecondPageGrid��5S      $��0��6   $y�	Collapsed=�64    5T      �� 5U      $
��auto �6    60   �� 5V      $
��auto �6    60   �� 5W      $
��auto �6    60   .+�5Y      ��  �� Orientation$ Vertical=�6   $��	0,100,0,0q�60   .+�5Z      �� -   5         ��Name$ ChooseNLSrootFolderText��5Z      $
��Left=�6=   $��	170,5,0,5q�6X   $8��2FROM: where is your language resource root folder?��6l   6�   �� 5[      $ 
Horizontal=�6   $	��Top=�66   $��Center=�6N   $��0q�6k   .+�5\      �� -   5         ��Name$ SelectedFolderTexBox_NLS��5\       �� 
IsReadOnly . $��8q�6w   $��15(�6�   $	��300��6�   $
��20,0q�6�   
���������6@   �� *	NLSPath+	 �� Mode$	 TwoWay=�6�   �� 5]      -   5        $	��150��5]      $��40��6)   $��Pick Folder��65   6p   5^      �� 5_      $ 
Horizontal=�6   $��
170,10,0,0q�66   .+�5`      	� -   5        $��ManualRaidoButton��5`   4   $��0,0,0,0q�6#   
 	� 	GroupName$
 IsManual��6M   $��Manual��6b   6�   	� 5a      -	   5        $��AutoImportRaidoButton��5a   5   $��10,0,0,0q�6#   $
 IsManual��6R   $*��$Auto import to your project directly��6g   6�   5b      5d      �� 5f      -
   5         ��Name$ AutoModeGrid��5f      $ Vertical=�6/   $��0,10,0,0q�6F   $��1��6X   .+�5g      �� -   5        $" ChooseImportTargetFolderText��5g      $
��Left=�6B   $��	170,5,0,5q�6]   $;��5TO: where is your import target resource root folder?��6q   6�   �� 5h      $ 
Horizontal=�6   $	��Top=�66   $��Center=�6N   $��0q�6k   .+�5i      �� -   5        $' !SelectedFolderTexBox_ImportTarget��5i       . $��8q�6�   $��15(�6�   $	��300��6�   $
��20,0q�6�   
���������6I   �� *ImportTargetPath+$	 TwoWay=�6�   �� 5j      -   5        $	��150��5j      $��40��6)   $��Pick Folder��65   6v   5k      5l      � 5n      $��2��6   .+�5o      �� $
��0,80q�6   .+�5p      �  � Maximum$	 100Q�6Q    � Minimum$ 0Q�6_    � Value 6"   �� *ProgressBarValue+$	 TwoWay=�6m   �� 5q      $��Center=�6    $��Center=�6;   $��Please wait...��6Y   6q   �� 5r      $��Center=�6    $��Center=�6;   $	��0,5q�6X   $
��Wrap=�6�   ��6e   �� *ProcessStatus+$	 TwoWay=�6�   5s      5t      5u      � 5w   
   $��1��6   $��10q�6   .+�5z      �� $��0,0,0,10q�6   $ 
Horizontal=�6+   $	��Top=�6D   $��Right=�6\   .+�5{      �� 
���  ��$��Center=�6X   $
��10,0q�6s   ��6   �� *ValidateText+$	 TwoWay=�6�   �� 5}      $��Center=�6D   $
��10,0q�6_   ��6   �� *	KeysCount+$	 TwoWay=�6o   �� 5~      $��Keys Loaded��6   $��Center=�6/   6L   �� 5      -   5        $ 
BackButton��5      {�.  $��10,10q�6C   $��80��6R   $��30��6]   .��5�      �� -   5        $ Power_Section_BackDesc��5�       Power_Section_BackDesc��$
��Back��6_   6m   5�      �� 5�      -   5        $ 
NextButton��5�      $��10,10q�6-   $��80��6<   $��30��6G   $y�Visible=�6S   .��5�      �� -   5        $ Power_Section_NextDesc��5�       Power_Section_NextDesc��$
��Next��6_   6m   5�      �� 5�      -   5        $ OKButton��5�      $��10,10q�6+   $��80��6:   $��30��6E   $y�	Collapsed=�6Q   .��5�      �� -   5        $ Power_Section_OkDesc��5�       Power_Section_OkDesc��$��OK��6[   6g   5�      �� 5�      -   5        $ CancelButton��5�      $��20,10q�60   $��80��6?   $��30��6J   $y�	Collapsed=�6V   .��5�      �� -   5        $ Power_Section_CancelDesc��5�       Power_Section_CancelDesc��$��Cancel��6c   6s   5�      5�      5�      5�      5�      