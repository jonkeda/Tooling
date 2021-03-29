using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Tooling.Foundation.UI.Controls
{
    public class ScrollViewerEx : ScrollViewer
    {
        public new static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(
            "HorizontalOffset", typeof(double), typeof(ScrollViewerEx), 
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                HorizontalOffsetPropertyChangedCallback));

        private static void HorizontalOffsetPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScrollViewerEx sv = d as ScrollViewerEx;
            sv?.ScrollToHorizontalOffset((double)e.NewValue);
        }

        public new double HorizontalOffset
        {
            get { return (double) GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        public new static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(
            "VerticalOffset", typeof(double), typeof(ScrollViewerEx),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                VerticalOffsetPropertyChangedCallback));

        public new double VerticalOffset
        {
            get { return (double) GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        private static void VerticalOffsetPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScrollViewerEx sv = d as ScrollViewerEx;
            sv?.ScrollToVerticalOffset((double)e.NewValue);
        }
        
        protected override void OnScrollChanged(ScrollChangedEventArgs e)
        {
            base.OnScrollChanged(e);
            if (HorizontalOffset != e.HorizontalOffset)
            {
                HorizontalOffset = e.HorizontalOffset;
            }
            if (VerticalOffset != e.VerticalOffset)
            {
                VerticalOffset = e.VerticalOffset;
            }

        }
    }

    public interface IDataGridColumnComparer<T> : IComparer<T>, IDataGridColumnComparer
    {
       
    }

    public interface IDataGridColumnCompare<T> : IDataGridColumnCompare
    {
    }

    public class DataGridEx : DataGrid
    {
        public DataGridEx()
        {
            Sorting += DataGridExSorting;
        }

        private void DataGridExSorting(object sender, DataGridSortingEventArgs e)
        {
            DataGridColumn column = e.Column;
            if (!(column is IDataGridColumnCompare columnCompare
                    && columnCompare.Comparer != null))
            {
                return;
            }

            e.Handled = true;

            ListSortDirection direction = (column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;

            column.SortDirection = direction;

            CollectionView lcv = (CollectionView)CollectionViewSource.GetDefaultView(this.ItemsSource);

            //lcv.Comparer = columnCompare.Comparer;
        }

        #region Command

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(DataGridEx), new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(DataGridEx),
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
    }
}