using KinectPhotobooth.Common;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KinectPhotobooth.ConfigWindows
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();

            CountdownDelay.Text = DataContainer.ViewModel.CountDownDelay.ToString();
            InitOverlayDelay.Text = DataContainer.ViewModel.OverlayInitDelay.ToString();
            MinBackgroundDistance.Text = DataContainer.ViewModel.MinBackgroundDistance.ToString();
            MaxBackgroundDistance.Text = DataContainer.ViewModel.BackgroundDistance.ToString();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            DataContainer.ViewModel.CountDownDelay = int.Parse(CountdownDelay.Text);
            DataContainer.ViewModel.OverlayInitDelay = int.Parse(InitOverlayDelay.Text);
            DataContainer.ViewModel.MinBackgroundDistance = int.Parse(MinBackgroundDistance.Text);
            DataContainer.ViewModel.BackgroundDistance = int.Parse(MaxBackgroundDistance.Text);

            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
