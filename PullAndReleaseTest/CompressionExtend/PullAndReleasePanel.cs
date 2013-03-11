using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CompressionExtend
{
    public class PullAndReleasePanel : Grid
    {
        public const string ScrollViewerPart = "ScrollViewer";

        public const string NoVerticalCompressionState = "NoVerticalCompression";

        public const string CompressionTopState = "CompressionTop";
        public const string CompressionTopLimitState = "CompressionTopLimit";
        public const string ReleaseOnTopLimitState = "ReleaseOnTopLimit";

        public const string CompressionBottomState = "CompressionBottom";
        public const string CompressionBottomLimitState = "CompressionBottomLimit";
        public const string ReleaseOnBottomLimitState = "ReleaseOnBottomLimit";

        public PullAndReleasePanel()
            : base()
        {
            this.Loaded += new RoutedEventHandler(PullAndReleasePanel_Loaded);
        }

        void PullAndReleasePanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (scrollViewer == null)
            {
                scrollViewer = (ScrollViewer)this.Parent as ScrollViewer;
                
                if (scrollViewer != null)
                {
                    this.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(ListBoxScrollViewerEx_ManipulationCompleted);
                    this.MouseLeftButtonDown += new MouseButtonEventHandler(PullAndReleasePanel_MouseLeftButtonDown);
                    this.MouseMove += new MouseEventHandler(ListBoxScrollViewerEx_MouseMove);
                }
            }
        }

        #region ScrollViewerCompression

        private const int LimitSize = 100;

        private bool scrolling = false;
        private double dragPointY;
        //private double offset;
        private double lastTranslateY;
        private string ScrollViewerCompressionState;

        private ScrollViewer scrollViewer = null;

        void ListBoxScrollViewerEx_MouseMove(object sender, MouseEventArgs e)
        {
            if (!scrolling)
            {
                return;
            }

            var nowPoint = e.GetPosition((UIElement)scrollViewer);
            var CompressionTopValue = lastTranslateY + nowPoint.Y - dragPointY;
            var CompressionBottomValue = dragPointY - nowPoint.Y - (scrollViewer.ScrollableHeight + lastTranslateY);
            var CompressionNewStatus = NoVerticalCompressionState;

            // 上端の処理
            if (CompressionTopValue > LimitSize)
            {
                CompressionNewStatus = CompressionTopLimitState;
            }
            else if (CompressionTopValue > 0)
            {
                CompressionNewStatus = CompressionTopState;
            }

            // 下端の処理
            if (CompressionBottomValue > LimitSize)
            {
                CompressionNewStatus = CompressionBottomLimitState;
            }
            else if (CompressionBottomValue > 0)
            {
                CompressionNewStatus = CompressionBottomState;
            }

            // VisualStateの更新
            ScrollViewerCompressionState = CompressionNewStatus;
            VisualStateManager.GoToState(this.scrollViewer, CompressionNewStatus, true);
        }

        void PullAndReleasePanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            scrolling = true;

            dragPointY = e.GetPosition((UIElement)scrollViewer).Y;

            if (scrollViewer != null)
            {
                //offset = scrollViewer.VerticalOffset;
                UIElement element = (UIElement)scrollViewer.Content;
                CompositeTransform ct = element.RenderTransform as CompositeTransform;
                if (ct != null)
                {
                    lastTranslateY = ct.TranslateY;
                }
            }
        }

        void ListBoxScrollViewerEx_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            var CompressionNewStatus = NoVerticalCompressionState;
            switch (ScrollViewerCompressionState)
            {
                case "CompressionTopLimit":
                    CompressionNewStatus = ReleaseOnTopLimitState;
                    break;
                case "CompressionBottomLimit":
                    CompressionNewStatus = ReleaseOnBottomLimitState;
                    break;
                default:
                    break;
            }
            // VisualStateの更新
            VisualStateManager.GoToState(this.scrollViewer, CompressionNewStatus, true);

            scrolling = false;
        }

        #endregion

        #region FindVisualState
        private static VisualStateGroup FindVisualState(FrameworkElement element, string name)
        {
            if (element == null)
            {
                return null;
            }

            IList groups = VisualStateManager.GetVisualStateGroups(element);
            foreach (VisualStateGroup group in groups)
            {
                if (group.Name == name)
                {
                    return group;
                }
            }
            return null;
        }
        #endregion
    }
}
