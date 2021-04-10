using System.ComponentModel;
using NopDeployer.ViewModels;

namespace NopDeployer
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainViewModel();
            DataContext = ViewModel;
        }

        public MainViewModel ViewModel { get; }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            ViewModel.Close();
        }
    }
}
