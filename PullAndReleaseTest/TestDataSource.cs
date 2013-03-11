using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace PullAndReleaseTest
{
    public class TestDataSource : INotifyPropertyChanged
    {
        private string _compressionTopValue;
        public string CompressionTopValue
        {
            get { return _compressionTopValue; }
            set
            {
                if (_compressionTopValue != value)
                {
                    _compressionTopValue = value;
                    RaisePropertyChanged("CompressionTopValue");
                }
            }
        }

        private string _compressionBottomValue;
        public string CompressionBottomValue
        {
            get { return _compressionBottomValue; }
            set
            {
                if (_compressionBottomValue != value)
                {
                    _compressionBottomValue = value;
                    RaisePropertyChanged("CompressionBottomValue");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            var d = PropertyChanged;
            if (d != null)
            {
                d(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
