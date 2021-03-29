using System.IO;
using System.Windows.Input;
using Tooling.Foundation.UI;
using Tooling.Foundation.UI.Interfaces;
using Tooling.Mio;
using Tooling.UI;

namespace Tooling.Foundation.ViewModels.Other
{
    public class FolderBrowserViewModel : ViewModel
    {
        public FolderBrowserViewModel(IDialog window)
        {
            _window = window;
        }

        private FolderViewModelCollection _folders;
        private readonly IDialog _window;
        private FolderViewModel _selectedFolder;

        public FolderViewModelCollection Folders
        {
            get
            {
                if (_folders == null)
                {
                    _folders = new FolderViewModelCollection();
                    foreach (string drive in Directory.GetLogicalDrives())
                    {
                        _folders.Add(item: new FolderViewModel(new DiskPath(drive)));
                    }
                }
                return _folders;
            }
        }

        public VirtualPath SelectedPath { get; set; }

        public ICommand OkCommand
        {
            get { return new TargetCommand(Ok); }
        }

        private void Ok()
        {
            if (SelectedFolder != null)
            {
                SelectedPath = SelectedFolder.Path;
                _window.DialogResult = true;
                _window.Close();
            }
        }

        public ICommand CancelCommand
        {
            get { return new TargetCommand(Cancel); }
        }

        public FolderViewModel SelectedFolder
        {
            get
            {
                return _selectedFolder;
            } 
            set
            {
                SetProperty(ref _selectedFolder, value);
            } 
        }

        private void Cancel()
        {
            _window.DialogResult = false;
            _window.Close();
        }

    }
}

