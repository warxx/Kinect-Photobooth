using System.Windows.Input;

namespace KinectPhotobooth.Common
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand
                (
                    "Exit",
                    "Exit",
                    typeof(CustomCommands),
                    new InputGestureCollection()
                    {
                        new KeyGesture(Key.Escape)
                    }
                );

        public static readonly RoutedUICommand GoAdmin = new RoutedUICommand
                (
                    "GoAdmin",
                    "GoAdmin",
                    typeof(CustomCommands),
                    new InputGestureCollection()
                    {
                        new KeyGesture(Key.F3)
                    }
                );

        //Define more commands here, just like the one above
    }
}
