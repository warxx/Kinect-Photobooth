using KinectPhotobooth.Common;
using KinectPhotobooth.Models;
using KinectPhotobooth.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KinectPhotobooth.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        private FbData _fbData;

        public FbData FaceBookData
        {
            get
            {
                if (_fbData == null)
                    _fbData = new FbData();

                return _fbData;
            }

            set
            {
                _fbData = value;
            }
        }

        private bool _isImagesChecked;

        public bool IsImagesChecked
        {
            get
            {
                return _isImagesChecked;
            }
            set
            {
                _isImagesChecked = value;
            }
        }

        private string _backgroundType;

        public string BackgroundType
        {
            get
            {
                return _backgroundType;
            }
            set
            {
                _backgroundType = value;
            }
        }

        private int _countDownSeconds = 5;


        public int CountdownSeconds
        {
            get
            {
                return _countDownSeconds;
            }
            set
            {
                _countDownSeconds = value;
            }
        }

        private int _countDownDelay = 3;

        public int CountDownDelay
        {
            get
            {
                return _countDownDelay;
            }
            set
            {
                _countDownDelay = value;
            }
        }

        private int _overlayInitDelay = 5;

        public int OverlayInitDelay {
            get
            {
                return this._overlayInitDelay;
            }
            set
            {
                this._overlayInitDelay = value;
            }
        }

        private string _backgroundPath;

        public string BackgroundPath
        {
            get
            {
                if(string.IsNullOrEmpty(_backgroundPath))
                {
                    _backgroundPath = "";
                }

                return _backgroundPath;
            }
            set
            {
                _backgroundPath = value;
            }
        }

        private string _overlayPath;

        public string OverlayPath
        {
            get
            {
                if (string.IsNullOrEmpty(_overlayPath))
                {
                    _overlayPath = "";
                }

                return _overlayPath;
            }
            set
            {
                _overlayPath = value;
            }
        }

        private string _endOverlayPath;

        public string EndOverlayPath
        {
            get
            {
                if (string.IsNullOrEmpty(_endOverlayPath))
                {
                    _endOverlayPath = "";
                }

                return _endOverlayPath;
            }
            set
            {
                _endOverlayPath = value;
            }
        }

        private string _ImagePhotoDirectory;

        public string ImagePhotoDirectory
        {
            get
            {
                //if(string.IsNullOrEmpty(_ImagePhotoDirectory))
                //{
                //    _ImagePhotoDirectory = "";
                //}

                return _ImagePhotoDirectory;
            }

            set
            {
                _ImagePhotoDirectory = value;
            }
        }

        private EmailService _email;

        private WriteableBitmap _bitmap = null;

        public ICommand OnClearClicked { get; set; }

        private bool _LeaveTrails;
        public bool LeaveTrails
        {
            get { return _LeaveTrails; }
            set
            {
                _LeaveTrails = value;
                OnProperyChanged();
            }
        }

        private bool _PersonFill;
        public bool PersonFill
        {
            get { return _PersonFill; }
            set
            {
                _PersonFill = value;
                OnProperyChanged();
            }
        }
        public WriteableBitmap Bitmap
        {
            get { return _bitmap; }
            set { _bitmap = value; }
        }

        private string _screenshotpath;

        public string ScreenshotPath
        {
            get
            {
                return this._screenshotpath;
            }
            set
            {
                this._screenshotpath = value;
                OnProperyChanged();
            }
        }

        public ImageSource ImageSource
        {
            get
            {
                return _bitmap;
            }
        }

        private ImageSource _nis;

        public ImageSource NewImageSource
        {
            get
            {
                return _nis;
            }
            set
            {
                _nis = value;
                OnProperyChanged();
            }
        }

        private int _BackgroundDistance;

        public int BackgroundDistance
        {
            get
            {
                return _BackgroundDistance;
            }

            set
            {
                _BackgroundDistance = value;
                Inches = value;
                OnProperyChanged();
            }
        }

        private int _MinBackgroundDistance;

        public int MinBackgroundDistance
        {
            get
            {
                return _MinBackgroundDistance;
            }

            set
            {
                _MinBackgroundDistance = value;
                Inches = value;
                OnProperyChanged();
            }
        }

        public double Inches
        {
            get
            {
                return 0.039370 * _BackgroundDistance;
            }
            set
            {
                OnProperyChanged();
            }
        }

        private string _StatusText;
        public  string StatusText
        {
            get
            {
                return _StatusText;
            }
            set
            {
                _StatusText = value;
                OnProperyChanged();
            }
        }

        private BackgroundModel _SelectedBackground;
        public BackgroundModel SelectedBackground
        {
            get { return _SelectedBackground; }
            set
            {
                _SelectedBackground = value;
                OnProperyChanged();
            }
        }

        private OverlayModel _SelectedOverlay;
        public OverlayModel SelectedOverlay
        {
            get { return _SelectedOverlay; }
            set
            {
                _SelectedOverlay = value;
                OnProperyChanged();
            }
        }

        private OverlayModel _SelectedEndOverlay;
        public OverlayModel SelectedEndOverlay
        {
            get { return _SelectedEndOverlay; }
            set
            {
                _SelectedEndOverlay = value;
                OnProperyChanged();
            }
        }

        private string _EmailAddress = "wifink@microsoft.com";

        public string EmailAddress
        {
            get
            {
                return _EmailAddress;
            }
            set
            {
                _EmailAddress = value;
                OnProperyChanged();
            }
        }


        private bool _ContactMe = true;
        public bool ContactMe
        {
            get
            {
                return _ContactMe;
            }
            set
            {
                _ContactMe = value;
                OnProperyChanged();
            }
        }

        public ObservableCollection<BackgroundModel> Backgrounds { get; set; }
        public ObservableCollection<OverlayModel> Overlays { get; set; }
        public ObservableCollection<OverlayModel> EndOverlays { get; set; }

        private void ClearClickedCommand()
        {
            EmailAddress = "";
            ContactMe = true;
        }

        public MainWindowViewModel()
        {
            OnClearClicked = new RelayCommand(ClearClickedCommand);

            Backgrounds = new ObservableCollection<BackgroundModel>();
            Overlays = new ObservableCollection<OverlayModel>();
            EndOverlays = new ObservableCollection<OverlayModel>();

            //Set the initial Background depth to 4000 mm
            _BackgroundDistance = 4000;
            _MinBackgroundDistance = 500;



            //Backgrounds.Add(new BackgroundModel() { Name = "Beach", ImagePath = "Images/Backgrounds/Beach.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Building", ImagePath = "Images/Backgrounds/FromBuilding.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Cliff Jump", ImagePath = "Images/Backgrounds/Cliff-jump.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Crowd", ImagePath = "Images/Backgrounds/Crowd.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Dinosaur", ImagePath = "Images/Backgrounds/Dino.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Explosion", ImagePath = "Images/Backgrounds/explosion.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Flames", ImagePath = "Images/Backgrounds/Flames.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Flowers", ImagePath = "Images/Backgrounds/Flowers1.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Galaxy", ImagePath = "Images/Backgrounds/Galaxy.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Lightning", ImagePath = "Images/Backgrounds/lightning.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Mars", ImagePath = "Images/Backgrounds/mars.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Moon", ImagePath = "Images/Backgrounds/astronaut_on_the_moon.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Presidents", ImagePath = "Images/Backgrounds/Presidents2.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Shark", ImagePath = "Images/Backgrounds/Shark.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Spaceship Window", ImagePath = "Images/Backgrounds/SpaceshipWindow.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Star Trek", ImagePath = "Images/Backgrounds/StarTrek1.jpg" });;
            //Backgrounds.Add(new BackgroundModel() { Name = "Tornado", ImagePath = "Images/Backgrounds/tornado.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Volcano", ImagePath = "Images/Backgrounds/volcano.jpg" });
            //Backgrounds.Add(new BackgroundModel() { Name = "Warp", ImagePath = "Images/Backgrounds/Warp.jpg" });

        //    LoadBackgrounds();

            this.ScreenshotPath = "";

            var myPhotos = String.Format("{0}\\{1}", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Photobooth");


            var path = new DirectoryInfo(myPhotos);
            if (!path.Exists)
            {
                path.Create();
            }


        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnProperyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


        public async void SendMail(BitmapEncoder bitmap)
        {

            if (_email == null)
            {
                _email = new EmailService();
            }


            bool mailSent = await _email.SendMail(_EmailAddress, bitmap);

        }

        //public void LoadBackgrounds()
        //{
        //    Backgrounds.Clear();
        //    var backgrounds = LoadBackgroundImages(BackroundPath);

        //    if(backgrounds != null)
        //    {
        //        backgrounds.ForEach(x => Backgrounds.Add(x));
        //    }
        //}

        //public void LoadBackgrounds(string path)
        //{
        //    BackroundPath = path;
        //    Backgrounds.Clear();
        //    var backgrounds = LoadBackgroundImages(BackroundPath);
        //    if(_backgroundPath != "")
        //    {
        //        backgrounds.ForEach(x => Backgrounds.Add(x));
        //    }
        //}

        public void LoadBackgrounds(string path, string imageType)
        {
            if(imageType == "backgrounds")
            {
                var backgroundsType = path.Substring(path.Length - 6);

                Backgrounds.Clear();
                var backgrounds = LoadBackgroundImages(path, "images");

                if (_backgroundPath != "")
                {
                    backgrounds.ForEach(x => Backgrounds.Add(x));
                }
            }

            if(imageType == "videos")
            {
                Backgrounds.Clear();
                var backgrounds = LoadBackgroundImages(path, "videos");

                if (_backgroundPath != "")
                {
                    backgrounds.ForEach(x => Backgrounds.Add(x));
                }
            }
            
            if(imageType == "init_overlays")
            {
                Overlays.Clear();
                var overlays = LoadOverlayImages(path);

                if (_overlayPath != "" && overlays != null)
                {
                    overlays.ForEach(x => Overlays.Add(x));
                }
            }

            if(imageType == "end_overlays")
            {
                EndOverlays.Clear();
                var endOverlays = LoadOverlayImages(path);

                if (_endOverlayPath != "")
                {
                    endOverlays.ForEach(x => EndOverlays.Add(x));
                }
            }                                   
        }

        private List<BackgroundModel> LoadBackgroundImages(string rootPath, string backgroundsType)
        {
            var results = new List<BackgroundModel>();
            if(string.IsNullOrEmpty(rootPath))
            {
                return null;
            }


            var fls = Directory.GetFiles(rootPath);

            if(backgroundsType == "images")
            {
                foreach (var fl in fls)
                {
                    var type = fl.Substring(fl.Length - 3);

                    if (type == "jpg" || type == "png")
                    {
                        results.Add(LoadBackground(fl));
                    }
                }
            }
            
            
            if(backgroundsType == "videos")
            {
                foreach (var fl in fls)
                {
                    var type = fl.Substring(fl.Length - 3);

                    if (type == "mp4" || type == "wmv")
                    {
                        results.Add(LoadBackground(fl));
                    }
                }
            }

            return results;
        }

        private List<OverlayModel> LoadOverlayImages(string rootPath)
        {
            var results = new List<OverlayModel>();
            if (string.IsNullOrEmpty(rootPath))
            {
                return null;
            }


            var fls = Directory.GetFiles(rootPath);

            foreach (var fl in fls)
            {
                var type = fl.Substring(fl.Length - 3);

                if (type == "jpg" || type == "png")
                {
                    results.Add(LoadOverlay(fl));
                }
            }


            return results;
        }

        private BackgroundModel LoadBackground(string filePath)
        {
            var flName = Path.GetFileName(filePath);
            return new BackgroundModel
            {
                ImagePath = filePath,
                Name = flName
            };
        }

        private OverlayModel LoadOverlay(string filePath)
        {
            var flName = Path.GetFileName(filePath);
            return new OverlayModel
            {
                ImagePath = filePath,
                Name = flName
            };
        }

        public BitmapImage ResizeImage(string path, int width, int height)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.DecodePixelHeight = width;
            image.DecodePixelWidth = height;
            image.UriSource = new Uri(path);
            image.EndInit();

            return image;
        }
    }
}
