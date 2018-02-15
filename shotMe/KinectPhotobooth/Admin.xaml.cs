using KinectPhotobooth.Common;
using KinectPhotobooth.Common.Gestures;
using KinectPhotobooth.ConfigWindows;
using KinectPhotobooth.Models;
using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KinectPhotobooth
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        private WriteableBitmap _ColorBitmap;
        //private MainWindowViewModel _vm;
        private Color[] colorArray;
        private WriteableBitmap _Overlay;

        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper _coordinateMapper = null;

        private FrameDescription _depthFrameDescription;

        private ushort[] DepthFrameData { get; set; }

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor _kinectSensor = null;

        /// <summary>
        /// Size of the RGB pixel in the bitmap
        /// </summary>
        private readonly int bytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

        /// <summary>
        /// Intermediate storage for pixes once frame is displayed 
        /// </summary>
        private byte[] _PreviousFrameDisplayPixels = null;

        /// <summary>
        /// Reader for depth/color/body index frames
        /// </summary>
        private MultiSourceFrameReader _reader = null;

        private WriteableBitmap _pointerBitmap;

        private ActionBlock<ImageModel> _ImageWriterActionBlock;
        private TransformBlock<ImageModel, ImageModel> _ImageTransformer;
        private TransformBlock<ImageModel, ImageModel> _BodyTransformer;

        private Random rnd = new Random();

        /// <summary>
        /// Occurs when a gesture is recognized.
        /// </summary>
        public event EventHandler<GestureEventArgs> GestureRecognized;

        GestureController _gestureController;

        private string _backgroundsPath;
        private string _overlaysPath;
        private string _endOverlaysPath;

        private SettingsModel settings;

        public Admin()
        {
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;

            this.InitializeComponent();

            LoadData();
        }

        public Admin(WriteableBitmap colorBitmap, KinectSensor kinectSensor, WriteableBitmap OverLay, CoordinateMapper coordinateMapper, FrameDescription depthFrameDescription, MultiSourceFrameReader reader)
        {
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;

            this.InitializeComponent();

            _kinectSensor = kinectSensor;
            _ColorBitmap = colorBitmap;
            ImagePreview.Source = _ColorBitmap;
            _Overlay = OverLay;
            _coordinateMapper = coordinateMapper;
            _depthFrameDescription = depthFrameDescription;
            _reader = reader;

          //  DataContainer.ViewModel.SelectedBackground.ImagePath = selectedBackgroundPath;
          //  DataContainer.ViewModel.SelectedOverlay.ImagePath = selectedOverlayPath;
          //  DataContainer.ViewModel.SelectedEndOverlay.ImagePath = selectedEndOverlayPath;

            LoadData();
        }
 
        private void LoadData()
        {
            this._overlaysPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\KinectPhotobooth\Images\InitOverlays"));
            DataContainer.ViewModel.OverlayPath = this._overlaysPath;

            this._endOverlaysPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\KinectPhotobooth\Images\EndOverlays"));
            DataContainer.ViewModel.EndOverlayPath = this._endOverlaysPath;

            //_vm = new MainWindowViewModel();

            colorArray = new Color[6];
            colorArray[0] = Colors.Blue;
            colorArray[1] = Colors.Red;
            colorArray[2] = Colors.Green;
            colorArray[3] = Colors.Yellow;
            colorArray[4] = Colors.Orange;
            colorArray[5] = Colors.HotPink;


            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.WindowState = System.Windows.WindowState.Maximized;

            if(DataContainer.ViewModel.Overlays.Count == 0)
            {              
                DataContainer.ViewModel.LoadBackgrounds(this._overlaysPath, "init_overlays");
            }

            if(DataContainer.ViewModel.EndOverlays.Count == 0)
            {             
                DataContainer.ViewModel.LoadBackgrounds(this._endOverlaysPath, "end_overlays");
            }

            //Fetch the event overlay 
          //  InitializeOverlay();

            // for Alpha, one sensor is supported
            this._kinectSensor = KinectSensor.GetDefault();


            if (this._kinectSensor != null)
            {
                // get the coordinate mapper
                this._coordinateMapper = this._kinectSensor.CoordinateMapper;

                // open the sensor
                this._kinectSensor.Open();                 

                int depthWidth = _depthFrameDescription.Width;
                int depthHeight = _depthFrameDescription.Height;
                DepthFrameData = new ushort[depthWidth * depthHeight];
                //_IRRawData = new ushort[kinectSensor.InfraredFrameSource.FrameDescription.Width * kinectSensor.InfraredFrameSource.FrameDescription.Height];
                //_IRConvertedData = new byte[kinectSensor.InfraredFrameSource.FrameDescription.Width * 4 * kinectSensor.InfraredFrameSource.FrameDescription.Height];

                // allocate space to put the pixels being received and converted

                this._PreviousFrameDisplayPixels = new byte[depthWidth * depthHeight * this.bytesPerPixel];
               
                // create the bitmap to display
                DataContainer.ViewModel.Bitmap = new WriteableBitmap(depthWidth, depthHeight, 96.0, 96.0, PixelFormats.Bgra32, null);

                _ColorBitmap = new WriteableBitmap(_kinectSensor.ColorFrameSource.FrameDescription.Width, _kinectSensor.ColorFrameSource.FrameDescription.Height, 96, 96, PixelFormats.Bgr32, null);


                FrameDescription colorFrameDescription = this._kinectSensor.ColorFrameSource.FrameDescription;

                int colorWidth = colorFrameDescription.Width;
                int colorHeight = colorFrameDescription.Height;



                // Create the Multisource reader and the types of streams it will use.
                // For this app, we will use Depth, Color, BodyIndex, Infrared, and Body
                this._reader = this._kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Depth |
                                                                            FrameSourceTypes.Color |
                                                                            FrameSourceTypes.BodyIndex |
                                                                            FrameSourceTypes.Infrared |
                                                                            FrameSourceTypes.Body);

                // set the status text
                //_vm.StatusText = Properties.Resources.InitializingStatusTextFormat;

                DataContainer.ViewModel.StatusText = Properties.Resources.InitializingStatusTextFormat;

            }
            else
            {
                // on failure, set the status text
                //_vm.StatusText = Properties.Resources.NoSensorStatusText;

                DataContainer.ViewModel.StatusText = Properties.Resources.NoSensorStatusText;
            }


            // initialize the components (controls) of the window
            this.InitializeComponent();


            KinectRegion.SetKinectRegion(this, kinectRegion);

            App app = ((App)Application.Current);
            app.KinectRegion = kinectRegion;

            // Use the default sensor
            this.kinectRegion.KinectSensor = KinectSensor.GetDefault();

            

            this.settings = new SettingsModel();
            string json = File.ReadAllText(Directory.GetCurrentDirectory() + "/Settings/settings.json");
            if(json.Length != 0)
            {
                this.settings = JsonConvert.DeserializeObject<SettingsModel>(json);
            }
                      
            DataContainer.ViewModel.OverlayInitDelay = this.settings.OverlayInitDelay;
            DataContainer.ViewModel.MinBackgroundDistance = this.settings.MinBackgroundDistance;
            DataContainer.ViewModel.BackgroundDistance = this.settings.BackgroundDistance;

            if (this.settings.IsImagesChecked == true)
            {
                Checkbox_Images.IsChecked = true;
            }
            else
            {
                Checkbox_Videos.IsChecked = true;
            }

            if (this.settings.IsLandscape)
            {
                Landscape.IsChecked = true;
            }
            else
            {
                Portrait.IsChecked = true;
            }            

            //    ResizeSelectedImages();

            DataContext = DataContainer.ViewModel;
        }

        /// <summary>
        /// Load the graphic overlay.  This will only be used on the generated snapshot to be sent.
        /// </summary>
        private void InitializeOverlay()
        {
            Uri uri = new Uri(@"Images\EventLogo.png", UriKind.Relative);
            PngBitmapDecoder decoder = new PngBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            BitmapSource bmps = decoder.Frames[0];

            _Overlay = new WriteableBitmap(bmps);


            uri = new Uri(@"Images\pointer.png", UriKind.Relative);
            decoder = new PngBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            BitmapSource pointer = decoder.Frames[0];

            _pointerBitmap = new WriteableBitmap(pointer);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this._reader != null)
            {
             //   this._reader.MultiSourceFrameArrived += this.Reader_MultiSourceFrameArrived;

                _gestureController = new GestureController();
                _gestureController.GestureRecognized += GestureController_GestureRecognized;
            }

          //  InitializeImageDataFlow();
        }


        /// <summary>
        /// Handles the depth/color/body index frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
