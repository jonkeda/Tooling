using System.Threading.Tasks;
using System.Windows;
using Tooling.Foundation.ViewModels.Other;
using Tooling.Foundation.Views.Other;
using Tooling.Threading;
using Tooling.UI;

namespace Tooling.Foundation.UI.Progressing
{
    public class Progress : Progress<object>
    {
        protected Progress(string title, string label, bool isCancelable, ProgressMode mode) : base(title, label, isCancelable, mode, null)
        {
        }

        public static void Run(ProgressDelegate<object> action, string title, string label, bool isCancelable)
        {
            Progress progress = new Progress(title, label, isCancelable, ProgressMode.Synchronous);

            progress.Run(action);
        }

        public static void RunAsync(ProgressDelegate<object> action, string title, string label, bool isCancelable)
        {
            Progress progress = new Progress(title, label, isCancelable, ProgressMode.Asynchronous);

            progress.Run(action);
        }

    }

    public class Progress<T> : IProgress
    {
        public T Value { get; }
        protected ProgressWindow<T> _window;
        protected ProgressViewModel<T> _viewModel;
        protected readonly string _title;
        protected readonly string _label;
        protected readonly bool _isCancelable;
        protected readonly ProgressMode _mode;


        private volatile bool _isCanceled;

        public bool IsCanceled
        {
            get { return _isCanceled; }
        }

        protected Progress(string title, string label, bool isCancelable, ProgressMode mode, T value)
        {
            Value = value;
            _title = title;
            _label = label;
            _isCancelable = isCancelable;
            _mode = mode;
        }

        public void SetMessage(string message)
        {
            _viewModel.Message = message;
        }

        public void SetSubmessage(string submessage)
        {
            _viewModel.SubMessage = submessage;
        }

        public void SetLabel(string label)
        {
            _viewModel.Label = label;
        }

        public void Cancel()
        {
            _isCanceled = true;
        }

        protected void Run(ProgressDelegate<T> method)
        {
            _viewModel = new ProgressViewModel<T>(this, _mode);
            _window = new ProgressWindow<T>(_viewModel)
            {
                Owner = Application.Current.MainWindow,
                Title =  _title
            };

            _viewModel.IsCancelable = _isCancelable;
            SetLabel(_label);

            _window.Run(this, method);
        }
        
        public static void Run(ProgressDelegate<T> action, string title, string label, bool isCancelable, T value)
        {
            Progress<T> progress = new Progress<T>(title, label, isCancelable, ProgressMode.Synchronous, value);

            progress.Run(action);
        }


        public static void RunAsync(ProgressDelegate<T> action, string title, string label, bool isCancelable, T value)
        {
            Task.Run(() =>
            {
                ThreadDispatcher.Invoke(() =>
                {
                    Progress<T> progress = new Progress<T>(title, label, isCancelable, ProgressMode.Asynchronous, value);

                    progress.Run(action);
                });
            });
        }

    }
}
