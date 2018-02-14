
namespace KinectPhotobooth
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
    using KinectPhotobooth.ViewModels;
    using System.Windows.Input;
    using System.Threading.Tasks.Dataflow;
    using KinectPhotobooth.Models;
    using Microsoft.Kinect.Wpf.Controls;
    using KinectPhotobooth.Common;
    using System.Windows.Threading;
    using System.Windows.Media.Animation;
    using Newtonsoft.Json;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow : Window
    {
        private Cursor _previousCursor;
        /// <summary>
        /// Indicates opaque in an opacity mask
        /// </summary>
        private const int OpaquePixel = -1;

        /// <summary>
        /// Size of the RGB pixel in the bitmap
        /// </summary>
        private readonly int bytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper coordinateMapper = null;

        /// <summary>
        /// Reader for depth/color/body index frames
        /// </summary>
        private MultiSourceFrameReader reader = null;



        /// <summary>
        /// Intermediate storage for pixes once frame is displayed 
        /// </summary>
        private byte[] _PreviousFrameDisplayPixels = null;


        private ushort[] DepthFrameData { get; set; }

        private ActionBlock<ImageModel> _ImageWriterActionBlock;
        private TransformBlock<ImageModel, ImageModel> _ImageTransformer;
        private TransformBlock<ImageModel, ImageModel> _BodyTransformer;

        private WriteableBitmap _Overlay;
        private WriteableBitmap _pointerBitmap;

        private MainWindowViewModel _vm;

        private WriteableBitmap _ColorBitmap;
        //private WriteableBitmap _IRBitmap;

        private Random rnd = new Random();

        private Color[] colorArray;

        //Countdown Timer
        private int counter;
        private DispatcherTimer countdownTimer;

        private SettingsModel settings;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            _vm = new MainWindowViewModel();

            colorArray = new Color[6];
            colorArray[0] = Colors.Blue;
            colorArray[1] = Colors.Red;
            colorArray[2] = Colors.Green;
            colorArray[3] = Colors.Yellow;
            colorArray[4] = Colors.Orange;
            colorArray[5] = Colors.HotPink;


            //  this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
            
            //Fetch the event overlay 
            InitializeOverlay();            

            _vm.ImagePhotoDirectory = @"Images/Screenshots/";

            // for Alpha, one sensor is supported
            this.kinectSensor = KinectSensor.GetDefault();


            if (this.kinectSensor != null)
            {
                // get the coordinate mapper
                this.coordinateMapper = this.kinectSensor.CoordinateMapper;

                // open the sensor
                this.kinectSensor.Open();

                FrameDescription depthFrameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;

                int depthWidth = depthFrameDescription.Width;
                int depthHeight = depthFrameDescription.Height;
                DepthFrameData = new ushort[depthWidth * depthHeight];
                //_IRRawData = new ushort[kinectSensor.InfraredFrameSource.FrameDescription.Width * kinectSensor.InfraredFrameSource.FrameDescription.Height];
                //_IRConvertedData = new byte[kinectSensor.InfraredFrameSource.FrameDescription.Width * 4 * kinectSensor.InfraredFrameSource.FrameDescription.Height];


                // allocate space to put the pixels being received and converted


                this._PreviousFrameDisplayPixels = new byte[depthWidth * depthHeight * this.bytesPerPixel];


                // create the bitmap to display
                _vm.Bitmap = new WriteableBitmap(depthWidth, depthHeight, 96.0, 96.0, PixelFormats.Bgra32, null);

                _ColorBitmap = new WriteableBitmap(kinectSensor.ColorFrameSource.FrameDescription.Width, kinectSensor.ColorFrameSource.FrameDescription.Height, 96, 96, PixelFormats.Bgr32, null);


                FrameDescription colorFrameDescription = this.kinectSensor.ColorFrameSource.FrameDescription;

                int colorWidth = colorFrameDescription.Width;
                int colorHeight = colorFrameDescription.Height;
                

                // Create the Multisource reader and the types of streams it will use.
                // For this app, we will use Depth, Color, BodyIndex, Infrared, and Body
                this.reader = this.kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Depth |
                                                                            FrameSourceTypes.Color |
                                                                            FrameSourceTypes.BodyIndex |
                                                                            FrameSourceTypes.Infrared |
                                                                            FrameSourceTypes.Body);

                // set the status text
                _vm.StatusText = Properties.Resources.InitializingStatusTextFormat;

            }
            else
            {
                // on failure, set the status text
                _vm.StatusText = Properties.Resources.NoSensorStatusText;
            }

            // initialize the components (controls) of the window
            this.InitializeComponent();

            //Hide mouse cursor
            //this.Cursor = Cursors.None;


            KinectRegion.SetKinectRegion(this, kinectRegion);

            App app = ((App)Application.Current);
            app.KinectRegion = kinectRegion;

            // Use the default sensor
            this.kinectRegion.KinectSensor = KinectSensor.GetDefault();

            //Read data from settings.json
            this.settings = new SettingsModel();
            string json = File.ReadAllText(Directory.GetCurrentDirectory() + "/Settings/settings.json");
            try
            {
                this.settings = JsonConvert.DeserializeObject<SettingsModel>(json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("A handled exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close();
            }


            if (this.settings != null)
            {
                this._vm.OverlayInitDelay = this.settings.OverlayInitDelay;
                this._vm.MinBackgroundDistance = this.settings.MinBackgroundDistance;
                this._vm.BackgroundDistance = this.settings.BackgroundDistance;
            }          

            BackgroundTypeChecking();
            
            DataContext = _vm;            
        }

        private void MyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {

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


        /// <summary>
        /// Execute start up tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.reader != null)
            {
                this.reader.MultiSourceFrameArrived += this.Reader_MultiSourceFrameArrived;
            }

            InitializeImageDataFlow();
        }

        private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                DataContainer.ViewModel.SelectedBackground = e.AddedItems[0] as BackgroundModel;
            }
        }

        #region ImageDataFlow
        /// <summary>
        /// I'm using DataFlow as a way to try processing different aspects of the images in different tasks.   DataFlow is a NuGet package and is not part of the core .net api
        /// </summary>


        /// <summary>
        /// This method will initialize the data flow functionality.  Data flow is a NuGet package and 
        /// </summary>
        private void InitializeImageDataFlow()
        {
            InitializeImageTransformer();
            InitializeImageWriter();
            _ImageTransformer.LinkTo(_ImageWriterActionBlock);          

            //InitializeBodyTransformer(); // Transform Block for future image processing.
            //_ImageTransformer.LinkTo(_BodyTransformer);
            //_BodyTransformer.LinkTo(_ImageWriterActionBlock);
        }



        /// <summary>
        /// Transform will iterate over each pixel and only will copy ones that belong to a tracked person.
        /// This is all done in a TransformBlock, which may be using a seperate Task.  The result will be returned to an ActionBlock for addition processing.
        /// </summary>
        private void InitializeImageTransformer()
        {
            _ImageTransformer = new TransformBlock<ImageModel, ImageModel>(imageModel =>
            {
                for (int y = 0; y < imageModel.DepthHeight; ++y)
                {
                    for (int x = 0; x < imageModel.DepthWidth; ++x)
                    {
                        // calculate index into depth array
                        int depthIndex = (y * imageModel.DepthWidth) + x;

                        byte player = imageModel.BodyIndexFrameData[depthIndex];

                        // if we're tracking a player for the current pixel, sets its color and alpha to full
                        if ((double)imageModel.DepthData[depthIndex] <= imageModel.MaxDistance && (double)imageModel.DepthData[depthIndex] >= imageModel.MinDistance)
                        {
                            

                            if (player != 0xff)
                            {

                                // retrieve the depth to color mapping for the current depth pixel
                                ColorSpacePoint colorPoint = imageModel.ColorPoints[depthIndex];

                                // make sure the depth pixel maps to a valid point in color space
                                int colorX = (int)Math.Floor(colorPoint.X + 0.5);
                                int colorY = (int)Math.Floor(colorPoint.Y + 0.5);

                                if ((colorX >= 0) && (colorX < imageModel.ColorWidth) && (colorY >= 0) && (colorY < imageModel.ColorHeight))
                                {
                                    // calculate index into color array
                                    int colorIndex = ((colorY * imageModel.ColorWidth) + colorX) * imageModel.BytesPerPixel;

                                    int displayIndex = depthIndex * imageModel.BytesPerPixel;
                                    if (imageModel.PersonFill)
                                    {
                                        imageModel.DisplayPixels[displayIndex] = colorArray[Convert.ToInt16(player)].R;
                                        imageModel.DisplayPixels[displayIndex + 1] = colorArray[Convert.ToInt16(player)].G;
                                        imageModel.DisplayPixels[displayIndex + 2] = colorArray[Convert.ToInt16(player)].B;
                                        imageModel.DisplayPixels[displayIndex + 3] = (byte)rnd.Next(215, 255);
                                    }
                                    else
                                    {
                                        // set source for copy to the color pixel
                                        imageModel.DisplayPixels[displayIndex] = imageModel.ColorFrameData[colorIndex];
                                        imageModel.DisplayPixels[displayIndex + 1] = imageModel.ColorFrameData[colorIndex + 1];
                                        imageModel.DisplayPixels[displayIndex + 2] = imageModel.ColorFrameData[colorIndex + 2];
                                        imageModel.DisplayPixels[displayIndex + 3] = 0xFF;
                                    }

                                }
                            }
                        }
                    }
                }

                return imageModel;
            });
        }



        // This transform block will be used in a future version of the applicaton
        private void InitializeBodyTransformer()
        {
            _BodyTransformer = new TransformBlock<ImageModel, ImageModel>(imageModel =>
            {
                foreach (Body body in imageModel.Bodies)
                {
                    if (body.IsTracked)
                    {
                        Joint j = body.Joints[JointType.HandRight];
                        if (j.TrackingState.Equals(TrackingState.Tracked))
                        {

                            DepthSpacePoint point = this.kinectSensor.CoordinateMapper.MapCameraPointToDepthSpace(j.Position);
                            int x = (int)Math.Floor(point.X + 0.5);
                            int y = (int)Math.Floor(point.Y + 0.5);
                            if ((x >= 0) && (x < imageModel.ColorWidth) && (y >= 0) && (y < imageModel.ColorHeight))
                            {


                                Dispatcher.Invoke(() =>
                                {

                                    byte[] temp = new byte[_pointerBitmap.PixelWidth * _pointerBitmap.PixelHeight * 4];
                                    _pointerBitmap.CopyPixels(temp, _pointerBitmap.BackBufferStride, 0);

                                    for (int posX = 0; posX < _pointerBitmap.PixelWidth; posX++)
                                    {
                                        for (int posY = 0; posY < _pointerBitmap.PixelHeight; posY++)
                                        {
                                            int pointerPosition = ((_pointerBitmap.PixelWidth * posY) + posX) * 4;
                                            int imagePosition = (((y + posY) * imageModel.DepthWidth) + (x + posX)) * 4;

                                            imageModel.DisplayPixels[imagePosition] = temp[pointerPosition];
                                            imageModel.DisplayPixels[imagePosition + 1] = temp[pointerPosition + 1];
                                            imageModel.DisplayPixels[imagePosition + 2] = temp[pointerPosition + 2];
                                            imageModel.DisplayPixels[imagePosition + 3] = temp[pointerPosition + 3];

                                        }
                                    }
                                });
                            }
                        }
                    }
                }

                return imageModel;
            });
        }



        /// <summary>
        /// This defines the ActionBlock, of DataFlow.  It may be performed in a seperate task.   This ActionBlock is linked to a TransformBlock.  Each "Block" may be
        /// managed in its own Task.
        /// </summary>
        private void InitializeImageWriter()
        {
            _ImageWriterActionBlock = new ActionBlock<ImageModel>(imageModel =>
            {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        //Hold pixels to serve as previous frame
                        Array.Copy(imageModel.DisplayPixels, this._PreviousFrameDisplayPixels, this._PreviousFrameDisplayPixels.Length);

                        _vm.Bitmap.WritePixels(
                                          new Int32Rect(0, 0, imageModel.DepthWidth, imageModel.DepthHeight),
                                          imageModel.DisplayPixels,
                                          imageModel.DepthWidth * imageModel.BytesPerPixel,
                                          0);
                        //_vm.NewImageSource = BlendImages(_vm.Bitmap, _Overlay, (int)CompositeImage.ActualWidth, (int)CompositeImage.ActualHeight,1.0);
                    });
                    imageModel.Dispose();

                   
                }
                catch (Exception ex)
                {
#if DEBUG
                    Console.WriteLine(ex);
#endif
                }
            });
        }
        #endregion

        /// <summary>
        /// Returns a blended ImageSource which could be used with an Image XAML tag.  Overlay will automatically be centered onto the primay image.
        /// </summary>
        /// <param name="mainImage">Primary Image</param>
        /// <param name="overlayImage">Overlay Image</param>
        /// <param name="width">Final Resulting width</param>
        /// <param name="height">Final Resulting height</param>
        /// <param name="overlayTransparency">Transparency level of overlay</param>
        /// <returns>ImageSource, suitable for binding with an Image</returns>
        private ImageSource BlendImages(WriteableBitmap mainImage, WriteableBitmap overlayImage, int width, int height, double overlayTransparency)
        {
            //Resulting image containg main mange and overlay
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(width, height, 96.0, 96.0, PixelFormats.Pbgra32);

            DrawingVisual dv = new DrawingVisual();
            
            using (DrawingContext dc = dv.RenderOpen())
            {
                
                ImageBrush mainImageBrush = new ImageBrush(mainImage);
                mainImageBrush.Opacity = 1.0;
                dc.DrawRectangle(mainImageBrush, null, new Rect(new Point(), new Size(width, height)));
                
                //ImageBrush overlayImageBrush = new ImageBrush(overlayImage);
                //overlayImageBrush.Opacity = overlayTransparency;

                //Calculate the starting point for the overlay.
                Point overlayPoint = new Point(
                   (width - overlayImage.Width) / 2,
                   (height - overlayImage.Height) / 2
                    );

               // dc.DrawRectangle(overlayImageBrush, null, new Rect(overlayPoint, new Size(overlayImage.Width, overlayImage.Height)));

                dc.Close();
            }
            renderBitmap.Render(dv);
            return renderBitmap;
        }


        #region Main Window Closing
        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.reader != null)
            {
                // MultiSourceFrameReder is IDisposable
                this.reader.Dispose();
                this.reader = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }

            if (_ImageTransformer != null)
            {
                _ImageTransformer.Complete();
            }

            if (_ImageWriterActionBlock != null)
            {
                _ImageWriterActionBlock.Complete();
            }
        }
        #endregion

        #region ScreenShotButton
        /// <summary>
        /// Handles the user clicking on the screenshot button
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void ScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            this.countdownTimer = new DispatcherTimer();
         
            _previousCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;

            Fadeout_ScreenshotButton(new TimeSpan(0, 0, 0, 0, 500));

            this.counter = 3;

            Countdown.Visibility = Visibility.Visible;
            this.countdownTimer.Interval = TimeSpan.FromSeconds(1); // 1 second
            this.countdownTimer.Tick += countdownTimer_Tick;
            this.countdownTimer.Start();

            Mouse.OverrideCursor = _previousCursor;
        }

        private void TakePhoto()
        {
            // create a render target that we'll render our composite control to
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(1920, 1080, 96.0, 96.0, PixelFormats.Pbgra32);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush brush = new VisualBrush(CompositeImage);
                dc.DrawRectangle(brush, null, new Rect(new Point(), new Size(1920, 1080)));

                //ImageBrush brush2 = new ImageBrush(_Overlay);
                //brush2.Opacity = .9;
                //dc.DrawRectangle(brush2, null, new Rect(new Point(0, CompositeImage.ActualHeight - _Overlay.Height), new Size(_Overlay.Width, _Overlay.Height)));

            }

            renderBitmap.Render(dv);

            // create a png bitmap encoder which knows how to save a .png file
            BitmapEncoder encoder = new PngBitmapEncoder();
            BitmapEncoder encoder_email = new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            encoder_email.Frames.Add(BitmapFrame.Create(renderBitmap));

            var timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");

            var fileName = String.Format("KinectPhotobooth-{0}-{1}-{2}.png", _vm.EmailAddress, _vm.ContactMe.ToString(), timestamp);

            var path = Path.Combine(@"..\..\..\KinectPhotobooth\Images\Screenshots\", fileName);

            //FadeOutImages();

            // write the new file to disk
            try
            {
                // FileStream is IDisposable
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    encoder.Save(fs);
                }

                encoder = null;
                
                var screenshotPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\KinectPhotobooth\Images\Screenshots\" + fileName));

                Fadeout_Photo(new TimeSpan(0, 0, 0, 0, 500), screenshotPath);
               
                _vm.SendMail(encoder_email);

                _vm.StatusText = string.Format(Properties.Resources.SavedScreenshotStatusTextFormat, path);

                this.countdownTimer = new DispatcherTimer();
                this.counter = this._vm.OverlayInitDelay;

                this.countdownTimer.Interval = TimeSpan.FromSeconds(1); // 1 second
                this.countdownTimer.Tick += countdownTimer_Tick2;
                this.countdownTimer.Start();

            }
            catch (IOException)
            {
                _vm.StatusText = string.Format(Properties.Resources.FailedScreenshotStatusTextFormat, path);
            }
        }

        #endregion

        private void countdownTimer_Tick(object sender, EventArgs e)
        {
            Countdown.Source = new BitmapImage(new Uri(@"\Images\Countdown\Countdown_" + this.counter + ".png", UriKind.Relative));
            this.counter--;
            if (counter == -1)
            {               
                this.countdownTimer.Stop();
                Countdown.Visibility = Visibility.Hidden;
                TakePhoto();
            }
        }

        private void countdownTimer_Tick2(object sender, EventArgs e)
        {
            this.counter--;
            if (counter == -1)
            {
                this.countdownTimer.Stop();
                Fadein_Photo(new TimeSpan(0, 0, 0, 2));
            }
        }


        #region Multisource Frame has arrived
        /// <summary>
        /// Handles the depth/color/body index frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            try
            {
                var multiSourceFrame = e.FrameReference.AcquireFrame();
                {

                    if (multiSourceFrame != null)
                    {
                        // MultiSourceFrame is IDisposable

                        using (var depthFrame = multiSourceFrame.DepthFrameReference.AcquireFrame())
                        {
                            using (var colorFrame = multiSourceFrame.ColorFrameReference.AcquireFrame())
                            {
                                using (var bodyIndexFrame = multiSourceFrame.BodyIndexFrameReference.AcquireFrame())
                                {
                                    using (var bodyFrame = multiSourceFrame.BodyFrameReference.AcquireFrame())
                                    {
                                        var _bodies = new Body[bodyFrame.BodyFrameSource.BodyCount];

                                        bodyFrame.GetAndRefreshBodyData(_bodies);

                                        foreach(var body in _bodies)
                                        {

                                            if (body.IsTracked)
                                            {
                                                if (InitOverlayImage.IsVisible)
                                                {
                                                    Fadeout_InitOverlayImage(new TimeSpan(0, 0, 0, 0, 300));
                                                }
                                            }
                                            else
                                            {
                                                if (!InitOverlayImage.IsVisible)
                                                {
                                                    Fadein_InitOverlayImage(new TimeSpan(0, 0, 0, 0, 500));
                                                }                                             
                                            }
                                        }

                                        if ((depthFrame != null) && (colorFrame != null) && (bodyIndexFrame != null))
                                        {
                                            ProcessFrames(depthFrame, colorFrame, bodyIndexFrame, bodyFrame);
                                        }
                                        else
                                        {
#if DEBUG
                                            Console.WriteLine("--Null:" + DateTime.Now.ToShortTimeString());
                                            Console.WriteLine("depthFrame:" + (depthFrame == null));
                                            Console.WriteLine("colorFrame:" + (colorFrame == null));
                                            Console.WriteLine("bodyIndexFrame:" + (bodyIndexFrame == null));
#endif
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex);
#endif
            }
        }



        private void ProcessFrames(DepthFrame depthFrame, ColorFrame colorFrame, BodyIndexFrame bodyIndexFrame, BodyFrame bodyFrame)
        {
            FrameDescription depthFrameDescription = depthFrame.FrameDescription;
            FrameDescription colorFrameDescription = colorFrame.FrameDescription;
            FrameDescription bodyIndexFrameDescription = bodyIndexFrame.FrameDescription;

            int bodyIndexWidth = bodyIndexFrameDescription.Width;
            int bodyIndexHeight = bodyIndexFrameDescription.Height;


            // The ImageModel object is used to transfer Kinect data into the DataFlow rotunies. 
            ImageModel imageModel = new ImageModel()
            {
                DepthWidth = depthFrameDescription.Width,
                DepthHeight = depthFrameDescription.Height,
                ColorWidth = colorFrameDescription.Width,
                ColorHeight = colorFrameDescription.Height,
                ShowTrails = _vm.LeaveTrails,
                PersonFill = _vm.PersonFill,
                MaxDistance = _vm.BackgroundDistance,
                MinDistance = _vm.MinBackgroundDistance
            };
            imageModel.ColorFrameData = new byte[imageModel.ColorWidth * imageModel.ColorHeight * this.bytesPerPixel];

            imageModel.DisplayPixels = new byte[_PreviousFrameDisplayPixels.Length];
            imageModel.BodyIndexFrameData = new byte[imageModel.DepthWidth * imageModel.DepthHeight];
            imageModel.ColorPoints = new ColorSpacePoint[imageModel.DepthWidth * imageModel.DepthHeight];
            imageModel.BytesPerPixel = bytesPerPixel;
            imageModel.Bodies = new Body[this.kinectSensor.BodyFrameSource.BodyCount];
            bodyFrame.GetAndRefreshBodyData(imageModel.Bodies);
            imageModel.DepthData = new ushort[imageModel.DepthWidth * imageModel.DepthHeight];
            
            depthFrame.CopyFrameDataToArray(imageModel.DepthData);
            depthFrame.CopyFrameDataToArray(this.DepthFrameData);
            
            if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                colorFrame.CopyRawFrameDataToArray(imageModel.ColorFrameData);
            }
            else
            {
                colorFrame.CopyConvertedFrameDataToArray(imageModel.ColorFrameData, ColorImageFormat.Bgra);
            }
            imageModel.PixelFormat = PixelFormats.Bgra32;


            _ColorBitmap.WritePixels(new Int32Rect(0, 0, imageModel.ColorWidth, imageModel.ColorHeight),
                                          imageModel.ColorFrameData,
                                          imageModel.ColorWidth * imageModel.BytesPerPixel,
                                          0);
 

            bodyIndexFrame.CopyFrameDataToArray(imageModel.BodyIndexFrameData);

            this.coordinateMapper.MapDepthFrameToColorSpace(DepthFrameData, imageModel.ColorPoints);

            if (_vm.LeaveTrails)
            {
                Array.Copy(this._PreviousFrameDisplayPixels, imageModel.DisplayPixels, this._PreviousFrameDisplayPixels.Length);
            }


            try
            {
                //Send the imageModel to the DataFlow transformer
                _ImageTransformer.Post(imageModel);
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex);
#endif
            }


        }
        #endregion

        private void GoAdminCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void GoAdminCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var adm = new Admin(_ColorBitmap, this.kinectSensor, this._Overlay, this.coordinateMapper, this.kinectSensor.DepthFrameSource.FrameDescription, this.reader);

            //this.Close();
            adm.Closed += Admin_Closing;
            adm.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var adm = new Admin(_ColorBitmap, this.kinectSensor, this._Overlay, this.coordinateMapper, this.kinectSensor.DepthFrameSource.FrameDescription, this.reader);

            //this.Close();
            adm.Closed += Admin_Closing;
            adm.Show();
        }

        private void Admin_Closing(object sender, EventArgs e)
        {
            this._vm.SelectedBackground = Common.DataContainer.ViewModel.SelectedBackground;
            this._vm.SelectedOverlay = Common.DataContainer.ViewModel.SelectedOverlay;
            this._vm.SelectedEndOverlay = Common.DataContainer.ViewModel.SelectedEndOverlay;
            //  this._vm.LeaveTrails = Common.DataContainer.ViewModel.LeaveTrails;
            //  this._vm.PersonFill = Common.DataContainer.ViewModel.PersonFill;
            //this._vm.EmailAddress = Common.DataContainer.ViewModel.EmailAddress;
            this._vm.BackgroundDistance = Common.DataContainer.ViewModel.BackgroundDistance;
            this._vm.MinBackgroundDistance = Common.DataContainer.ViewModel.MinBackgroundDistance;
            this._vm.OverlayInitDelay = Common.DataContainer.ViewModel.OverlayInitDelay;
            this._vm.IsImagesChecked = Common.DataContainer.ViewModel.IsImagesChecked;

            this.settings = new SettingsModel()
            {
                BackgroundDistance = this._vm.BackgroundDistance,
                MinBackgroundDistance = this._vm.MinBackgroundDistance,
                OverlayInitDelay = this._vm.OverlayInitDelay,
                IsImagesChecked = this._vm.IsImagesChecked
            };

            string jsonObj = JsonConvert.SerializeObject(settings);
            File.WriteAllText(Directory.GetCurrentDirectory() + @"/Settings/settings.json", jsonObj);

            BackgroundTypeChecking();
        }

        private void BackgroundTypeChecking()
        {
            if (this._vm.SelectedBackground != null)
            {
                var imagePath = this._vm.SelectedBackground.ImagePath;
                string type = this._vm.SelectedBackground.ImagePath.Substring(imagePath.Length - 3);

                if (type == "mp4" || type == "wmv")
                {
                    BackVideoDrop.Visibility = Visibility.Visible;
                    Backdrop.Visibility = Visibility.Hidden;
                }
                else
                {
                    BackVideoDrop.Visibility = Visibility.Hidden;
                    Backdrop.Visibility = Visibility.Visible;

                    var resizedImage = DataContainer.ViewModel.ResizeImage(imagePath, 1920, 1080);
                    Backdrop.Source = resizedImage;
                }
            }
        }

        private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if(WindowStyle == WindowStyle.None)
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
            }
            else
            {
                WindowState = WindowState.Maximized;
                WindowStyle = WindowStyle.None;
            }
        }

        private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        public void AdminInfo(BackgroundModel selectedBackground, OverlayModel selectedOverlay, OverlayModel selectedEndOverlay, int backgroundDistance)
        {
            this._vm.SelectedBackground = selectedBackground;
            this._vm.SelectedOverlay = selectedOverlay;
            this._vm.SelectedEndOverlay = selectedEndOverlay;
            this._vm.BackgroundDistance = backgroundDistance;
            //  this._vm.LeaveTrails = Common.DataContainer.ViewModel.LeaveTrails;
            //  this._vm.PersonFill = Common.DataContainer.ViewModel.PersonFill;
            //  this._vm.EmailAddress = Common.DataContainer.ViewModel.EmailAddress;

            this._vm.SelectedBackground = Common.DataContainer.ViewModel.SelectedBackground;
            this._vm.SelectedOverlay = Common.DataContainer.ViewModel.SelectedOverlay;
            this._vm.BackgroundDistance = Common.DataContainer.ViewModel.BackgroundDistance;
        }

        private void Fadeout_InitOverlayImage(TimeSpan fadeTime)
        {
            var fadeOutAnimation = new DoubleAnimation(0d, fadeTime);

            fadeOutAnimation.Completed += (o, e) =>
            {
                InitOverlayImage.Visibility = Visibility.Hidden;
            };

            InitOverlayImage.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void Fadein_InitOverlayImage(TimeSpan fadeTime)
        {
            var fadeInAnimation = new DoubleAnimation(1d, fadeTime);
            
            InitOverlayImage.Opacity = 0d;
            InitOverlayImage.Visibility = Visibility.Visible;                     

            InitOverlayImage.BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        private void Fadeout_Photo(TimeSpan fadeTime, string screenshotPath)
        {
            var fadeInAnimation = new DoubleAnimation(1d, new TimeSpan(0, 0, 1));
            var fadeOutAnimation = new DoubleAnimation(0d, fadeTime);

            fadeOutAnimation.Completed += (o, e) =>
            {
                CompositeImage.Visibility = Visibility.Hidden;

                _vm.ScreenshotPath = screenshotPath;
                ScreenshotGrid.Opacity = 0d;

                ScreenshotGrid.Visibility = Visibility.Visible;

                ScreenshotGrid.BeginAnimation(OpacityProperty, fadeInAnimation);
            };

            CompositeImage.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void Fadein_Photo(TimeSpan fadeTime)
        {
            var fadeInAnimation = new DoubleAnimation(1d, new TimeSpan(0, 0, 1));
            var fadeOutAnimation = new DoubleAnimation(0d, fadeTime);
            
            fadeOutAnimation.Completed += (o, e) =>
            {

                _vm.ScreenshotPath = "";               
            };

            CompositeImage.Opacity = 0d;

            CompositeImage.Visibility = Visibility.Visible;
            ScreenshotButton.Visibility = Visibility.Visible;

            CompositeImage.BeginAnimation(OpacityProperty, fadeInAnimation);
            ScreenshotButton.BeginAnimation(OpacityProperty, fadeInAnimation);

            ScreenshotGrid.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void Fadeout_ScreenshotButton(TimeSpan fadeTime)
        {
            var fadeOutAnimation = new DoubleAnimation(0d, fadeTime);

            fadeOutAnimation.Completed += (o, e) =>
            {
                ScreenshotButton.Opacity = 0d;
                ScreenshotButton.Visibility = Visibility.Hidden;
            };

            ScreenshotButton.Opacity = 1d;

            ScreenshotButton.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void BackVideoDrop_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackVideoDrop.Position = TimeSpan.Zero;
        }
    }
}


