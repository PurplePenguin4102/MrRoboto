using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Google.Cloud.Translation.V2;
using Google.Apis.Auth.OAuth2;
using System.Collections.ObjectModel;
using Windows.ApplicationModel;
using System.Threading.Tasks;
using Windows.Storage;
using System.Linq;

namespace MrRoboto
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly string _keyPath = @"jsonKey.json";
        private readonly string _vocabPath = @"vocab.txt";
        private TranslationClient client;
        private StorageFolder _local = ApplicationData.Current.LocalFolder;
        private HashSet<Phrase> _currentVocab = new HashSet<Phrase>();
        private bool _vocabLoaded = false;

        public async void TryLoadTranslator()
        {
            var exists = await _local.TryGetItemAsync(_keyPath);
            if (exists != null)
            {
                try
                {
                    client = TranslationClient.Create(GoogleCredential.FromFile(Path.Combine(_local.Path, _keyPath)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ClientExists"));
                }
                catch
                {
                    await exists.DeleteAsync();
                }
            }
        }

        public bool ClientExists { get => client != null; }

        public async Task SaveKey(string key)
        {
            using (var sw = new StreamWriter(new FileStream(Path.Combine(_local.Path, _keyPath), FileMode.OpenOrCreate)))
            {
                await sw.WriteLineAsync(key);
                await sw.FlushAsync();
            }
        }

        public void ClearScreen()
        {
            SavedTranslations.Clear();
        }

        public async Task SaveVocab()
        {
            _currentVocab.UnionWith(SavedTranslations);
            using (var sw = new StreamWriter(new FileStream(Path.Combine(_local.Path, _vocabPath), FileMode.OpenOrCreate)))
            {
                foreach (var ph in _currentVocab)
                {
                    await sw.WriteLineAsync($"{ph.Kana}={ph.Ego}");
                }
                await sw.FlushAsync();
            }
            ClearScreen();
        }

        public async Task LoadVocab()
        {
            if (!_vocabLoaded)
            {
                await GetPhrasesFromFile();
                _vocabLoaded = true;
            }

            SavedTranslations.Clear();
            var rnd = new Random();
            var ary = _currentVocab.ToArray();
            if (!ary.Any()) return;
            for (int i = 0; i < 15; i++)
            {
                var ph = ary[rnd.Next(ary.Length)];
                if (!SavedTranslations.Contains(ph))
                {
                    SavedTranslations.Add(ph);
                }
            }
            return;
        }

        public void AddPhrase(Phrase ph)
        {
            if (!SavedTranslations.Contains(ph))
            {
                SavedTranslations.Add(ph);
            }
            _currentVocab.Add(ph);
        }

        public void DeleteVocab()
        {
            _currentVocab.Except(SavedTranslations);
            ClearScreen();
        }

        public void DeletePhrase(Phrase ph)
        {
            SavedTranslations.Remove(ph);
            _currentVocab.Remove(ph);
        }

        private async Task GetPhrasesFromFile()
        {
            using (var sr = new StreamReader(new FileStream(Path.Combine(_local.Path, _vocabPath), FileMode.OpenOrCreate)))
            {
                while (!sr.EndOfStream)
                {
                    var ln = await sr.ReadLineAsync();
                    if (string.IsNullOrEmpty(ln)) continue;
                    var spl = ln.Split('=');
                    if (spl.Length == 2)
                    {
                        _currentVocab.Add(new Phrase { Kana = spl[0], Ego = spl[1] });
                    }
                }
            }
        }

        public ObservableCollection<Phrase> SavedTranslations { get; set; } = new ObservableCollection<Phrase>();

        public void Translate(string hiragana) => 
            Translation = string.IsNullOrEmpty(hiragana) ? "" 
                : client.TranslateText(hiragana, "en").TranslatedText;

        private string _translation;
        public string Translation
        {
            get => _translation;
            set
            {
                if (_translation != value)
                {
                    _translation = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
