using Tooling.Mio;
using Tooling.UI;

namespace Tooling.Foundation.ViewModels.Other
{
    public class FolderViewModel : ViewModel
    {
        public FolderViewModel(VirtualPath path)
        {
            Path = path;
        }
        
        public string Name
        {
            get { return Path.Name; }
        }

        public VirtualPath Path { get; }

        private FolderViewModelCollection _folders;
        public FolderViewModelCollection Folders
        {
            get
            {
                if (_folders == null)
                {
                    _folders = new FolderViewModelCollection();
                    foreach (VirtualPath path in Path.EnumerateDirectories())
                    {
                        _folders.Add(new FolderViewModel(path));
                    }
                }
                return _folders;
            }
        }

    }
}