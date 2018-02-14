using KinectPhotobooth.Common;
using System.Windows;

namespace KinectPhotobooth.ConfigWindows
{
    /// <summary>
    /// Interaction logic for OutputImagesConfig.xaml
    /// </summary>
    public partial class OutputImagesConfig : Window
    {
        public OutputImagesConfig()
        {
            InitializeComponent();
            txtOutPutImages.Text = DataContainer.ViewModel.ImagePhotoDirectory;
        }

        private void btnOutputImagesPath_Click(object sender, RoutedEventArgs e)
        {
            txtOutPutImages.Text = DataContainer.ViewModel.ImagePhotoDirectory;
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                    txtOutPutImages.Text = dialog.SelectedPath + "\\";

                DataContainer.ViewModel.ImagePhotoDirectory = txtOutPutImages.Text;
            }

            this.Close();
        }
    }
}