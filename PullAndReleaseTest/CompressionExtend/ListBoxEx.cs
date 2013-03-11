using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CompressionExtend
{
    [TemplatePart(Name = ListBoxEx.ScrollViewerPart, Type = typeof(ScrollViewer))]
    public class ListBoxEx : ListBox
    {
        public const string ScrollViewerPart = "ScrollViewer";

        public const string NoVerticalCompressionState = "NoVerticalCompression";

        public const string CompressionTopState = "CompressionTop";
        public const string CompressionTopLimitState = "CompressionTopLimit";
        public const string ReleaseOnTopLimitState = "ReleaseOnTopLimit";

        public const string CompressionBottomState = "CompressionBottom";
        public const string CompressionBottomLimitState = "CompressionBottomLimit";
        public const string ReleaseOnBottomLimitState = "ReleaseOnBottomLimit";

        public ListBoxEx()
            : base()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            scrollViewer = (ScrollViewer)this.GetTemplateChild(ListBoxEx.ScrollViewerPart);

            if (scrollViewer != null)
            {
                this.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(ListBoxScrollViewerEx_ManipulationStarted);
                this.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(ListBoxScrollViewerEx_ManipulationCompleted);
                this.MouseMove += new MouseEventHandler(ListBoxScrollViewerEx_MouseMove);
                scrollViewer.Loaded += scrollViewer_Loaded;
            }
        }

        void scrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = VisualTreeHelper.GetChild(scrollViewer, 0) as FrameworkElement;
            if (element != null)
            {
                VisualStateGroup group = FindVisualState(element, "ScrollStates");
                if (group != null)
                {
                    VisualStateGroup vgroup = FindVisualState(element, "VerticalCompression");
                    if (vgroup != null)
                    {
                        vgroup.CurrentStateChanging += new EventHandler<VisualStateChangedEventArgs>(vgroup_CurrentStateChanging);
                    }
                }
            }
        }

        private int compressing;
        private void vgroup_CurrentStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            switch (e.NewState.Name)
            {
                case CompressionTopState:
                    compressing = -1;
                    scrolling = true;
                    break;
                case CompressionBottomState:
                    compressing = 1;
                    scrolling = true;
                    break;
                case NoVerticalCompressionState:
                    ScrollViewerCompressionState = NoVerticalCompressionState;
                    break;
                default:
                    break;
            }
        }

        #region ScrollViewerCompression

        private const int LimitSize = 80;

        private bool scrolling = false;
        private double dragPointY;
        private double offset;
        private double lastTranslateY;
        private string ScrollViewerCompressionState;

        private ScrollViewer scrollViewer = null;

        void ListBoxScrollViewerEx_MouseMove(object sender, MouseEventArgs e)
        {
            if (!scrolling)
            {
                return;
            }
            if (compressing == 0)
            {
                dragPointY = e.GetPosition((UIElement)sender).Y;
                return;
            }
            if (compressing == -1)
            {
                dragPointY = (dragPointY + e.GetPosition((UIElement)sender).Y) / 2;
                compressing = -2;
            }
            if (compressing == 1)
            {
                dragPointY = (dragPointY + e.GetPosition((UIElement)sender).Y) / 2;
                compressing = 2;
            }

            if (scrollViewer != null)
            {
                var nowPoint = e.GetPosition((UIElement)sender);
                var CompressionTopValue = lastTranslateY + nowPoint.Y - dragPointY;
                var CompressionBottomValue = dragPointY - nowPoint.Y - lastTranslateY;
                var CompressionNewStatus = NoVerticalCompressionState;

                // 上端の処理
                if (compressing < 0)
                {
                    if (CompressionTopValue > LimitSize)
                    {
                        CompressionNewStatus = CompressionTopLimitState;
                    }
                    else if (CompressionTopValue > 0)
                    {
                        CompressionNewStatus = CompressionTopState;
                    }
                    else
                    {
                        return;
                    }
                }

                // 下端の処理
                if (compressing > 0)
                {
                    if (CompressionBottomValue > LimitSize / 2)
                    {
                        CompressionNewStatus = CompressionBottomLimitState;
                    }
                    else if (CompressionBottomValue > 0)
                    {
                        CompressionNewStatus = CompressionBottomState;
                    }
                    else
                    {
                        return;
                    }
                }

                // VisualStateの更新
                ScrollViewerCompressionState = CompressionNewStatus;
                VisualStateManager.GoToState(this.scrollViewer, CompressionNewStatus, true);
            }
        }

        void ListBoxScrollViewerEx_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            scrolling = true;

            dragPointY = e.ManipulationOrigin.Y;

            //if (scrollViewer != null)
            //{
            //    offset = scrollViewer.VerticalOffset;
            //    UIElement element = (UIElement)scrollViewer.Content;
            //    CompositeTransform ct = element.RenderTransform as CompositeTransform;
            //    if (ct != null)
            //    {
            //        lastTranslateY = ct.TranslateY;
            //    }
            //}
        }

        void ListBoxScrollViewerEx_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            var CompressionNewStatus = NoVerticalCompressionState;
            switch (ScrollViewerCompressionState)
            {
                case CompressionTopLimitState:
                    if (compressing < 0)
                    {
                        CompressionNewStatus = ReleaseOnTopLimitState;
                    }
                    break;
                case CompressionBottomLimitState:
                    if (compressing > 0)
                    {
                        CompressionNewStatus = ReleaseOnBottomLimitState;
                    }
                    break;
                default:
                    break;
            }
            // VisualStateの更新
            VisualStateManager.GoToState(this.scrollViewer, CompressionNewStatus, true);

            compressing = 0;
            scrolling = false;
        }
        
        #endregion
        
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
    }
}
