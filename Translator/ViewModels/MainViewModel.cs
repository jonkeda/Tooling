using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.Win32;
using Tooling.Foundation.UI;
using Tooling.UI;
using Microsoft.Office.Interop.Word;
using Tooling.Extensions;
using Document = Microsoft.Office.Interop.Word.Document;

namespace Translator.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region Fields

        private string _log;
        private string _translationTexts = "Wymiary klienta\tiets met een klant\nWymiary klienta2\tiets met een klant";
        private TranslationCollection _translations;

        #endregion

        #region Constructor

        public MainViewModel()
        {
            LoadDefaultTranslations();
        }

        #endregion

        #region Methods

        public string Log
        {
            get { return _log; }
            set { SetProperty(ref _log, value); }
        }

        public string TranslationTexts
        {
            get { return _translationTexts; }
            set { SetProperty(ref _translationTexts, value); }
        }

        public TranslationCollection Translations
        {
            get { return _translations; }
            set { SetProperty(ref _translations, value); }
        }
        
        public ICommand TranslateDocumentCommand
        {
            get { return new TargetCommand(TranslateDocument); }
        }

        private void TranslateDocument()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                Translate2(ofd.FileName);
            }
        }

        private static Regex _sentences = new Regex(@"<w:t>([^<]*)</w:t>", RegexOptions.Compiled);

        private void Translate2(string path)
        {
            try
            {
                string directory = Path.GetDirectoryName(path);
                string filename = Path.GetFileNameWithoutExtension(path);
                string extension = Path.GetExtension(path);
                string newPath = Path.Combine(directory, $"{filename}_vertaald{extension}");
                
                File.Copy(path, newPath, true);

                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(newPath, true))
                {
                    using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    {
                        //var translations = Translations; // GetTranslations();
                        _translationsByVan = Translations.CreateDictionary();
                        _scentencesByFound = new SentenceDictionary();
                        string docText = sr.ReadToEnd();
                        string newText = _sentences.Replace(docText, MatchEvaluator);
                        Sentences = _scentencesByFound.CreateCollection();

                        using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                        {
                            sw.Write(newText);
                        }
                        AddLog($"Document {path} vertaald.");

                        Process.Start(newPath);
                    }
                }
            }
            catch (Exception e)
            {
                AddLog(e);
            }
        }

        private TranslationDictionary _translationsByVan;
        private SentenceDictionary _scentencesByFound;
        private SentenceCollection _sentences1;
        private Sentence _selectedSentence;
        private string _translationText;

        public SentenceCollection Sentences
        {
            get { return _sentences1; }
            set { SetProperty(ref _sentences1, value); }
        }

        public Sentence SelectedSentence
        {
            get { return _selectedSentence; }
            set { SetProperty(ref _selectedSentence, value); }
        }

        public string TranslationText
        {
            get { return _translationText; }
            set { SetProperty(ref _translationText, value); }
        }

        public ICommand TranslationAddCommand
        {
            get
            {
                return new TargetCommand(TranslationAdd);
            }
        }

        private void TranslationAdd()
        {
            if (SelectedSentence == null)
            {
                return;
            }
            Translation translation = Translations.FirstOrDefault(v => v.Van == SelectedSentence.Zin);
            if (translation == null)
            {
                Translations.Add(new Translation(SelectedSentence.Zin, TranslationText));
            }
        }

        private string MatchEvaluator(Match match)
        {
            string found = match.Groups[1].Value;
            string foundTrimmed = found.TrimStart().TrimEnd();
            if (_translationsByVan.TryGetValue(foundTrimmed, out var translation))
            {
                if (!_scentencesByFound.ContainsKey(found))
                {
                    _scentencesByFound.Add(found, true);
                }
                return match.Value.Replace(foundTrimmed, translation.Tot);
            }
            if (!_scentencesByFound.ContainsKey(found))
            {
                _scentencesByFound.Add(found, false);
            }
            return match.Value;
        }


        private void AddLog(string text)
        {
            Log = $"{DateTime.Now}\n{text}\n\n{Log}";
        }

        private void AddLog(Exception ex)
        {
            string message = ex.Message;
            ex = ex.InnerException;
            while (ex != null)
            {
                message += $"\n\r ==> {ex.Message}";
                ex = ex.InnerException;
            }
            AddLog(message);
        }

        public void Close()
        {
            SaveTranslations();
        }

        private void LoadDefaultTranslations()
        {
            string path = Path.GetDirectoryName(typeof(MainViewModel).Assembly.Location);
            var file = XmlSerializerEx.Load<TranslationFile>(Path.Combine(path, "Translations.txt"));
            Translations = file.Translations;
        }

        private void SaveTranslations()
        {
            TranslationFile file = new TranslationFile
            {
                Translations = Translations
            };
            string path = Path.GetDirectoryName(typeof(MainViewModel).Assembly.Location);

            File.WriteAllText(Path.Combine(path, "Translations.txt"), file.Serialize());
        }

        #endregion

        #region Old

        private void Translate(string fileName)
        {
            Application winword = null;
            Document document = null;
            try
            {
                winword = new Application
                {
                    ShowAnimation = false,
                    Visible = false
                };

                document = winword.Documents.Open(fileName);

                var translations = GetTranslations();
                if (translations.Count > 0)
                {
                    foreach (var entry in translations)
                    {
                        SearchReplace(winword, (string)entry.Key, (string)entry.Value);
                    }
                    //foreach (Range sentence in document.Sentences)
                    //{
                    //    string text = sentence.Text.TrimEnd('\r', '\a', '\f');
                    //    if (!string.IsNullOrEmpty(text))
                    //    {
                    //        string translation = (string)translations[text];
                    //        if (translation != null)
                    //        {
                    //            sentence.Text = sentence.Text.Replace(text, translation);
                    //        }
                    //    }
                    //}

                    //foreach (Table table in document.Tables)
                    //{
                    //    table.
                    //    foreach (Row row in table.Rows)
                    //    {
                    //        foreach (Cell cell in row.Cells)
                    //        {
                    //            string text = cell.Range.Text.TrimEnd('\r', '\a', '\f');
                    //            if (!string.IsNullOrEmpty(text))
                    //            {
                    //                string translation = (string)translations[text];
                    //                if (translation != null)
                    //                {
                    //                    cell.Range.Text = cell.Range.Text.Replace(text, translation);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }

                string directory = Path.GetDirectoryName(fileName);
                string filename = Path.GetFileNameWithoutExtension(fileName);
                string extension = Path.GetExtension(filename);

                document.SaveAs(Path.Combine(directory, $"{filename}_vertaald{extension}"));

                AddLog($"Document {fileName} vertaald.");
            }
            catch (Exception e)
            {
                AddLog(e);
            }
            finally
            {
                document?.Close();

                winword?.Quit();

            }
        }

        private void SearchReplace(Application winword, string fromText, string toText)
        {
            winword.Selection.WholeStory();
            var text = winword.Selection.Range.Text;
            Find findObject = winword.Selection.Find;
            findObject.ClearFormatting();
            findObject.Text = fromText;
            findObject.MatchWholeWord = false;
            findObject.Replacement.ClearFormatting();
            findObject.Replacement.Text = toText;
            findObject.Format = false;

            object missing = null;
            object replaceAll = WdReplace.wdReplaceAll;
            bool done = findObject.Execute(ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref replaceAll, ref missing, ref missing, ref missing, ref missing);
        }

        public ICommand LoadTranslationsCommand
        {
            get { return new TargetCommand(LoadTranslations); }
        }

        private void LoadTranslations()
        {

        }

        public object ParseTranslationsCommand
        {
            get { return new TargetCommand(ParseTranslations); }
        }

        private void ParseTranslations()
        {
            Dictionary<string, string> translations = new Dictionary<string, string>();

            bool getNextFind = false;
            string findLine = null;
            bool getNextReplace = false;
            string replaceLine = null;

            foreach (string line in TranslationTexts.Lines())
            {
                if (line.StartsWith("FIND"))
                {
                    getNextFind = true;
                }
                else if (getNextFind)
                {
                    findLine = line;
                    getNextFind = false;
                }
                else if (line.StartsWith("REPLACE"))
                {
                    getNextReplace = true;
                }
                else if (getNextReplace)
                {
                    replaceLine = line;
                    getNextReplace = false;

                    string[] findParts = findLine.Split(',');
                    string[] replaceParts = replaceLine.Split(',');
                    for (int i = 0; i < findParts.Length; i++)
                    {
                        if (!translations.ContainsKey(findParts[i].TrimEnd().TrimStart()))
                        {
                            translations.Add(findParts[i].TrimEnd().TrimStart(),
                                replaceParts[i].TrimEnd().TrimStart());
                        }
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            TranslationCollection translationLists = new TranslationCollection();
            foreach (var key in translations.Keys.OrderBy(k => k))
            {
                string value = translations[key];
                sb.Append(key);
                sb.Append('\t');
                sb.AppendLine(value);

                translationLists.Add(new Translation(key, value));
            }

            Translations = translationLists;
            TranslationTexts = sb.ToString();
        }

        private Dictionary<string, string> GetTranslations()
        {
            Dictionary<string, string> translations = new Dictionary<string, string>();
            foreach (string line in TranslationTexts.Lines())
            {
                string[] parts = line.Split('\t');
                if (parts.Length == 2)
                {
                    string key = parts[0].TrimEnd().TrimStart();
                    if (!translations.ContainsKey(key))
                    {
                        translations.Add(key, parts[1].TrimEnd().TrimStart());
                    }
                }
                else if (!string.IsNullOrEmpty(parts[0]))
                {
                    string key = parts[0].TrimEnd().TrimStart();
                    if (!translations.ContainsKey(key))
                    {
                        translations.Add(key, "");
                    }
                }
            }
            return translations;
        }


        #endregion
    }
}