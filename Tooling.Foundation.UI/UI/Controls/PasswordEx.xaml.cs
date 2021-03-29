using System.Windows;

namespace Tooling.Foundation.UI.Controls
{
    public partial class PasswordEx
    {
        public PasswordEx()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
            nameof(Password), typeof(string), typeof(PasswordEx), new FrameworkPropertyMetadata(default(string), 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                PasswordChangedCallback));

        private static void PasswordChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordEx passwordEx = d as PasswordEx;
            if (passwordEx?._passwordSetting == false)
            {
                passwordEx.TbxPasswordBox.Password = (string)e.NewValue;
            }
        }

        public string Password
        {
            get { return (string) GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        private bool _passwordSetting;
        private void TbxPasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _passwordSetting = true;
            Password = TbxPasswordBox.Password;
            _passwordSetting = false;
        }
    }
}
