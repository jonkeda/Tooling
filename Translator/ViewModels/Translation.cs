using Tooling.UI;

namespace Translator.ViewModels
{
    public class Translation : ViewModel
    {
        private string _van;
        private string _tot;

        public Translation()
        {
        }

        public Translation(string van, string tot)
        {
            Van = van;
            Tot = tot;
        }

        public string Van
        {
            get { return _van; }
            set { SetProperty(ref _van, value); }
        }

        public string Tot
        {
            get { return _tot; }
            set { SetProperty(ref _tot, value); }
        }
    }
}