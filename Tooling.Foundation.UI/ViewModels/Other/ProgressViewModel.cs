using System.Windows.Input;
using Tooling.Foundation.UI;
using Tooling.Foundation.UI.Progressing;
using Tooling.UI;

namespace Tooling.Foundation.ViewModels.Other
{
    public class ProgressViewModel<T> : ProgressViewModel
    {
        public ProgressViewModel(Progress<T> progress, ProgressMode mode) : base(mode)
        {
            Progress = progress;
        }
        public Progress<T> Progress { get; }

        protected override void Cancel()
        {
            Progress.Cancel();
        }

    }

    public class ProgressViewModel : ViewModel
    {
        public ProgressMode Mode { get; }

        public ProgressViewModel(ProgressMode mode)
        {
            Mode = mode;
        }

        private string _message;
        private string _label;
        private bool _isCancelable;
        private string _subMessage;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public string SubMessage
        {
            get { return _subMessage; }
            set { SetProperty(ref _subMessage, value); }
        }

        public string Label
        {
            get { return _label; }
            set { SetProperty(ref _label, value); }
        }

        public bool IsCancelable
        {
            get { return _isCancelable; }
            set { SetProperty(ref _isCancelable, value); }
        }


        public ICommand CancelCommand
        {
            get { return new TargetCommand(Cancel); }
        }

        protected  virtual void Cancel()
        {
        }
    }
}