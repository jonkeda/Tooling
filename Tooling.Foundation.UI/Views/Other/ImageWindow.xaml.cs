using System.Windows;
using Tooling.Foundation.ViewModels.Other;
using Tooling.Mio;

namespace Tooling.Foundation.Views.Other
{

    public partial class ImageWindow
    {
        public ImageWindow()
        {
            InitializeComponent();
            ViewModel = new ImageViewModel();
            DataContext = ViewModel;
        }

        public VirtualFile Image {
            get { return ViewModel.Image; }
            set { ViewModel.Image = value; }
        }

        public ImageViewModel ViewModel { get; }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
