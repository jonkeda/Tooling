using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tooling.Foundation.UI.Controls
{
    public class GraphSelector : ItemsControl
    {
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem), typeof(object), typeof(GraphSelector), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public void NotifyGraphItemClicked(GraphItem graphItem, MouseButton mouseButton)
        {
            if (mouseButton == MouseButton.Left)
            {
                SelectedItem = graphItem.DataContext;
            }
        }
    }
}