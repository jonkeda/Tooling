using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tooling.Foundation.UI.Controls
{
    public class ContentControlEx : ContentControl
    {
        #region DoubleClickCommand

        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.Register(nameof(DoubleClickCommand), typeof(ICommand), 
                typeof(ContentControlEx), new PropertyMetadata(null));

        public static readonly DependencyProperty DoubleClickCommandParameterProperty =
            DependencyProperty.Register(nameof(DoubleClickCommandParameter), typeof(object), 
                typeof(ContentControlEx),
                new PropertyMetadata(null));

        public ICommand DoubleClickCommand
        {
            get { return (ICommand)GetValue(DoubleClickCommandProperty); }
            set { SetValue(DoubleClickCommandProperty, value); }
        }

        public object DoubleClickCommandParameter
        {
            get { return GetValue(DoubleClickCommandParameterProperty); }
            set { SetValue(DoubleClickCommandParameterProperty, value); }
        }

        #endregion

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            ICommand command = DoubleClickCommand;
            if (command != null
                && command.CanExecute(DoubleClickCommandParameter))
            {
                command.Execute(DoubleClickCommandParameter);
            }
            else
            {
                base.OnMouseDoubleClick(e);
            }
        }
    }
}
