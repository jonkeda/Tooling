using System.Windows.Controls;

namespace Tooling.Foundation.UI.Controls
{
    public class DataGridTextColumnEx<T> : DataGridTextColumn, IDataGridColumnCompare<T>
    {
        public IDataGridColumnComparer Comparer { get; set; }
    }
}