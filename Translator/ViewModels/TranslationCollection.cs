using System.Collections.ObjectModel;

namespace Translator.ViewModels
{
    public class TranslationCollection : ObservableCollection<Translation>
    {
        public TranslationDictionary CreateDictionary()
        {
            var dict = new TranslationDictionary();
            foreach (Translation translation in this)
            {
                if (!dict.ContainsKey(translation.Van))
                {
                    dict.Add(translation.Van, translation);
                }
                else
                {
                    
                }
            }

            return dict;
        }
    }
}