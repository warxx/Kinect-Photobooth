   M S B A M L   `   `   `  ����K  GKinectPhotobooth, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:KinectPhotobooth.CommonKinectPhotobooth.Common  5   	   !    KinectPhotobooth.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   d `Microsoft.Kinect.Wpf.Controls, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e350k(http://schemas.microsoft.com/kinect/2014  5   	   4commands%clr-namespace:KinectPhotobooth.Common  5   	   -   5          � Title$  Kinect Photobooth��5   	   $
��1920��5   	   $
��1080��6    � 
ResizeMode$ NoResize=�5   	    J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$" commands:CustomCommands.Exit��5      6d     5   
   -   5        $% commands:CustomCommands.GoAdmin��5      6j    � 	Resources 5         MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  TextBlockStyle  CheckboxStyle  ScreenshotButton	 �� 
TargetType
 �� Orientation ��Name �� Triggers P� Property P� Value �� 
TargetName  FacebookButton  AnnotationStyle��%�  &	        &	 #     &	 F     '�� i     &	 �     &	 [    &	     &	 �    &	     �� 5   
   $ 	#ff6e6e6e��62   �� 5   
   $ 	#ff52318f��64   �� 5   
   $ 	#ff00BCF2��62   �� 5   
   " ��6   .�5      ��  �@J�SnapsToDevicePixels . 64   5      �� 5   
   " ��6'   .�5      ��  � ��$ Segoe UI)�6+   �� 5       � ��$ 18(�6)   �� 5       � ��
 �� ��6+   5      �� 5   
   " ��6&   .�5      ��  � ��$ Segoe UI)�6+   �� 5       � ��$ 18(�6)   �� 5        � ��
 �� ��6+   �� 5!       � ��$ Center=�69   5"      �� 5$   
   " ��6   .�5%      ��  � �� 5&      �� 5'      "	 ��6&   .R�5(      � .+�5)      �� $
 
Horizontal=�6)   
������ .+�5+   "   � .+�5,   &   �� $ ScreenshotNormal��6,   $��Images\ScreenShot.png��6F   $
��None=�6e   $��Center=�6t   �� 5-   &   $ ScreenshotHover��6,   $ ��Images\ScreenShotHober.png��6E   $
��None=�6i   $��Center=�6x   $y�	Collapsed=�6�   5.   #   5/      50       51      P� 52       �@J�IsMouseOver . . �53   "   �� $ ScreenshotNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 54   "   $ ScreenshotHover��6O    � y�$ Visible=�6?   6O   56      58      5:      5;      �� 5=   
   " ��6   .�5>      ��  � �� 5?      �� 5@      "	 ��6&   .R�5A      � .+�5B      �� $
 
Horizontal=�6)   
������ .+�5D   "   � .+�5E   &   �� $ ScreenshotNormal��6,   $��Images\facebook.png��6F   $
��None=�6c   $��Center=�6r   5F   #   5G      5H      5I      5K      5L      �� 5N   
   " ��6(   .�5O      ��  � ��$ Center=�64   �� 5P       � ��$ Center=�62   �� 5Q       �@��TextAlignment$ Center=�6.   �� 5R       � ��$
 Wrap=�6-   �� 5S       � ��$	 300(�6)   �� 5T       � ��
 �����6+   5U      5V      .��5X      0  *Microsoft.Kinect.Wpf.Controls.KinectRegion  -   5          Name$ kinectRegion��5X      .��5Y   
   �  � ColumnDefinitions 5Z      �� 5[       � RowDefinitions 5^      �� 5_      $
��Auto �6    �� 5`      �� 5a      $
��Auto �6    .+�5m      � -   5        $��CompositeImage��5m      $��1��6)   .+�5n      �� -   5        $��Backdrop��5n      $
��Fill=�6)   r� 5o      -   5        $��BackVideoDrop��5o       r� Stretch$
 Fill=�65   $y�Hidden=�6�    r� Source 6D   ��  �� Path$" SelectedBackground.ImagePath�6�   �� 5p      -   5        $��Persdrop��5p      $��UniformToFill=�6G   ��6(   �� *ImageSource+6G   �� 5q      -	   5        $��	Countdown��5q      $y�Hidden=�6)   5r      � 5t      -
   5        $��InitOverlayImage��5t      $��1��6+   .+�5u      �� -   5        $��Overdrop��5u      $
��Fill=�6)   5v      � 5x      -   5        $��ScreenshotGrid��5x      $��1��6)   $y�Hidden=�66   .+�5z      �� -   5        $��Screenshotdrop��5z      $
��Fill=�6P   $��216,111,259,113q�6_    J� RenderTransformOrigin$ 0.5,0.5&�6x   $��Center=�6�   $��Center=�6�   $
��1439��6�   $	��827��6�   ��6.   �� *ScreenshotPath+z�5{      V� 5|      .~�5}      �� �� 5~      �� 5       �� Angle$ -7.531Q�6.   T� 5�      5�      5�      �� 5�      -   5        $��ScreenshotOver��5�      $
��Fill=�6/   �� 5�      -   5        $��FacebookButton��5�      #��[ 6/   $��1��6W   $��Center=�6d   $��Center=�6   $��Center=�6�   $��Center=�6�   $	��100��6�   $	��100��6  $��70,630,1744,321q�6  $ 0.5,0.5&�63  z�5�      V� 5�      .~�5�      �� �� 5�      �� 5�      $ -11.473Q�6.   T� 5�      5�      5�      5�      �� 5�      -   5        $��ScreenshotButton��5�      #��[ 6-   $��1��6W   $��Stretch=�6d   $��Center=�6�   $��
0,945,0,10q�6�   5�      5�      5�      