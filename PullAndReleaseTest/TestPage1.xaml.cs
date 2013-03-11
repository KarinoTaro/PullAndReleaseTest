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
using Chutus.Library;

namespace PullAndReleaseTest
{
    public partial class TestPage1 : PhoneApplicationPage
    {
        TestDataSource ds;
        
        public TestPage1()
        {
            InitializeComponent();

            ds = new TestDataSource();
            this.DataContext = ds;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // スクロールオーバー処理の設定
            try
            {
                var sv = MainScrollViewer;
                if (sv != null)
                {
                    FrameworkElement element = VisualTreeHelper.GetChild(sv, 0) as FrameworkElement;
                    if (element != null)
                    {
                        VisualStateGroup group = ScrollViewerCompressionHelper.FindVisualState(element, "ScrollStates");
                        if (group != null)
                        {
                            VisualStateGroup vgroup = ScrollViewerCompressionHelper.FindVisualState(element, "VerticalCompression");
                            if (vgroup != null)
                            {
                                vgroup.CurrentStateChanging += new EventHandler<VisualStateChangedEventArgs>(vgroup_CurrentStateChanging);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        void vgroup_CurrentStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            TestValue1.Text = e.NewState.Name;
            switch (e.NewState.Name)
            {
                case "CompressionTop":
                    TopCompress.Visibility = System.Windows.Visibility.Visible;
                    TopTextBlock.Text = "Top";
                    break;
                case "CompressionTopLimit":
                    TopCompress.Visibility = System.Windows.Visibility.Visible;
                    TopTextBlock.Text = "TopLimitOver";
                    break;
                case "ReleaseOnTopLimit":
                    TopCompress.Visibility = System.Windows.Visibility.Visible;
                    TopTextBlock.Text = "ReleaseTop";
                    break;
                case "CompressionBottom":
                    BottomCompress.Visibility = System.Windows.Visibility.Visible;
                    BottomTextBlock.Text = "Bottom";
                    break;
                case "CompressionBottomLimit":
                    BottomCompress.Visibility = System.Windows.Visibility.Visible;
                    BottomTextBlock.Text = "BottomLimitOver";
                    break;
                case "ReleaseOnBottomLimit":
                    BottomCompress.Visibility = System.Windows.Visibility.Visible;
                    BottomTextBlock.Text = "ReleaseBottom";
                    break;
                case "NoVerticalCompression":
                    TopCompress.Visibility = System.Windows.Visibility.Collapsed;
                    BottomCompress.Visibility = System.Windows.Visibility.Collapsed;
                    TopTextBlock.Text = "";
                    BottomTextBlock.Text = "";
                    break;
                default:
                    TopCompress.Visibility = System.Windows.Visibility.Collapsed;
                    BottomCompress.Visibility = System.Windows.Visibility.Collapsed;
                    TopTextBlock.Text = "";
                    BottomTextBlock.Text = "";
                    break;
            }
        }
    }
}