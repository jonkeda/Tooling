using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Tooling.Foundation.UI.Controls
{
    public class RichTextBoxEx : RichTextBox
    {
        public RichTextBoxEx()
        {
            LostFocus += LostFocusEx;
            
        }

        private void LostFocusEx(object sender, System.Windows.RoutedEventArgs e)
        {
            TextSource = GetText();
        }

        public static readonly DependencyProperty TextSourceProperty = DependencyProperty.Register(
            nameof(TextSource), typeof(string), typeof(RichTextBoxEx),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                TextSourcePropertyChangedCallback));

        private static void TextSourcePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RichTextBoxEx box = (RichTextBoxEx)d;
            box.SetText((string)e.NewValue);
        }

        public string TextSource
        {
            get { return (string) GetValue(TextSourceProperty); }
            set { SetValue(TextSourceProperty, value); }
        }

        public void SetText(string text)
        {
            Document.Blocks.Clear();
            Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        public string GetText()
        {
            return new TextRange(Document.ContentStart,
                Document.ContentEnd).Text;
        }

    }
}
