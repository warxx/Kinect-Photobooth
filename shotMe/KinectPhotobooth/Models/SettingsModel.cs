namespace KinectPhotobooth.Models
{
    public class SettingsModel
    {
        public SettingsModel()
        {
            this.BackgroundDistance = 4000;
            this.MinBackgroundDistance = 2500;
            this.OverlayInitDelay = 0;
            this.IsImagesChecked = true;
        }

        public int BackgroundDistance { get; set; }

        public int MinBackgroundDistance { get; set; }

        public int OverlayInitDelay { get; set; }

        public bool IsImagesChecked { get; set; }
    }
}
