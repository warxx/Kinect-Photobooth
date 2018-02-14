using KinectPhotobooth.Common;
using System.Windows;

namespace KinectPhotobooth.ConfigWindows
{
    /// <summary>
    /// Interaction logic for BackgroundImagesConfig.xaml
    /// </summary>
    public partial class BackgroundImagesConfig : Window
    {
        public BackgroundImagesConfig()
        {
            InitializeComponent();
            txtBackgroundPath.Text = DataContainer.ViewModel.BackgroundPath;
        }

        private void btnBackgroundSelect_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                    txtBackgroundPath.Text = dialog.SelectedPath + "\\";

                DataContainer.ViewModel.BackgroundPath = txtBackgroundPath.Text;
              //  DataContainer.ViewModel.LoadBackgrounds(txtBackgroundPath.Text);
            }

            this.Close();
        }
    }
    }