//        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
//        {
//            try
//            {
//                var multiSourceFrame = e.FrameReference.AcquireFrame();
//                {

//                    if (multiSourceFrame != null)
//                    {
//                        // MultiSourceFrame is IDisposable

//                        using (var depthFrame = multiSourceFrame.DepthFrameReference.AcquireFrame())
//                        {
//                            using (var colorFrame = multiSourceFrame.ColorFrameReference.AcquireFrame())
//                            {
//                                using (var bodyIndexFrame = multiSourceFrame.BodyIndexFrameReference.AcquireFrame())
//                                {
//                                    using (var bodyFrame = multiSourceFrame.BodyFrameReference.AcquireFrame())
//                                    {

//                                        if ((depthFrame != null) && (colorFrame != null) && (bodyIndexFrame != null))
//                                        {

//                                            ProcessFrames(depthFrame, colorFrame, bodyIndexFrame, bodyFrame);
//                                        }
//                                        else
//                                        {
//#if DEBUG
//                                            Console.WriteLine("--Null:" + DateTime.Now.ToShortTimeString());
//                                            Console.WriteLine("depthFrame:" + (depthFrame == null));
//                                            Console.WriteLine("colorFrame:" + (colorFrame == null));
//                                            Console.WriteLine("bodyIndexFrame:" + (bodyIndexFrame == null));
//#endif
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//#if DEBUG
//                Console.WriteLine(ex);
//#endif
//            }
//        }



