using System;
using System.Threading.Tasks;
using System.Windows;
using Tooling.Foundation.UI.Progressing;
using Tooling.Foundation.ViewModels.Other;
using Tooling.Threading;

namespace Tooling.Foundation.Views.Other
{
    public class ProgressWindow<T> : ProgressWindow
    {
        public ProgressWindow(ProgressViewModel<T> viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

        }

        private ProgressDelegate<T> _method;
        private UI.Progressing.Progress<T> _progress;

        private ProgressViewModel<T> ViewModel { get; }

        public void Run(UI.Progressing.Progress<T> progress, ProgressDelegate<T> method)
        {
            _progress = progress;
            _method = method;

            Task.Run(RunTask);

            if (ViewModel.Mode == ProgressMode.Asynchronous)
            {
                Show();
            }
            else
            {
                ShowDialog();
            }
        }

        private void RunTask()
        {
            try
            {
                _method.Invoke(_progress);
            }
            catch (Exception e)
            {
                ThreadDispatcher.Invoke(() => MessageBox.Show(e.Message));
            }
            finally
            {
                ThreadDispatcher.Invoke(Close);
            }
        }
    }

    public partial class ProgressWindow
    {
        public ProgressWindow()
        {
            InitializeComponent();
        }


    }
}
