using System;
using System.Collections;
using System.Windows;
using System.Windows.Media;

namespace Chutus.Library
{
    public static class ScrollViewerCompressionHelper
    {
        // 指定したタイプのUIElementを検索して返す
        public static UIElement FindElementRecursive(FrameworkElement parent, Type targetType)
        {
            if (parent.GetType() == targetType)
            {
                return parent as UIElement;
            }
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            UIElement returnElement = null;
            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    Object element = VisualTreeHelper.GetChild(parent, i);
                    if (element.GetType() == targetType)
                    {
                        return element as UIElement;
                    }
                    else
                    {
                        returnElement = FindElementRecursive(VisualTreeHelper.GetChild(parent, i) as FrameworkElement, targetType);
                    }
                }
            }
            return returnElement;
        }

        // 指定したVisualStateGroupを検索して返す
        public static VisualStateGroup FindVisualState(FrameworkElement element, string name)
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
