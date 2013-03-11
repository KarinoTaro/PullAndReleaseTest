using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace PullAndReleaseTest
{
    public partial class MainPage : PhoneApplicationPage
    {
        // コンストラクター
        public MainPage()
        {
            InitializeComponent();
        }

        private void Test1_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/TestPage1.xaml", UriKind.Relative));
        }

        private void Test2_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/TestPage2.xaml", UriKind.Relative));
        }

    }
}