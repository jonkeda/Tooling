using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Tooling.Foundation.Extensions
{
    public static class DependencyObjectExtension
    {
        public static T GetParentByType<T>(this DependencyObject prop) where T : class //where T : DependencyObject
        {
            if (prop == null)
            {
                return null;
            }
            DependencyObject parent = VisualTreeHelper.GetParent(prop);
            T p = parent as T;
            while (parent != null
                   && p == null)
            {
                parent = VisualTreeHelper.GetParent(parent);
                p = parent as T;
            }
            return p;
        }

        public static T GetFirstChildByType<T>(this DependencyObject prop) where T : DependencyObject
        {
            if (prop == null)
            {
                return null;
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(prop); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(prop, i);
                if (child == null)
                    continue;

                T castedProp = child as T;
                if (castedProp != null)
                    return castedProp;

                castedProp = GetFirstChildByType<T>(child);

                if (castedProp != null)
                    return castedProp;
            }
            return null;
        }

        public static IEnumerable<T> GetChildrenByType<T>(this DependencyObject prop) where T : DependencyObject
        {
            if (prop == null)
            {
                yield break;
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(prop); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild((prop), i);
                if (child == null)
                    continue;

                T castedProp = child as T;
                if (castedProp != null)
                    yield return castedProp;

                foreach (T subChild in GetChildrenByType<T>(child))
                {
                    yield return subChild;
                }
            }
        }

    }
}
