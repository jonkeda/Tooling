using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using Tooling.Foundation.UI;
using Tooling.Mio;
using Tooling.UI;

namespace Tooling.Foundation.ViewModels.Other
{
    public class ImageViewModel : ViewModel
    {
        private VirtualFile _image;

        public VirtualFile Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public ICommand CopyImageCommand
        {
            get { return new TargetCommand(CopyImage); }
        }

        private void CopyImage()
        {
            Clipboard.SetImage(Image.ReadImage());
        }

        public ICommand CopyImageFileCommand
        {
            get { return new TargetCommand(CopyImageFile); }
        }

        private void CopyImageFile()
        {
            Clipboard.SetFileDropList(new StringCollection {Image.Path}); 
        }

    }
}
