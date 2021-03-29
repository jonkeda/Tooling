using Tooling.Foundation.UI.Interfaces;
using Tooling.Foundation.ViewModels.Other;
using Tooling.Mio;

namespace Tooling.Foundation.Views.Other
{
    public partial class FolderBrowserWindow : IDialog
    {
        public FolderBrowserWindow()
        {
            InitializeComponent();

            _viewModel = new FolderBrowserViewModel(this);
            DataContext = _viewModel;
        }

        private readonly FolderBrowserViewModel _viewModel;

        public VirtualPath SelectedPath
        {
            get { return _viewModel.SelectedPath; }
        }
    }
}
