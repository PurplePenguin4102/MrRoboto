using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Google.Cloud.Translation.V2;
using Google.Apis.Auth.OAuth2;
using System.Collections.ObjectModel;
using Windows.ApplicationModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace MrRoboto
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly string _keyPath = @"jsonKey.json";
        private TranslationClient client;

        public async void TryLoadTranslator()
        {
            var fld = ApplicationData.Current.LocalFolder;
            var exists = await fld.TryGetItemAsync(_keyPath);
            if (exists != null)
            {
                try
                {
                    client = TranslationClient.Create(GoogleCredential.FromFile(Path.Combine(fld.Path, _keyPath)));
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
            var fld = ApplicationData.Current.LocalFolder;
            using (var sw = new StreamWriter(new FileStream(Path.Combine(fld.Path, _keyPath), FileMode.OpenOrCreate)))
            {
                await sw.WriteLineAsync(key);
                await sw.FlushAsync();
            }
        }

        public ObservableCollection<string> SavedTranslations { get; set; } = new ObservableCollection<string>();

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
