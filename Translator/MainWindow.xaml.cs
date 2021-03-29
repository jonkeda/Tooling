using System;
using Tooling.Helpers;
using Tooling.UI;
using Translator.ViewModels;

namespace Translator
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainViewModel();
            DataContext = ViewModel;

            Title = $"Translator {AssemblyFileVersionHelper.GetShortVersion()}";
        }

        public MainViewModel ViewModel { get; }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            ViewModel.Close();

        }
    }
}