//        private void ProcessFrames(DepthFrame depthFrame, ColorFrame colorFrame, BodyIndexFrame bodyIndexFrame, BodyFrame bodyFrame)
//        {



//            FrameDescription depthFrameDescription = depthFrame.FrameDescription;
//            FrameDescription colorFrameDescription = colorFrame.FrameDescription;
//            FrameDescription bodyIndexFrameDescription = bodyIndexFrame.FrameDescription;




//            int bodyIndexWidth = bodyIndexFrameDescription.Width;
//            int bodyIndexHeight = bodyIndexFrameDescription.Height;


//            // The ImageModel object is used to transfer Kinect data into the DataFlow rotunies. 
//            ImageModel imageModel = new ImageModel()
//            {
//                DepthWidth = depthFrameDescription.Width,
//                DepthHeight = depthFrameDescription.Height,
//                ColorWidth = colorFrameDescription.Width,
//                ColorHeight = colorFrameDescription.Height,
//                ShowTrails = DataContainer.ViewModel.LeaveTrails,
//                PersonFill = DataContainer.ViewModel.PersonFill,
//                MaxDistance = DataContainer.ViewModel.BackgroundDistance
//            };
//            imageModel.ColorFrameData = new byte[imageModel.ColorWidth * imageModel.ColorHeight * this.bytesPerPixel];

//            imageModel.DisplayPixels = new byte[_PreviousFrameDisplayPixels.Length];
//            imageModel.BodyIndexFrameData = new byte[imageModel.DepthWidth * imageModel.DepthHeight];
//            imageModel.ColorPoints = new ColorSpacePoint[imageModel.DepthWidth * imageModel.DepthHeight];
//            imageModel.BytesPerPixel = bytesPerPixel;
//            imageModel.Bodies = new Body[this._kinectSensor.BodyFrameSource.BodyCount];
//            bodyFrame.GetAndRefreshBodyData(imageModel.Bodies);
//            imageModel.DepthData = new ushort[imageModel.DepthWidth * imageModel.DepthHeight];

//            depthFrame.CopyFrameDataToArray(imageModel.DepthData);
//            depthFrame.CopyFrameDataToArray(this.DepthFrameData);

