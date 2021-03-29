using Tooling.UI;

namespace Tooling.Foundation.UI.Controls
{
    public class ListTextItem : PropertyNotifier
    {
        public ListTextItem()
        {
        }

        public ListTextItem(bool isSelected, string description)
        {
            _isSelected = isSelected;
            _description = description;
        }

        private bool _isSelected;
        private string _description;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
