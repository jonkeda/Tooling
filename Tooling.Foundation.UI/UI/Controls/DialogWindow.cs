using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace Tooling.Foundation.UI.Controls
{
    public class DialogWindow : MetroWindow
    {
        public DialogWindow()
        {
            KeyDown += DialogWindow_KeyDown;

            LayoutTransform = new ScaleTransform(Zoom, Zoom, 0,0);
        }

        private void DialogWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        public static double Zoom { get; set; } = 1;
    }
}