//            if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
//            {
//                colorFrame.CopyRawFrameDataToArray(imageModel.ColorFrameData);
//            }
//            else
//            {
//                colorFrame.CopyConvertedFrameDataToArray(imageModel.ColorFrameData, ColorImageFormat.Bgra);
//            }
//            imageModel.PixelFormat = PixelFormats.Bgra32;



//            _ColorBitmap.WritePixels(new Int32Rect(0, 0, imageModel.ColorWidth, imageModel.ColorHeight),
//                                          imageModel.ColorFrameData,
//                                          imageModel.ColorWidth * imageModel.BytesPerPixel,
//                                          0);


//            //RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)CompositeImage.ActualWidth, (int)CompositeImage.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
//            //DrawingVisual dv = new DrawingVisual();
//            //VisualBrush brush = new VisualBrush(CompositeImage);

//            //foreach(Body body in _bodies)
//            //{
//            //    if (body.IsTracked)
//            //    {
//            //        Joint joint = body.Joints[JointType.HandRight];
//            //        using (DrawingContext dc = dv.RenderOpen())
//            //        {

//            //            dc.DrawRectangle(brush, null, new Rect(new Point(), new Size(CompositeImage.ActualWidth, CompositeImage.ActualHeight)));
//            //            ImageBrush brush2 = new ImageBrush(_pointerBitmap);
//            //            brush2.Opacity = 1.0;
//            //            dc.DrawRectangle(brush2, null, new Rect(new Point(0, CompositeImage.ActualHeight - _Overlay.Height), new Size(_pointerBitmap.Width, _pointerBitmap.Height)));
//            //        }
//            //    }
//            //}

//            //ConvertIRDataToByte();






//            ImagePreview.Source = _ColorBitmap;


//            bodyIndexFrame.CopyFrameDataToArray(imageModel.BodyIndexFrameData);

//            this._coordinateMapper.MapDepthFrameToColorSpace(DepthFrameData, imageModel.ColorPoints);

//            if (DataContainer.ViewModel.LeaveTrails)
//            {
//                Array.Copy(this._PreviousFrameDisplayPixels, imageModel.DisplayPixels, this._PreviousFrameDisplayPixels.Length);
//            }


//            try
//            {
//                //Send the imageModel to the DataFlow transformer
//                _ImageTransformer.Post(imageModel);
//            }
//            catch (Exception ex)
//            {
//#if DEBUG
//                Console.WriteLine(ex);
//#endif
//            }


