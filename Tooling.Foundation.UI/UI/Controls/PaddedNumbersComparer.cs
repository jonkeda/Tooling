using System;
using System.ComponentModel;
using Tooling.Extensions;

namespace Tooling.Foundation.UI.Controls
{
    public class PaddedNumbersComparer : IDataGridColumnComparer<string>
    {
        public ListSortDirection Direction { get; set; }

        public int Compare(string x, string y)
        {
            x = x.PadNumbers();
            y = y.PadNumbers();


            return string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase);
        }

        public int Compare(object x, object y)
        {
            return Compare((string) x, (string) y);
        }
    }
}