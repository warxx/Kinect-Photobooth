using KinectPhotobooth.Common;
using KinectPhotobooth.ViewModels;
using System;
using System.Windows;

namespace KinectPhotobooth
{
    /// <summary>
    /// Interaction logic for CountDownConfig.xaml
    /// </summary>
    public partial class CountDownConfig : Window
    {
       

        public CountDownConfig()
        {
            InitializeComponent();
            txtCountdownSeconds.Text = DataContainer.ViewModel.CountdownSeconds.ToString();
        }

        //private void btnCoundownPath_Click(object sender, RoutedEventArgs e)
        //{
        //    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

        //    // Set filter for file extension and default file extension 
        //    dlg.DefaultExt = ".gif";
        //    dlg.Filter = "GIF Files (*.gif)|*.gif|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";


        //    // Display OpenFileDialog by calling ShowDialog method 
        //    Nullable<bool> result = dlg.ShowDialog();


        //    // Get the selected file name and display in a TextBox 
        //    if (result == true)
        //    {
        //        // Open document 
        //        string filename = dlg.FileName;
        //        txtCountDownPath.Text = filename;
        //    }
        //    DataContainer.ViewModel.CountdownGifPath = txtCountDownPath.Text;
        //}

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DataContainer.ViewModel.CountdownSeconds = int.Parse(txtCountdownSeconds.Text);
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
