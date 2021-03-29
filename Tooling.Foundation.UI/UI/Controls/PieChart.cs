using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Tooling.Foundation.UI.Controls
{
    public class PieChart : ContentControl
    {

        #region dependency properties

        public int PushOut { get; set; } = 0;

        public static readonly DependencyProperty PlottedPropertyProperty = DependencyProperty.Register(
            "PlottedProperty", typeof(string), typeof(PieChart), new PropertyMetadata(default(string), PlottedPropertyChanged));

        /// <summary>
        /// The property of the bound object that will be plotted
        /// </summary>
        public string PlottedProperty
        {
            get { return (string) GetValue(PlottedPropertyProperty); }
            set { SetValue(PlottedPropertyProperty, value); }
        }

        /// <summary>
        /// A class which selects a color based on the item being rendered.
        /// </summary>
        public IPieColorSelector ColorSelector
        {
            get;
            set;
        }

        /// <summary>
        /// The size of the hole in the centre of circle (as a percentage)
        /// </summary>
        public double HoleSize
        {
            get { return (double)GetValue(HoleSizeProperty); }
            set
            {
                SetValue(HoleSizeProperty, value);
                ConstructPiePieces();
            }
        }

        public static readonly DependencyProperty HoleSizeProperty =
            DependencyProperty.Register(nameof(HoleSize), typeof(double), typeof(PieChart), new UIPropertyMetadata(0.0));

        /// <summary>
        /// A list which contains the current piece pieces, where the piece index
        /// is the same as the index of the item within the collection view which 
        /// it represents.
        /// </summary>
        private readonly List<PiePiece> _piePieces = new List<PiePiece>();

        private readonly Canvas _canvas;

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource), typeof(object), typeof(PieChart), new PropertyMetadata(default(object), ItemsourceChanged));

        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
       
        #endregion


        public PieChart()
        {
            _canvas = new Canvas();
            Content = _canvas;
        }

        #region property change handlers

        private static void ItemsourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PieChart pie = sender as PieChart;
            pie?.ItemsourceContextChangedHandler();
        }

        private CollectionView _collectionView;
        private INotifyCollectionChanged _observable;
        /// <summary>
        /// Handle changes in the datacontext. When a change occurs handlers are registered for events which
        /// occur when the collection changes or any items within teh collection change.
        /// </summary>
        void ItemsourceContextChangedHandler()
        {
            if (_observable != null)
            {
                _observable.CollectionChanged -= BoundCollectionChanged;
            }

            _observable = ItemsSource as INotifyCollectionChanged;
            // handle the events that occur when the bound collection changes
            if (_observable != null)
            {
                _observable.CollectionChanged += BoundCollectionChanged;
            }

            if (_collectionView != null)
            {
                _collectionView.CurrentChanged -= CollectionViewCurrentChanged;
                _collectionView.CurrentChanging -= CollectionViewCurrentChanging;
            }

            // handle the selection change events
            _collectionView = (CollectionView)CollectionViewSource.GetDefaultView(ItemsSource);
            if (_collectionView == null)
            {
                return;
            }
            _collectionView.CurrentChanged += CollectionViewCurrentChanged;
            _collectionView.CurrentChanging += CollectionViewCurrentChanging;

            ConstructPiePieces();
            ObserveBoundCollectionChanges();
        }

        /// <summary>
        /// Handles changes to the PlottedProperty property.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void PlottedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PieChart pie = sender as PieChart;
            
            pie?.ConstructPiePieces();
        }

        #endregion

        #region event handlers

        /// <summary>
        /// Handles the MouseUp event from the individual Pie Pieces
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PiePieceMouseUp(object sender, MouseButtonEventArgs e)
        {
            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(DataContext);
            if (collectionView == null)
                return;

            PiePiece piece = sender as PiePiece;
            if (piece == null)
                return;

            // select the item which this pie piece represents
            int index = (int)piece.Tag;
            collectionView.MoveCurrentToPosition(index);
        }

        /// <summary>
        /// Handles the event which occurs when the selected item is about to change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CollectionViewCurrentChanging(object sender, CurrentChangingEventArgs e)
        {
            CollectionView collectionView = (CollectionView)sender;

            if (collectionView != null && collectionView.CurrentPosition >= 0 && collectionView.CurrentPosition <= _piePieces.Count)
            {
                PiePiece piece = _piePieces[collectionView.CurrentPosition];

                DoubleAnimation a = new DoubleAnimation
                {
                    To = 0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200))
                };

                piece.BeginAnimation(PiePiece.PushOutProperty, a);
            }
        }

        /// <summary>
        /// Handles the event which occurs when the selected item has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CollectionViewCurrentChanged(object sender, EventArgs e)
        {
            CollectionView collectionView = (CollectionView)sender;

            if (collectionView != null && collectionView.CurrentPosition >= 0 && collectionView.CurrentPosition <= _piePieces.Count)
            {
                PiePiece piece = _piePieces[collectionView.CurrentPosition];

                DoubleAnimation a = new DoubleAnimation
                {
                    To = 10,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200))
                };

                piece.BeginAnimation(PiePiece.PushOutProperty, a);
            }

            
        }

        /// <summary>
        /// Handles events which are raised when the bound collection changes (i.e. items added/removed)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BoundCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ConstructPiePieces();
            ObserveBoundCollectionChanges();
        }

        /// <summary>
        /// Iterates over the items inthe bound collection, adding handlers for PropertyChanged events
        /// </summary>
        private void ObserveBoundCollectionChanges()
        {
            foreach(object item in _collectionView)
            {
                if (item is INotifyPropertyChanged observable)
                {
                    observable.PropertyChanged += ItemPropertyChanged;
                }
            }
        }

        
        /// <summary>
        /// Handles events which occur when the properties of bound items change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // if the property which this pie chart represents has changed, re-construct the pie
            if (e.PropertyName.Equals(PlottedProperty))
            {
                ConstructPiePieces();
            }
        }

        #endregion

        private Brush GetBackgroundColor(object item)
        {
            if (item is IPieceBackgroundColor color)
            {
                return color.GetBackgroundColor();
            }

            return ColorSelector != null ? ColorSelector.SelectBrush(item, -1) : Brushes.Black;
        }

        private double GetPlottedPropertyValue(object item)
        {
            if (item is IPieceValue pieceValue)
            {
                return pieceValue.GetValue();
            }

            PropertyDescriptorCollection filterPropDesc = TypeDescriptor.GetProperties(item);
            object itemValue = filterPropDesc[PlottedProperty].GetValue(item);

            if (itemValue is double d)
            {
                return d;
            }
            if (itemValue is int i)
            {
                return i;
            }
            if (itemValue is long l)
            {
                return l;
            }

            return 0;
        }

        /// <summary>
        /// Constructs pie pieces and adds them to the visual tree for this control's canvas
        /// </summary>
        private void ConstructPiePieces()
        {
            CollectionView myCollectionView = _collectionView;
            if (myCollectionView == null)
                return;

            double halfWidth = Width / 2;
            double innerRadius = halfWidth * HoleSize;            

            // compute the total for the property which is being plotted
            double total = 0;
            foreach (object item in myCollectionView)
            {
                total += GetPlottedPropertyValue(item);
            }
            
            // add the pie pieces
            _canvas.Children.Clear();
            _piePieces.Clear();
                        
            double accumulativeAngle=0;
            foreach (object item in myCollectionView)
            {
                bool selectedItem = item == myCollectionView.CurrentItem;
                double value = GetPlottedPropertyValue(item);
                double wedgeAngle = value * 360 / total;

                PiePiece piece = new PiePiece()
                {
                    Radius = halfWidth,
                    InnerRadius = innerRadius,
                    CentreX = halfWidth,
                    CentreY = halfWidth,
                    PushOut = (selectedItem ? PushOut : 0),
                    WedgeAngle = wedgeAngle,
                    PieceValue = value,
                    RotationAngle = accumulativeAngle,
                    Fill = GetBackgroundColor(item),
                    Tag = myCollectionView.IndexOf(item),
                    ToolTip = new ToolTip()
                };

                piece.ToolTipOpening += PiePieceToolTipOpening;
                piece.MouseUp += PiePieceMouseUp;

                _piePieces.Add(piece);
                _canvas.Children.Insert(0, piece);

                accumulativeAngle += wedgeAngle;
            }
        }

        /// <summary>
        /// Handles the event which occurs just before a pie piece tooltip opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PiePieceToolTipOpening(object sender, ToolTipEventArgs e)
        {
            PiePiece piece = (PiePiece)sender;

            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(DataContext);
            if (collectionView == null)
                return;
                       
            // select the item which this pie piece represents
            int index = (int)piece.Tag;
            if (piece.ToolTip != null)
            {
                ToolTip tip = (ToolTip)piece.ToolTip;
                tip.DataContext = collectionView.GetItemAt(index);
            }         
        }

    }
}