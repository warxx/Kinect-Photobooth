   M S B A M L   `   `   `  ����K  GKinectPhotobooth, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:KinectPhotobooth.ConfigWindowsKinectPhotobooth.ConfigWindows  5   	   -    'KinectPhotobooth.ConfigWindows.Settings   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   >local,clr-namespace:KinectPhotobooth.ConfigWindows     5   	     � Title$  Settings��5   	   $��339.946��6   $��367.663��6+   .��5	      �  � RowDefinitions 5
   
   �� 5      $	��20* �6   �� 5      $	��20* �6   �� 5      $	��15* �6   �� 5      $	��20* �6   �� 5      $	��15* �6   �� 5      $	��20* �6   �� 5      $	��15* �6   �� 5      $	��20* �6   �� 5      $	��50* �6    � ColumnDefinitions 5   
   �� 5      $	��21* �6   �� 5      $
��193* �6   �� 5      $	��60* �6   �� 5      $	��65* �6   �� 5      $	��21* �6   .+�5   
   �� -   5         ��Name$ CountdownDelay��5   [   $��1��6   $��2��6    �� TextWrapping$ NoWrap=�6/    �� AcceptsReturn .  $��0,3,0,3q�6s   $��30��6�   $��Center=�6�   $��Center=�6�   {�.  6  �� 5   
   $��1��6   $��1��6   $��Delay before countdown:��6-   $��Right=�6O   $��Center=�6k   $��0,0,5,0q�6�   �� 5    
   $��1��6   $��3��6   $��seconds��6-   $
��Left=�6?   $��Center=�6Z   $��3,0,0,0q�6u   �� 5"   
   -   5        $ InitOverlayDelay��5"   [   $��3��6   $��2��6   $ NoWrap=�6/    .  $��0,3,0,3q�6u   $��30��6�   $��Center=�6�   $��Center=�6�   6�   �� 5#   
   $��3��6   $��1��6   $'��!Delay before get to Init Overlay:��6-   $��Right=�6Y   $��Center=�6u   $��0,0,5,0q�6�   �� 5$   
   $��3��6   $��3��6   $��seconds��6-   $
��Left=�6?   $��Center=�6Z   $��3,0,0,0q�6u   �� 5&   
   -   5        $ MinBackgroundDistance��5&   [   $��5��6   $��2��6   $ NoWrap=�6/    .  $��0,3,0,3q�6z   $��30��6�   $��Center=�6�   $��Center=�6�   6�   �� 5'   
   $��5��6   $��1��6   $��Min Background Distance:��6-   $��Right=�6P   $��Center=�6l   $��0,0,5,0q�6�   �� 5(   
   $��5��6   $��3��6   $��mm��6-   $
��Left=�6:   $��Center=�6U   $��3,0,0,0q�6p   �� 5*   
   -   5        $ MaxBackgroundDistance��5*   [   $��7��6   $��2��6   $ NoWrap=�6/    .  $��0,3,0,3q�6z   $��30��6�   $��Center=�6�   $��Center=�6�   6�   �� 5+   
   $��7��6   $��1��6   $��Max Background Distance:��6-   $��Right=�6P   $��Center=�6l   $��0,0,5,0q�6�   �� 5,   
   $��7��6   $��3��6   $��mm��6-   $
��Left=�6:   $��Center=�6U   $��3,0,0,0q�6p   �� 5.   
   -   5        $��btnSave��5.   -   $��8��6   $
��Save��6   $��	0,30,20,0q�6<   $��Right=�6O   $��57��6k   $��23��6v   $	��Top=�6�   $��2��6�   $��1��6�   6�   �� 5/   
   -   5        $��btnClose��5/   /   $��8��6   $��Cancel��6   $��	56,30,5,0q�6?   $��23��6R   $	��Top=�6^   $��2��6v   $��2��6�   6�   51      52      