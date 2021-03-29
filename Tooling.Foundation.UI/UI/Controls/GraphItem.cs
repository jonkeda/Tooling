using System.Windows.Controls;
using System.Windows.Input;
using Tooling.Foundation.Extensions;

namespace Tooling.Foundation.UI.Controls
{
    public class GraphItem : Border
    {
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                HandleMouseButtonDown(MouseButton.Left);
            }
            base.OnMouseDown(e);
        }

        private void HandleMouseButtonDown(MouseButton mouseButton)
        {
            ParentGraphSelector?.NotifyGraphItemClicked(this, mouseButton);
        }


        private GraphSelector ParentGraphSelector
        {
            get
            {
                return this.GetParentByType<GraphSelector>();
            }
        }
    }
}