using System.Windows;
using Tooling.Threading;

namespace Translator
{
    public partial class App : Application
    {
        public App()
        {
            Current.DispatcherUnhandledException += manager_DispatcherUnhandledException;
        }

        void manager_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            ThreadDispatcher.Invoke(() =>
            {
                try
                {
                    MessageBox.Show(e.Exception.ToString());
                }
                catch
                { }
            });
        }
    }
}
