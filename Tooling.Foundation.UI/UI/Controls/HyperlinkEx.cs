using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace Tooling.Foundation.UI.Controls
{
    public class HyperlinkEx : Hyperlink
    {
        public HyperlinkEx()
        {
            RequestNavigate += Hyperlink_RequestNavigate;
        }

         

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }


}
