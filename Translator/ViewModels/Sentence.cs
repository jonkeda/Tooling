namespace Translator.ViewModels
{
    public class Sentence
    {
        public string Zin { get; set; }
        public bool Vertaald { get; set; }

        public Translation Translation { get; set; }
    }
}