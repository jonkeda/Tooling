using System.Collections.Generic;
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
                dict.Add(translation.Van, translation);
            }

            return dict;
        }
    }

    public class TranslationDictionary : Dictionary<string, Translation>
    { }

    public class SentenceDictionary : Dictionary<string , bool>
    {
        public SentenceCollection CreateCollection()
        {
            SentenceCollection list = new SentenceCollection();
            foreach (var pair in this)
            {
                list.Add(new Sentence { Zin = pair.Key, Vertaald = pair.Value });
            }

            return list;
        }
    }

    public class SentenceCollection : Collection<Sentence>
    { }

    public class Sentence
    {
        public string Zin { get; set; }
        public bool Vertaald { get; set; }
    }

}