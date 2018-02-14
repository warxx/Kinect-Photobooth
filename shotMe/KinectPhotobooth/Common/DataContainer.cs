using KinectPhotobooth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectPhotobooth.Common
{
    public static class DataContainer
    {
        private static MainWindowViewModel _vm;


        public static MainWindowViewModel ViewModel
        {
            get
            {

                if (_vm == null)
                    _vm = new MainWindowViewModel();

                return _vm;

            }
            set
            {
                if (value != null)
                    _vm = value;
            }
        }
    }
}
