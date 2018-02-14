using KinectPhotobooth.Common;
using System.Windows;

namespace KinectPhotobooth
{
    /// <summary>
    /// Interaction logic for AddFB.xaml
    /// </summary>
    public partial class AddFB : Window
    {
        public AddFB()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //DataContainer.ViewModel.FaceBookData.Group = txtFbGroup.Text;
            DataContainer.ViewModel.FaceBookData.Password = txtFbPassword.Password;
            DataContainer.ViewModel.FaceBookData.UserName = txtFbUserName.Text;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
