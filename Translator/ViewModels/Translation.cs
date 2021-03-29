namespace Translator.ViewModels
{
    public class Translation
    {
        public Translation()
        {
        }

        public Translation(string van, string tot)
        {
            Van = van;
            Tot = tot;
        }

        public string Van { get; set; }

        public string Tot { get; set; }
    }
}