//        }

        /// <summary>
        /// This method will initialize the data flow functionality.  Data flow is a NuGet package and 
        /// </summary>
        //private void InitializeImageDataFlow()
        //{
        //    InitializeImageTransformer();
        //    InitializeImageWriter();
        //    _ImageTransformer.LinkTo(_ImageWriterActionBlock);


        //    //InitializeBodyTransformer(); // Transform Block for future image processing.
        //    //_ImageTransformer.LinkTo(_BodyTransformer);
        //    //_BodyTransformer.LinkTo(_ImageWriterActionBlock);


        //}


        /// <summary>
        /// Transform will iterate over each pixel and only will copy ones that belong to a tracked person.
        /// This is all done in a TransformBlock, which may be using a seperate Task.  The result will be returned to an ActionBlock for addition processing.
        /// </summary>
        //private void InitializeImageTransformer()
        //{
        //    _ImageTransformer = new TransformBlock<ImageModel, ImageModel>(imageModel =>
        //    {
        //        for (int y = 0; y < imageModel.DepthHeight; ++y)
        //        {
        //            for (int x = 0; x < imageModel.DepthWidth; ++x)
        //            {
        //                // calculate index into depth array
        //                int depthIndex = (y * imageModel.DepthWidth) + x;

        //                byte player = imageModel.BodyIndexFrameData[depthIndex];

        //                // if we're tracking a player for the current pixel, sets its color and alpha to full
        //                if ((double)imageModel.DepthData[depthIndex] <= imageModel.MaxDistance)
        //                {
        //                    if (player != 0xff)
        //                    {
        //                        // retrieve the depth to color mapping for the current depth pixel
        //                        ColorSpacePoint colorPoint = imageModel.ColorPoints[depthIndex];

        //                        // make sure the depth pixel maps to a valid point in color space
        //                        int colorX = (int)Math.Floor(colorPoint.X + 0.5);
        //                        int colorY = (int)Math.Floor(colorPoint.Y + 0.5);

        //                        if ((colorX >= 0) && (colorX < imageModel.ColorWidth) && (colorY >= 0) && (colorY < imageModel.ColorHeight))
        //                        {
        //                            // calculate index into color array
        //                            int colorIndex = ((colorY * imageModel.ColorWidth) + colorX) * imageModel.BytesPerPixel;

        //                            int displayIndex = depthIndex * imageModel.BytesPerPixel;
        //                            if (imageModel.PersonFill)
        //                            {

        //                                imageModel.DisplayPixels[displayIndex] = colorArray[Convert.ToInt16(player)].R;
        //                                imageModel.DisplayPixels[displayIndex + 1] = colorArray[Convert.ToInt16(player)].G;
        //                                imageModel.DisplayPixels[displayIndex + 2] = colorArray[Convert.ToInt16(player)].B;
        //                                imageModel.DisplayPixels[displayIndex + 3] = (byte)rnd.Next(215, 255);

        //                            }
        //                            else
        //                            {
        //                                // set source for copy to the color pixel

        //                                imageModel.DisplayPixels[displayIndex] = imageModel.ColorFrameData[colorIndex];
        //                                imageModel.DisplayPixels[displayIndex + 1] = imageModel.ColorFrameData[colorIndex + 1];
        //                                imageModel.DisplayPixels[displayIndex + 2] = imageModel.ColorFrameData[colorIndex + 2];
        //                                imageModel.DisplayPixels[displayIndex + 3] = 0xFF;
        //                            }

        //                        }
        //                    }
        //                }

        //            }
        //        }


        //        return imageModel;
        //    });
        //}

        void GestureController_GestureRecognized(object sender, GestureEventArgs e)
        {
            tblGestures.Text = e.GestureType.ToString();
        }


        // This transform block will be used in a future version of the applicaton
        //private void InitializeBodyTransformer()
        //{
        //    _BodyTransformer = new TransformBlock<ImageModel, ImageModel>(imageModel =>
        //    {


        //        foreach (Body body in imageModel.Bodies)
        //        {
        //            if (body.IsTracked)
        //            {
        //                Joint j = body.Joints[JointType.HandRight];
        //                if (j.TrackingState.Equals(TrackingState.Tracked))
        //                {

        //                    DepthSpacePoint point = this._kinectSensor.CoordinateMapper.MapCameraPointToDepthSpace(j.Position);
        //                    int x = (int)Math.Floor(point.X + 0.5);
        //                    int y = (int)Math.Floor(point.Y + 0.5);
        //                    if ((x >= 0) && (x < imageModel.ColorWidth) && (y >= 0) && (y < imageModel.ColorHeight))
        //                    {


        //                        Dispatcher.Invoke(() =>
        //                        {

        //                            byte[] temp = new byte[_pointerBitmap.PixelWidth * _pointerBitmap.PixelHeight * 4];
        //                            _pointerBitmap.CopyPixels(temp, _pointerBitmap.BackBufferStride, 0);

        //                            for (int posX = 0; posX < _pointerBitmap.PixelWidth; posX++)
        //                            {
        //                                for (int posY = 0; posY < _pointerBitmap.PixelHeight; posY++)
        //                                {
        //                                    int pointerPosition = ((_pointerBitmap.PixelWidth * posY) + posX) * 4;
        //                                    int imagePosition = (((y + posY) * imageModel.DepthWidth) + (x + posX)) * 4;

        //                                    imageModel.DisplayPixels[imagePosition] = temp[pointerPosition];
        //                                    imageModel.DisplayPixels[imagePosition + 1] = temp[pointerPosition + 1];
        //                                    imageModel.DisplayPixels[imagePosition + 2] = temp[pointerPosition + 2];
        //                                    imageModel.DisplayPixels[imagePosition + 3] = temp[pointerPosition + 3];


        //                                }

        //                            }




        //                        });

        //                    }
        //                }
        //            }
        //        }


        //        return imageModel;
        //    });
        //}



        /// <summary>
        /// This defines the ActionBlock, of DataFlow.  It may be performed in a seperate task.   This ActionBlock is linked to a TransformBlock.  Each "Block" may be
        /// managed in its own Task.
        /// </summary>
