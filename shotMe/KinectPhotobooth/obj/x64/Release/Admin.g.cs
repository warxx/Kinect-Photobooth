﻿#pragma checksum "..\..\..\Admin.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "D507D8D31B6E968E97DBF7F5EAAD69AC05F05DD2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using KinectPhotobooth;
using Microsoft.Kinect.Wpf.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace KinectPhotobooth {
    
    
    /// <summary>
    /// Admin
    /// </summary>
    public partial class Admin : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 113 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Kinect.Wpf.Controls.KinectRegion kinectRegion;
        
        #line default
        #line hidden
        
        
        #line 143 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ImagePreview;
        
        #line default
        #line hidden
        
        
        #line 162 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid CompositeImage;
        
        #line default
        #line hidden
        
        
        #line 168 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem Settings;
        
        #line default
        #line hidden
        
        
        #line 169 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem cbCountDownConfig;
        
        #line default
        #line hidden
        
        
        #line 170 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem cbOutputImages;
        
        #line default
        #line hidden
        
        
        #line 174 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem cbFacebook;
        
        #line default
        #line hidden
        
        
        #line 209 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image Backdrop;
        
        #line default
        #line hidden
        
        
        #line 210 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MediaElement BackVideoDrop;
        
        #line default
        #line hidden
        
        
        #line 261 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image BackgroundDrop;
        
        #line default
        #line hidden
        
        
        #line 262 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MediaElement BgVideoDrop;
        
        #line default
        #line hidden
        
        
        #line 293 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image InitOverlayDrop;
        
        #line default
        #line hidden
        
        
        #line 326 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image EndOverlayDrop;
        
        #line default
        #line hidden
        
        
        #line 334 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox Checkbox_Images;
        
        #line default
        #line hidden
        
        
        #line 335 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox Checkbox_Videos;
        
        #line default
        #line hidden
        
        
        #line 339 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider BackgroundDistanceControl;
        
        #line default
        #line hidden
        
        
        #line 341 "..\..\..\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tblGestures;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/KinectPhotobooth;component/admin.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Admin.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 10 "..\..\..\Admin.xaml"
            ((KinectPhotobooth.Admin)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.kinectRegion = ((Microsoft.Kinect.Wpf.Controls.KinectRegion)(target));
            return;
            case 3:
            this.ImagePreview = ((System.Windows.Controls.Image)(target));
            return;
            case 4:
            
            #line 147 "..\..\..\Admin.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Back_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.CompositeImage = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.Settings = ((System.Windows.Controls.MenuItem)(target));
            
            #line 168 "..\..\..\Admin.xaml"
            this.Settings.Click += new System.Windows.RoutedEventHandler(this.cbSettings_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.cbCountDownConfig = ((System.Windows.Controls.MenuItem)(target));
            
            #line 169 "..\..\..\Admin.xaml"
            this.cbCountDownConfig.Click += new System.Windows.RoutedEventHandler(this.cbCountDownConfig_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.cbOutputImages = ((System.Windows.Controls.MenuItem)(target));
            
            #line 170 "..\..\..\Admin.xaml"
            this.cbOutputImages.Click += new System.Windows.RoutedEventHandler(this.cbOutputImages_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 171 "..\..\..\Admin.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.cbFacebook = ((System.Windows.Controls.MenuItem)(target));
            
            #line 174 "..\..\..\Admin.xaml"
            this.cbFacebook.Click += new System.Windows.RoutedEventHandler(this.cbFacebook_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 185 "..\..\..\Admin.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ShowInitOverlay_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 186 "..\..\..\Admin.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RefreshInitOverlays);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 194 "..\..\..\Admin.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ShowEndOverlay_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 195 "..\..\..\Admin.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RefreshEndOverlays);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 203 "..\..\..\Admin.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ShowBackgrounds_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 204 "..\..\..\Admin.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RefreshBackgrounds);
            
            #line default
            #line hidden
            return;
            case 17:
            this.Backdrop = ((System.Windows.Controls.Image)(target));
            return;
            case 18:
            this.BackVideoDrop = ((System.Windows.Controls.MediaElement)(target));
            
            #line 210 "..\..\..\Admin.xaml"
            this.BackVideoDrop.MediaEnded += new System.Windows.RoutedEventHandler(this.BackVideoDrop_MediaEnded);
            
            #line default
            #line hidden
            return;
            case 19:
            
            #line 225 "..\..\..\Admin.xaml"
            ((System.Windows.Controls.ListBox)(target)).SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ListView_BackgroundChanged);
            
            #line default
            #line hidden
            return;
            case 20:
            this.BackgroundDrop = ((System.Windows.Controls.Image)(target));
            return;
            case 21:
            this.BgVideoDrop = ((System.Windows.Controls.MediaElement)(target));
            
            #line 262 "..\..\..\Admin.xaml"
            this.BgVideoDrop.MediaEnded += new System.Windows.RoutedEventHandler(this.BackVideoDrop_MediaEnded);
            
            #line default
            #line hidden
            return;
            case 22:
            
            #line 277 "..\..\..\Admin.xaml"
            ((System.Windows.Controls.ListView)(target)).SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ListView_OverlayChanged);
            
            #line default
            #line hidden
            return;
            case 23:
            this.InitOverlayDrop = ((System.Windows.Controls.Image)(target));
            return;
            case 24:
            
            #line 308 "..\..\..\Admin.xaml"
            ((System.Windows.Controls.ListView)(target)).SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ListView_EndOverlayChanged);
            
            #line default
            #line hidden
            return;
            case 25:
            this.EndOverlayDrop = ((System.Windows.Controls.Image)(target));
            return;
            case 26:
            this.Checkbox_Images = ((System.Windows.Controls.CheckBox)(target));
            
            #line 334 "..\..\..\Admin.xaml"
            this.Checkbox_Images.Checked += new System.Windows.RoutedEventHandler(this.ImagesCheckBox_Checked);
            
            #line default
            #line hidden
            return;
            case 27:
            this.Checkbox_Videos = ((System.Windows.Controls.CheckBox)(target));
            
            #line 335 "..\..\..\Admin.xaml"
            this.Checkbox_Videos.Checked += new System.Windows.RoutedEventHandler(this.VideosCheckBox_Checked);
            
            #line default
            #line hidden
            return;
            case 28:
            this.BackgroundDistanceControl = ((System.Windows.Controls.Slider)(target));
            return;
            case 29:
            this.tblGestures = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

