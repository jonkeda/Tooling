using System.Windows;
using System.Windows.Controls;

namespace Tooling.Foundation.UI.Controls
{
    public class TextBoxEx : TextBox
    {
        public static readonly DependencyProperty SelectionStartExProperty = DependencyProperty.Register(
                nameof(SelectionStartEx), typeof(int), typeof(TextBoxEx), new FrameworkPropertyMetadata(default(int), 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectionStartExChangedCallback));
        private static void SelectionStartExChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxEx textBox = (TextBoxEx)d;
            textBox.Focus();
            textBox.SelectionStart = (int)e.NewValue;
        }
        public int SelectionStartEx
        {
            get { return (int) GetValue(SelectionStartExProperty); }
            set { SetValue(SelectionStartExProperty, value); }
        }


        public static readonly DependencyProperty SelectionLengthExProperty = DependencyProperty.Register(
                nameof(SelectionLengthEx), typeof(int), typeof(TextBoxEx), new FrameworkPropertyMetadata(default(int), 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectionLengthExChangedCallback));
        private static void SelectionLengthExChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxEx textBox = (TextBoxEx)d;
            textBox.Focus();
            textBox.SelectionLength = (int)e.NewValue;
        }
        public int SelectionLengthEx
        {
            get { return (int) GetValue(SelectionLengthExProperty); }
            set { SetValue(SelectionLengthExProperty, value); }
        }


        public static readonly DependencyProperty CaretIndexExProperty = DependencyProperty.Register(
                nameof(CaretIndexEx), typeof(int), typeof(TextBoxEx), new FrameworkPropertyMetadata(default(int), 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, CaretIndexExChangedCallback));
        private static void CaretIndexExChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxEx textBox = (TextBoxEx)d;
            textBox.Focus();
            int value = (int) e.NewValue;
            int line = textBox.GetLineIndexFromCharacterIndex(value);
            if (line >= 0)
            {
                textBox.ScrollToLine(line);
            }
        }
        public int CaretIndexEx
        {
            get { return (int) GetValue(CaretIndexExProperty); }
            set { SetValue(CaretIndexExProperty, value); }
        }
    }
}
