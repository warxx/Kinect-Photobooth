   M S B A M L   `   `   `  ���� K  GKinectPhotobooth, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:KinectPhotoboothKinectPhotobooth      KinectPhotobooth.Admin   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     d `Microsoft.Kinect.Wpf.Controls, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e350k(http://schemas.microsoft.com/kinect/2014  *localclr-namespace:KinectPhotobooth  -     � 
ResizeMode$  NoResize=� � Title$ Admin��$��976.5��$
��1920�� � 	Resources    MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  TextBlockStyle  CheckboxStyle  SimpleListBoxItem �� 
TargetType �� Triggers	 P� Property
 P� Value �� 
TargetName  RefreshButton �� Orientation ��Name  
BackButton ��Name �� TextAlignment��%�  &	        &	      &	 *     '�� ?     &	 t     &	 �     &	 <    &	 �    &	 h    7  7  �� $ 	#ff6e6e6e���� $ 	#ff52318f���� $ 	#ff00BCF2���� " ��.���  �@J�SnapsToDevicePixels . �� " ��.���  � ��$ Segoe UI)���  � ��$ 18(���  � ��
 �� ���� " ��.���  � ��$ Segoe UI)���  � ��$ 18(���  � ��
 �� ����  � ��$ Center=��� " ��.���  � �� �� " ��.R��� $��Border�� P� 	 �@��
IsSelected
 . . ��� $ Border�� � ��
 �  ���� " ��.���  � �� �� " ��.R�� .+��� $ 
Horizontal=�
������ .+�� .+��� $ ScreenshotNormal��$��Images\refresh-icon.png��$
��None=�$��Center=��� $ ScreenshotHover��$#��Images\refresh-icon-hover.png��$
��None=�$��Center=�$y�	Collapsed=� P� 	 �@J�IsMouseOver
 . . ��� $ ScreenshotNormal�� � y�$ 	Collapsed=��� $ ScreenshotHover�� � y�$ Visible=��� " ��.���  � �� �� " ��.R�� .+��� $ Vertical=�
������ .+�� .+��� $ ScreenshotNormal��$��Images\exit.png��$
��None=�$��Center=��� $ ScreenshotHover��$��Images\exit-hover.png��$
��None=�$��Center=�$y�	Collapsed=��� $ BackText��$
��Back��$ Center=�$��Center=�$��Center=�8��  $��15(�$��0q�$��52�� P� 	 �@J�IsMouseOver
 . . ��� $ ScreenshotNormal�� � y�$ 	Collapsed=��� $ ScreenshotHover�� � y�$ Visible=��� 8  $ BackText�� � ��.��0  *Microsoft.Kinect.Wpf.Controls.KinectRegion  -     Name$ kinectRegion��.��� $��	10 0 10 0q� � ColumnDefinitions �� $��50 ��� $��4* ��� $��50 � � RowDefinitions �� $
��Auto ��� �� $
��Auto �$��86��.+�� $��1��$��0��$��2�� �� $	��550 ��� $��2* ��� $��2* ��� $��4* � �� $��15 ��� $	��150 ��� $��25 �.+��� $��0��$��1��$��Uniform=�$
��Left=�$��Images\AdminPane.png���� -   $ ImagePreview��$��1��$��1��$��Uniform=�$��Center=�$��	20,0,20,0q�4  .Microsoft.Kinect.Wpf.Controls.KinectUserViewer  $��2��$��1��$	��150��$��Stretch=�$��Stretch=�   ImageBackground
 ��  ��� $��Admin��$
��80px(�$��221,18,67,21q�$��1���� -   #��[ $��1��$��3��$��Center=�$��Right=�$��Center=�$��Center=�$��
0,56,35,38q�$��56��$��36��� $��1��$��1�� �� �� $	��255 ��� $	��260 ��� $	��260 � �� $	��50* ��� $
��619* �.+�� -   $��CompositeImage��$��0��$��0��.+�o� $
��Left=�$��42��$	��Top=�$	��200��.@�m� $��General��$��42��$	��100��.@�m� -   $��Settings��$��Settings��$��42��$
��Left=�$	��186��$��	0,0,-25,0q�m� -   $��cbCountDownConfig��$��Countdown Config��$��42��$
��Left=�$	��186��$��	0,0,-25,0q�{�.  m� -   $��cbOutputImages��$��Output Images Path��$��42��$
��Left=�$	��192��$��0,0,-6,0q�{�.  m� -	   $��Backgrounds Path��$��42��$
��Left=�$	��192��$��0,0,-6,0q�{�.  m� $��Social Groups��$��42��$	��100��.@�m� -
   $��
cbFacebook��$��Share To Facebook��$��42��$
��Left=�$	��185��$��	0,0,-24,0q�m� $��Send To Mail��$��42��$��	0,0,-21,0q�{�.  � $��1��$��0��.+��� $��Center=�$��Center=�$��	70,0,69,0q�#��[ $��20(�Init Overlays�� -   $��10,10,200,10q�$
��Show���� -   $��210,18,25,12q�#��[ � $��2��$��0��.+��� $��Center=�$��Center=�$��	70,0,69,0q�#��[ $��20(�End Overlays�� -   $��10,10,200,10q�$
��Show���� -   $��210,18,30,12q�#��[ � $��3��$��0��.+��� $��Center=�$��Center=�$��70,12q�#��[ $��20(�$	��115��Backgrounds�� -   $��10,10,200,10q�$
��Show���� -   $��210,18,30,12q�#��[ � $��0��$��1��.+��� -   $��Backdrop��$��10,60,10,40q�$
��Fill=�r� -   $��BackVideoDrop�� r� Stretch$
 Fill=�$��10,60,10,10q�$y�Hidden=� r� Source ��  �� Path$" SelectedBackground.ImagePath��� $��3��$��1��.���  �� �� $	��230 �.+��� -   $��Visible=�$��Visible=� �� SelectionMode$ Single=�$��Center=�$
��10,0q�$	��240������ $ Backgrounds� �� SelectedItem �� $ SelectedBackground� �� Mode$ TwoWay=�  �� Triggers ]� 
Conditions �� Property �� Value �� Binding �� Value��%�   '��       �� " �� ]�  ��  �@��
IsSelected . ./���  � ��
 �  ����  ��  	My Value6.a���  � {� .  ���� .R��� $��Center=�$��Center=�.+��� $��Center=�#��[ $��Center=�$��15(����� $
 Name�� $��1��$��0��.+��� -   $��BackgroundDrop��$��15,30,15,40q�$
��Fill=�r� -   $��BgVideoDrop��$
 Fill=�$��15,30,15,40q�$y�Hidden=� �� $" SelectedBackground.ImagePath��� $��1��$��2��.���  �� �� $	��230 �.+��� -   $��Visible=�$��Visible=�$ Single=�$��Center=�$
��10,0q�$	��235������ $ Overlays� �� $ SelectedOverlay�$ TwoWay=����� .R��� $��Center=�$��Center=�.+��� $��Center=�#��[ $��Center=�$��15(����� $
 Name�� $��1��$��0��.+��� -   $��InitOverlayDrop��$��15,30,15,40q�$
��Fill=��� $��2��$��2��.���  �� �� $	��230 �.+��� -   $��Visible=�$��Visible=�$ Single=�$��Center=�$
��10,0q�$	��240������ $ EndOverlays� �� $ SelectedEndOverlay�$ TwoWay=����� .R��� $��Center=�$��Center=�.+��� $��Center=�#��[ $��Center=�$��15(����� $
 Name�� $��1��$��0��.+��� -   $��EndOverlayDrop��$��15,30,15,40q�$
��Fill=�� $��1��$��2��$��Right=�$	��570��$��47��$	��Top=�.+��� #��[ $��0,0,50,0q�$
��Fill��$��53��  J� RenderTransformOrigin$  0.623,1.615&�! d� 	IsChecked! �� *
PersonFill+$ TwoWay=��� -   $��Checkbox_Images��#��[ $��345,0,141,0q�$��Images���� -   $��Checkbox_Videos��#��[ $��
450,0,40,0q�$��Videos���� $ Vertical=�$��0��$��1��.+��� $��10(����� $ BackgroundDistance�$ OneWay=��� -   " ��Name$" BackgroundDistanceControl��# �� Orientation$# Vertical=�$ � Minimum$	$ 100Q�% � Maximum$
% 4500Q�$	��450��& � Value& �� $ BackgroundDistance�$ TwoWay=��� -   ' ��Name$' tblGestures��$��1��$��Right=�$��32��$��
0,10,654,0q�$��2��( �� TextWrapping$
( Wrap=�$	��Top=�$	��262��