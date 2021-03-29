namespace Tooling.Foundation.UI.Controls
{
    public class DataGridNameColumn : DataGridTextColumnEx<string>
    {
        private static  PaddedNumbersComparer _comparer = new PaddedNumbersComparer();
        public DataGridNameColumn()
        {
            Comparer = _comparer;
        }
    }
}