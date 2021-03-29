using System;
using System.Windows.Input;

namespace Tooling.Foundation.UI.Progressing
{
    public class ProgressCommand : ProgressCommand<object>
    {
        public ProgressCommand(ProgressDelegate<object> method) : base(method)
        {
        }

        public ProgressCommand(ProgressDelegate<object> method, string title, string label) : base(method, title, label)
        {
        }

        public ProgressCommand(ProgressDelegate<object> method, string title, string label, bool isCancelable) : base(method, title, label, isCancelable)
        {
        }

        public ProgressCommand(ProgressDelegate<object> method, ProgressCanExecuteDelegate canExecute) : base(method, canExecute)
        {
        }
    }

    public class ProgressCommand<T> : ICommand
    {
        private readonly ProgressCanExecuteDelegate _canExecute;
        private readonly ProgressDelegate<T> _method;
        public string Title { get; set; } = "Progress";
        public string Label { get; set; }
        public bool IsCancelable { get; set; } = false;

        public ProgressCommand(ProgressDelegate<T> method) : this(method, "", "", false)
        {
        }

        public ProgressCommand(ProgressDelegate<T> method, string title, string label) : this(method, title, label, false)
        {
        }

        public ProgressCommand(ProgressDelegate<T> method, string title, string label, bool isCancelable)
        {
            _method = method;
            Title = title;
            Label = label;
            IsCancelable = isCancelable;
        }

        public ProgressCommand(ProgressDelegate<T> method, ProgressCanExecuteDelegate canExecute)
        {
            _method = method;
            _canExecute = canExecute;
        }

        #region ICommand Members

        public void Execute(object parameter)
        {
            Progress<T>.Run(_method, Title, Label, IsCancelable, default(T));
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute();
            }
            return true;
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }
}
