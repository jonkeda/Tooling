using System.Collections.Generic;

namespace Translator.ViewModels
{
    public class SentenceDictionary : Dictionary<string , Translation>
    {
        public SentenceCollection CreateCollection()
        {
            SentenceCollection list = new SentenceCollection();
            foreach (var pair in this)
            {
                list.Add(new Sentence { Zin = pair.Key, Vertaald = pair.Value != null, Translation = pair.Value});
            }

            return list;
        }
    }
}