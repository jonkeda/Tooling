using System.Collections;
using System.ComponentModel;

namespace Tooling.Foundation.UI.Controls
{
    public interface IDataGridColumnComparer : IComparer
    {
        ListSortDirection Direction { get; set; }
    }
}