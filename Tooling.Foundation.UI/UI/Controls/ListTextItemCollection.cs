using System.Collections.ObjectModel;
using Tooling.Extensions;

namespace Tooling.Foundation.UI.Controls
{
    public class ListTextItemCollection : ObservableCollection<ListTextItem>
    {
        public ListTextItemCollection()
        {
        }

        public ListTextItemCollection(string text)
        {
            foreach (string line in text.Lines())
            {
                Add(new ListTextItem(false, line));
            }
        }
    }
}