//        private void InitializeImageWriter()
//        {
//            _ImageWriterActionBlock = new ActionBlock<ImageModel>(imageModel =>
//            {
//                try
//                {
//                    Dispatcher.Invoke(() =>
//                    {
//                        //Hold pixels to serve as previous frame
//                        Array.Copy(imageModel.DisplayPixels, this._PreviousFrameDisplayPixels, this._PreviousFrameDisplayPixels.Length);

//                        DataContainer.ViewModel.Bitmap.WritePixels(
//                                          new Int32Rect(0, 0, imageModel.DepthWidth, imageModel.DepthHeight),
//                                          imageModel.DisplayPixels,
//                                          imageModel.DepthWidth * imageModel.BytesPerPixel,
//                                          0);
//                        //_vm.NewImageSource = BlendImages(_vm.Bitmap, _Overlay, (int)CompositeImage.ActualWidth, (int)CompositeImage.ActualHeight,1.0);
//                    });
//                    imageModel.Dispose();


//                }
//                catch (Exception ex)
//                {
//#if DEBUG
//                    Console.WriteLine(ex);
//#endif
//                }
//            });
//        }
 

        private void ListView_BackgroundChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                var bgModel = e.AddedItems[0] as BackgroundModel;
                var type = bgModel.ImagePath.Substring(bgModel.ImagePath.Length - 3);

                if(type == "jpg" || type == "png")
                {
                    if (DataContainer.ViewModel.IsLandscape)
                    {
                        var resizedImage = DataContainer.ViewModel.ResizeImage(bgModel.ImagePath, 600, 600, true);
                        Backdrop.Source = resizedImage;

                        var resizedImage2 = DataContainer.ViewModel.ResizeImage(bgModel.ImagePath, 200, 200, true);
                        BackgroundDrop.Source = resizedImage;
                    }
                    else
                    {
                        var resizedImage = DataContainer.ViewModel.ResizeImage(bgModel.ImagePath, 600, 600, false);
                        Backdrop.Source = resizedImage;

                        var resizedImage2 = DataContainer.ViewModel.ResizeImage(bgModel.ImagePath, 200, 200, false);
                        BackgroundDrop.Source = resizedImage;
                    }
                }

                DataContainer.ViewModel.SelectedBackground = bgModel;               
            }
        }

        private void ListView_OverlayChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                var overlayModel = e.AddedItems[0] as OverlayModel;

                if (DataContainer.ViewModel.IsLandscape)
                {
                    var resizedImage = DataContainer.ViewModel.ResizeImage(overlayModel.ImagePath, 200, 200, true);
                    InitOverlayDrop.Source = resizedImage;
                }
                else
                {
                    var resizedImage = DataContainer.ViewModel.ResizeImage(overlayModel.ImagePath, 200, 200, false);
                    InitOverlayDrop.Source = resizedImage;
                }

                DataContainer.ViewModel.SelectedOverlay = overlayModel;
            }
        }

        private void ListView_EndOverlayChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                var overlayModel = e.AddedItems[0] as OverlayModel;

                if (DataContainer.ViewModel.IsLandscape)
                {
                    var resizedImage = DataContainer.ViewModel.ResizeImage(overlayModel.ImagePath, 200, 200, true);
                    EndOverlayDrop.Source = resizedImage;
                }
                else
                {
                    var resizedImage = DataContainer.ViewModel.ResizeImage(overlayModel.ImagePath, 200, 200, false);
                    EndOverlayDrop.Source = resizedImage;
                }

                DataContainer.ViewModel.SelectedEndOverlay = overlayModel;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
        } 

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            }
        }

        private void cbSettings_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new Settings();
            settingsWindow.ShowDialog();
        }

        private void btnCount_Click(object sender, RoutedEventArgs e)
        {
            var countdownWindow = new CountDownConfig();
            countdownWindow.ShowDialog();
        }

        private void cbCountDownConfig_Click(object sender, RoutedEventArgs e)
        {
            var countdownWindow = new CountDownConfig();
            countdownWindow.ShowDialog();
        }

        private void cbOutputImages_Click(object sender, RoutedEventArgs e)
        {
            var outputImages = new OutputImagesConfig();
            outputImages.ShowDialog();

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var background = new BackgroundImagesConfig();
            background.Show();
            //DataContainer.ViewModel.LoadBackgrounds();
        }

        private void cbFacebook_Click(object sender, RoutedEventArgs e)
        {
            var fbAdmins = new AddFB();
            fbAdmins.Show();
        }

        private void ShowInitOverlay_Click(object sender, RoutedEventArgs e)
        {
            var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\KinectPhotobooth\Images\InitOverlays"));
            Process.Start(path);
        }

        private void ShowEndOverlay_Click(object sender, RoutedEventArgs e)
        {
            var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\KinectPhotobooth\Images\EndOverlays"));
            Process.Start(path);
        }

        private void ShowBackgrounds_Click(object sender, RoutedEventArgs e)
        {
            var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\KinectPhotobooth\Images\Backgrounds"));
            Process.Start(path);
        }

        private void RefreshEndOverlays(object sender, RoutedEventArgs e)
        {
            DataContainer.ViewModel.LoadBackgrounds(this._endOverlaysPath, "end_overlays");
        }

        private void RefreshInitOverlays(object sender, RoutedEventArgs e)
        {
            DataContainer.ViewModel.LoadBackgrounds(this._overlaysPath, "init_overlays");
        }

        private void RefreshBackgrounds(object sender, RoutedEventArgs e)
        {
            if (DataContainer.ViewModel.IsImagesChecked)
            {
                DataContainer.ViewModel.LoadBackgrounds(this._backgroundsPath, "backgrounds");
            }
            else
            {
                DataContainer.ViewModel.LoadBackgrounds(this._backgroundsPath, "videos");
            }
        }

        private void ImagesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this._backgroundsPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\KinectPhotobooth\Images\Backgrounds\Images"));
            DataContainer.ViewModel.BackgroundPath = this._backgroundsPath;

            Checkbox_Videos.IsChecked = false;
            Checkbox_Videos.IsEnabled = true;

            Checkbox_Images.IsEnabled = false;

            BackVideoDrop.Visibility = Visibility.Hidden;
            BgVideoDrop.Visibility = Visibility.Hidden;

            Backdrop.Visibility = Visibility.Visible;
            BackgroundDrop.Visibility = Visibility.Visible;

            if(this.settings.IsImagesChecked == false)
            {              
                DataContainer.ViewModel.LoadBackgrounds(this._backgroundsPath, "backgrounds");
                this.settings.IsImagesChecked = true;
                DataContainer.ViewModel.IsImagesChecked = true;
            }          

            if (DataContainer.ViewModel.Backgrounds.Count == 0)
            {
                DataContainer.ViewModel.LoadBackgrounds(this._backgroundsPath, "backgrounds");
                DataContainer.ViewModel.IsImagesChecked = true;
            }
        }

        private void VideosCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this._backgroundsPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\KinectPhotobooth\Images\Backgrounds\Videos"));
            DataContainer.ViewModel.BackgroundPath = this._backgroundsPath;

            Checkbox_Images.IsChecked = false;
            Checkbox_Images.IsEnabled = true;

            Checkbox_Videos.IsEnabled = false;

            Backdrop.Visibility = Visibility.Hidden;
            BackgroundDrop.Visibility = Visibility.Hidden;

            BackVideoDrop.Visibility = Visibility.Visible;
            BgVideoDrop.Visibility = Visibility.Visible;

            //Load Videos
            if (this.settings.IsImagesChecked == true)
            {                
                DataContainer.ViewModel.LoadBackgrounds(this._backgroundsPath, "videos");
                DataContainer.ViewModel.IsImagesChecked = false;
                this.settings.IsImagesChecked = false;
            }

            if(DataContainer.ViewModel.Backgrounds.Count == 0)
            {
                DataContainer.ViewModel.LoadBackgrounds(this._backgroundsPath, "videos");
                DataContainer.ViewModel.IsImagesChecked = false;
            }
        }

        private void LandscapeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Portrait.IsChecked = false;
            Portrait.IsEnabled = true;

            Landscape.IsEnabled = false;

            DataContainer.ViewModel.IsLandscape = true;

            if (DataContainer.ViewModel.SelectedBackground != null)
            {
                var resizedBackground = DataContainer.ViewModel.ResizeImage(DataContainer.ViewModel.SelectedBackground.ImagePath, 200, 200, true);
                BackgroundDrop.Source = resizedBackground;

                var resizedBackground2 = DataContainer.ViewModel.ResizeImage(DataContainer.ViewModel.SelectedBackground.ImagePath, 600, 600, true);
                Backdrop.Source = resizedBackground2;
            }
            
            if(DataContainer.ViewModel.SelectedOverlay != null)
            {
                var resizedOverlay = DataContainer.ViewModel.ResizeImage(DataContainer.ViewModel.SelectedOverlay.ImagePath, 200, 200, true);
                InitOverlayDrop.Source = resizedOverlay;
            }

            if (DataContainer.ViewModel.SelectedEndOverlay != null)
            {
                var resizedEndOverlay = DataContainer.ViewModel.ResizeImage(DataContainer.ViewModel.SelectedEndOverlay.ImagePath, 200, 200, true);
                EndOverlayDrop.Source = resizedEndOverlay;
            }

        }

        private void PortraitCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Landscape.IsChecked = false;
            Landscape.IsEnabled = true;

            Portrait.IsEnabled = false;

            DataContainer.ViewModel.IsLandscape = false;

            if(DataContainer.ViewModel.SelectedBackground != null)
            {
                var resizedBackground = DataContainer.ViewModel.ResizeImage(DataContainer.ViewModel.SelectedBackground.ImagePath, 200, 200, false);
                BackgroundDrop.Source = resizedBackground;

                var resizedBackground2 = DataContainer.ViewModel.ResizeImage(DataContainer.ViewModel.SelectedBackground.ImagePath, 600, 600, false);
                Backdrop.Source = resizedBackground2;
            }

            if(DataContainer.ViewModel.SelectedOverlay != null)
            {
                var resizedOverlay = DataContainer.ViewModel.ResizeImage(DataContainer.ViewModel.SelectedOverlay.ImagePath, 200, 200, false);
                InitOverlayDrop.Source = resizedOverlay;
            }
            
            if(DataContainer.ViewModel.SelectedEndOverlay != null)
            {
                var resizedEndOverlay = DataContainer.ViewModel.ResizeImage(DataContainer.ViewModel.SelectedEndOverlay.ImagePath, 200, 200, false);
                EndOverlayDrop.Source = resizedEndOverlay;
            }         
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BackVideoDrop_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackVideoDrop.Position = TimeSpan.Zero;
        }

        private void ResizeSelectedImages()
        {
            var overlayPath = DataContainer.ViewModel.SelectedOverlay.ImagePath;
            var overlayEndPath = DataContainer.ViewModel.SelectedEndOverlay.ImagePath;

            if(DataContainer.ViewModel.IsLandscape)
            {
                var resizedOverlayImage = DataContainer.ViewModel.ResizeImage(overlayPath, 200, 200, true);
                var resizedEndOverlayImage = DataContainer.ViewModel.ResizeImage(overlayEndPath, 200, 200, true);

                InitOverlayDrop.Source = resizedOverlayImage;
                EndOverlayDrop.Source = resizedEndOverlayImage;
            }
            else
            {
                var resizedOverlayImage = DataContainer.ViewModel.ResizeImage(overlayPath, 200, 200, false);
                var resizedEndOverlayImage = DataContainer.ViewModel.ResizeImage(overlayEndPath, 200, 200, false);

                InitOverlayDrop.Source = resizedOverlayImage;
                EndOverlayDrop.Source = resizedEndOverlayImage;
            }                   
        }
    }
}
