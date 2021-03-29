using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Tooling.Foundation.UI.Controls
{
    public enum ScrollIntoViewMode
    {
        None,
        Center,
        Bottom
    }

    public class ListViewEx : ListView
    {
        public ListViewEx()
        {
            AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnColumnClick));

            SelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedValue == null)
            {
                return;
            }
            if (ShouldScrollIntoView
                && SelectedValue != null)
            {
                if (!this.IsInview(SelectedValue))
                {
                    this.ScrollToCenterOfView(SelectedValue);
                }
            }
        }

        public static readonly DependencyProperty ShouldScrollIntoViewProperty = DependencyProperty.Register(
             nameof(ShouldScrollIntoView), typeof(bool), typeof(ListViewEx), new PropertyMetadata(default(bool)));

        public bool ShouldScrollIntoView
        {
            get { return (bool)GetValue(ShouldScrollIntoViewProperty); }
            set { SetValue(ShouldScrollIntoViewProperty, value); }
        }

        #region Command

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ListViewEx), new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(ListViewEx),
                new PropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            ICommand command = Command;
            if (command != null
                && command.CanExecute(CommandParameter))
            {
                e.Handled = true;
                command.Execute(CommandParameter);
            }
        }

        #endregion

        #region Columns

        private void OnColumnClick(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is GridViewColumnHeader columnHeader)
                || !(columnHeader.Column is GridViewColumnEx column))
            {
                return;
            }

            string orderBy = column.OrderBy ?? column.Header as string;
            ICommand command = OrderByCommand;
            if (command != null
                && command.CanExecute(orderBy))
            {
                e.Handled = true;
                command.Execute(orderBy);
            }
        }

        public static readonly DependencyProperty OrderByCommandProperty =
            DependencyProperty.Register(nameof(OrderByCommand), typeof(ICommand),
                typeof(ListViewEx), new PropertyMetadata(null));


        public ICommand OrderByCommand
        {
            get { return (ICommand)GetValue(OrderByCommandProperty); }
            set { SetValue(OrderByCommandProperty, value); }
        }

        #endregion

    }

    public static class ItemsControlExtensions
    {
        public static bool IsInview(this ItemsControl itemsControl, object item)
        {
            // Find the container
            UIElement container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as UIElement;
            if (container == null)
            {
                return true;
            }

            // Find the ScrollContentPresenter
            ScrollContentPresenter presenter = null;
            for (Visual vis = container; vis != null && vis != itemsControl; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if ((presenter = vis as ScrollContentPresenter) != null)
                    break;
            if (presenter == null)
            {
                return true;
            }

            // Find the IScrollInfo
            IScrollInfo scrollInfo =
                !presenter.CanContentScroll ? presenter :
                presenter.Content as IScrollInfo ??
                FirstVisualChild(presenter.Content as ItemsPresenter) as IScrollInfo ??
                presenter;

            // Compute the center point of the container relative to the scrollInfo
            Size size = container.RenderSize;
            Point center = container.TransformToAncestor((Visual)scrollInfo).Transform(new Point(size.Width / 2, size.Height / 2));
            center.Y += scrollInfo.VerticalOffset;
            center.X += scrollInfo.HorizontalOffset;

            Orientation orientation = Orientation.Vertical;

            // Adjust for logical scrolling
            if (scrollInfo is StackPanel || scrollInfo is VirtualizingStackPanel)
            {
                double logicalCenter = itemsControl.ItemContainerGenerator.IndexFromContainer(container) + 0.5;
                orientation = scrollInfo is StackPanel ? ((StackPanel)scrollInfo).Orientation : ((VirtualizingStackPanel)scrollInfo).Orientation;
                if (orientation == Orientation.Horizontal)
                    center.X = logicalCenter;
                else
                    center.Y = logicalCenter;
            }

            // Scroll the center of the container to the center of the viewport
            if (scrollInfo.CanVerticallyScroll) scrollInfo.SetVerticalOffset(CenteringOffset(center.Y, scrollInfo.ViewportHeight, scrollInfo.ExtentHeight));
            if (scrollInfo.CanHorizontallyScroll) scrollInfo.SetHorizontalOffset(CenteringOffset(center.X, scrollInfo.ViewportWidth, scrollInfo.ExtentWidth));

            if (orientation == Orientation.Horizontal)
            {
                return center.X > scrollInfo.HorizontalOffset 
                       && center.Y < scrollInfo.HorizontalOffset + scrollInfo.ExtentWidth ;
            }
            return center.Y > scrollInfo.VerticalOffset 
                   && center.Y < scrollInfo.VerticalOffset + scrollInfo.ExtentHeight ;
       }

        public static void ScrollToCenterOfView(this ItemsControl itemsControl, object item)
        {
            // Scroll immediately if possible
            if (!itemsControl.TryScrollToCenterOfView(item))
            {
                // Otherwise wait until everything is loaded, then scroll
                if (itemsControl is ListBox) ((ListBox)itemsControl).ScrollIntoView(item);
                itemsControl.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                  {
                      itemsControl.TryScrollToCenterOfView(item);
                  }));
            }
        }

        private static bool TryScrollToCenterOfView(this ItemsControl itemsControl, object item)
        {
            // Find the container
            UIElement container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as UIElement;
            if (container == null)
            {
                return false;
            }

            // Find the ScrollContentPresenter
            ScrollContentPresenter presenter = null;
            for (Visual vis = container; vis != null && vis != itemsControl; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if ((presenter = vis as ScrollContentPresenter) != null)
                    break;
            if (presenter == null)
            {
                return false;
            }

            // Find the IScrollInfo
            IScrollInfo scrollInfo =
                !presenter.CanContentScroll ? presenter :
                presenter.Content as IScrollInfo ??
                FirstVisualChild(presenter.Content as ItemsPresenter) as IScrollInfo ??
                presenter;

            // Compute the center point of the container relative to the scrollInfo
            Size size = container.RenderSize;
            Point center = container.TransformToAncestor((System.Windows.Media.Visual)scrollInfo).Transform(new Point(size.Width / 2, size.Height / 2));
            center.Y += scrollInfo.VerticalOffset;
            center.X += scrollInfo.HorizontalOffset;

            // Adjust for logical scrolling
            if (scrollInfo is StackPanel || scrollInfo is VirtualizingStackPanel)
            {
                double logicalCenter = itemsControl.ItemContainerGenerator.IndexFromContainer(container) + 0.5;
                Orientation orientation = scrollInfo is StackPanel ? ((StackPanel)scrollInfo).Orientation : ((VirtualizingStackPanel)scrollInfo).Orientation;
                if (orientation == Orientation.Horizontal)
                    center.X = logicalCenter;
                else
                    center.Y = logicalCenter;
            }

            // Scroll the center of the container to the center of the viewport
            if (scrollInfo.CanVerticallyScroll) scrollInfo.SetVerticalOffset(CenteringOffset(center.Y, scrollInfo.ViewportHeight, scrollInfo.ExtentHeight));
            if (scrollInfo.CanHorizontallyScroll) scrollInfo.SetHorizontalOffset(CenteringOffset(center.X, scrollInfo.ViewportWidth, scrollInfo.ExtentWidth));
            return true;
        }

        private static double CenteringOffset(double center, double viewport, double extent)
        {
            return Math.Min(extent - viewport, Math.Max(0, center - viewport / 2));
        }
        private static DependencyObject FirstVisualChild(System.Windows.Media.Visual visual)
        {
            if (visual == null) return null;
            if (VisualTreeHelper.GetChildrenCount(visual) == 0) return null;
            return VisualTreeHelper.GetChild(visual, 0);
        }
    }
}