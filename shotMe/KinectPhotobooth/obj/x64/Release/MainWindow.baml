   M S B A M L   `   `   `  ���� K  GKinectPhotobooth, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:KinectPhotobooth.CommonKinectPhotobooth.Common  !    KinectPhotobooth.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     d `Microsoft.Kinect.Wpf.Controls, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e350k(http://schemas.microsoft.com/kinect/2014  4commands%clr-namespace:KinectPhotobooth.Common  -     � Title$  Kinect Photobooth��$
��1920��$
��1080�� � 
ResizeMode$ NoResize=� J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$" commands:CustomCommands.Exit��  -   $% commands:CustomCommands.GoAdmin�� � 	Resources    MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  TextBlockStyle  CheckboxStyle  ScreenshotButton	 �� 
TargetType
 �� Orientation ��Name �� Triggers P� Property P� Value �� 
TargetName  FacebookButton  AnnotationStyle��%�  &	        &	      &	 *     '�� ?     &	 t     &	 �     &	 <    &	 �    &	 �    �� $ 	#ff6e6e6e���� $ 	#ff52318f���� $ 	#ff00BCF2���� " ��.���  �@J�SnapsToDevicePixels . �� " ��.���  � ��$ Segoe UI)���  � ��$ 18(���  � ��
 �� ���� " ��.���  � ��$ Segoe UI)���  � ��$ 18(���  � ��
 �� ����  � ��$ Center=��� " ��.���  � �� �� "	 ��.R�� .+��� $
 
Horizontal=�
������ .+�� .+��� $ ScreenshotNormal��$��Images\ScreenShot.png��$
��None=�$��Center=��� $ ScreenshotHover��$ ��Images\ScreenShotHober.png��$
��None=�$��Center=�$y�	Collapsed=� P�  �@J�IsMouseOver . . ��� $ ScreenshotNormal�� � y�$ 	Collapsed=��� $ ScreenshotHover�� � y�$ Visible=��� " ��.���  � �� �� "	 ��.R�� .+��� $
 
Horizontal=�
������ .+�� .+��� $ ScreenshotNormal��$��Images\facebook.png��$
��None=�$��Center=��� " ��.���  � ��$ Center=���  � ��$ Center=���  �@��TextAlignment$ Center=���  � ��$
 Wrap=���  � ��$	 300(���  � ��
 �����.��0  *Microsoft.Kinect.Wpf.Controls.KinectRegion  -     Name$ kinectRegion��.���  � ColumnDefinitions ��  � RowDefinitions �� $
��Auto ��� �� $
��Auto �.+�� -   $��CompositeImage��$��1��.+��� -   $��Backdrop��$
��Fill=�r� -   $��BackVideoDrop�� r� Stretch$
 Fill=�$y�Hidden=� r� Source ��  �� Path$" SelectedBackground.ImagePath��� -   $��Persdrop��$��UniformToFill=����� *ImageSource+�� -	   $��	Countdown��$y�Hidden=�� -
   $��InitOverlayImage��$��1��.+��� -   $��Overdrop��$
��Fill=����� $ SelectedOverlay.ImagePath�� -   $��ScreenshotGrid��$��1��$y�Hidden=�.+��� -   $��Screenshotdrop��$
��Fill=�$��237,178,274,160q� J� RenderTransformOrigin$ 0.5,0.5&�$��Center=�$��Center=�$
��1403��$	��713������ *ScreenshotPath+z�V� .~��� �� ��  �� Angle$ -8.019Q�T� �� -   $��ScreenshotOver��$
��Fill=����� $" SelectedEndOverlay.ImagePath��� -   $��FacebookButton��#��[ $��1��$��Center=�$��Center=�$��Center=�$��Center=�$	��100��$	��100��$��70,630,1744,321q�$ 0.5,0.5&�z�V� .~��� �� �� $ -11.473Q�T� �� -   $��ScreenshotButton��#��[ $��1��$��Center=�$��Center=�$��Center=�$��Center=�$��872,936,944,19q�$��98��$��96��