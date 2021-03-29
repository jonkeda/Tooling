using System.Windows;
using System.Windows.Input;

namespace Tooling.Foundation.UI.Controls
{
    public class CanvasExEventArgs
    {
        public CanvasExEventArgs(IInputElement element, MouseEventArgs eventArgs)
        {
            EventArgs = eventArgs;
            Element = element;
        }

        public MouseEventArgs EventArgs { get; }

        public IInputElement Element { get; }

        public Point GetPosition()
        {
            return EventArgs.GetPosition(Element);
        }
    }
}