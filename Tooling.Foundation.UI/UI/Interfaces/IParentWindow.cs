using System.Windows;

namespace Tooling.Foundation.UI.Interfaces
{
    public interface IParentWindow
    {
        WindowState WindowState { get; set; }
        void Focus();
    }
}