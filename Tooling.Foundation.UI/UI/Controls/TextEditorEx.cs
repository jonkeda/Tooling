using System;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Search;

namespace Tooling.Foundation.UI.Controls
{
    public class TextEditorEx : TextEditor
    {
        public TextEditorEx()
        {
            //TextArea.SelectionChanged += OnSelectionChanged;
            LostFocus += OnSelectionChanged;
            LostFocus += TextEditorEx_LostFocus;

            SearchPanel.Install(TextArea);
        }

        private void TextEditorEx_LostFocus(object sender, RoutedEventArgs e)
        {
            TextEx = Text;
        }

        public static readonly DependencyProperty TextExProperty = DependencyProperty.Register(
            nameof(TextEx), typeof(string), typeof(TextEditorEx),
            new FrameworkPropertyMetadata(default(string), TextChangedCallback)
            {
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                BindsTwoWayByDefault = true
            });

        private static void TextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextEditorEx ctl = (TextEditorEx)d;
            ctl.Text = (string)e.NewValue;
        }

        public string TextEx
        {
            get { return (string)GetValue(TextExProperty); }
            set { SetValue(TextExProperty, value); }
        }

        public static readonly DependencyProperty BindableSelectionStartProperty =
            DependencyProperty.Register(
                nameof(BindableSelectionStart),
            typeof(int),
            typeof(TextEditorEx),
            new PropertyMetadata(OnBindableSelectionStartChanged));

        public static readonly DependencyProperty BindableSelectionLengthProperty =
            DependencyProperty.Register(
                nameof(BindableSelectionLength),
            typeof(int),
            typeof(TextEditorEx),
            new PropertyMetadata(OnBindableSelectionLengthChanged));

        private bool _changeFromUi;

        public int BindableSelectionStart
        {
            get
            {
                return (int)GetValue(BindableSelectionStartProperty);
            }

            set
            {
                SetValue(BindableSelectionStartProperty, value);
            }
        }

        public int BindableSelectionLength
        {
            get
            {
                return (int)GetValue(BindableSelectionLengthProperty);
            }

            set
            {
                SetValue(BindableSelectionLengthProperty, value);
            }
        }

        private static void OnBindableSelectionStartChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            TextEditorEx textBox = dependencyObject as TextEditorEx;

            if (!textBox._changeFromUi)
            {
                int newValue = (int)args.NewValue;
                textBox.SelectionStart = newValue;
            }
            else
            {
                textBox._changeFromUi = false;
            }
        }

        private static void OnBindableSelectionLengthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            TextEditorEx textBox = dependencyObject as TextEditorEx;

            if (!textBox._changeFromUi)
            {
                int newValue = (int)args.NewValue;
                textBox.SelectionLength = newValue;
            }
            else
            {
                textBox._changeFromUi = false;
            }
        }

        private void OnSelectionChanged(object sender, EventArgs eventArgs)
        {
            if (BindableSelectionStart != SelectionStart)
            {
                _changeFromUi = true;
                BindableSelectionStart = SelectionStart;
            }

            if (BindableSelectionLength != SelectionLength)
            {
                _changeFromUi = true;
                BindableSelectionLength = SelectionLength;
            }
        }

        public static IHighlightingDefinition LoadHighlightingDefinition(Type type,
            string resourceName)
        {
            string fullName = type.Namespace + "." + resourceName;
            using (Stream stream = type.Assembly.GetManifestResourceStream(fullName))
            using (XmlTextReader reader = new XmlTextReader(stream))
            {
                return HighlightingLoader.Load(reader, HighlightingManager.Instance);

            }
        }
    }
}
