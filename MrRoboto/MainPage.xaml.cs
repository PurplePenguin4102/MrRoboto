using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MrRoboto
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MonitorTranslation();
            
        }

        private async void MonitorTranslation()
        {
            var lastSeen = "";
            var vm = DataContext as MainViewModel;
            while (true)
            {
                await Task.Delay(1000);
                if (vm == null) return;
                if (lastSeen != Hiragana.Text && vm.ClientExists)
                {
                    lastSeen = Hiragana.Text;
                    vm.Translate(lastSeen);
                }
            }
        }

        private void AddKey_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Page2));
        }

        private void VocabSave_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void VocabLoad_OnClick(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var ctx = this.DataContext as MainViewModel;
            if (ctx != null && !DesignMode.DesignModeEnabled)
            {
                ctx.TryLoadTranslator();
            }
            base.OnNavigatedTo(e);
        }

        private void EnterKeyPressed(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                var vm = DataContext as MainViewModel;
                if (vm == null || string.IsNullOrEmpty(Ego.Text)) return;
                if (!vm.SavedTranslations.Contains($"{Hiragana.Text} : {Ego.Text}"))
                {
                    vm.SavedTranslations.Add($"{Hiragana.Text} : {Ego.Text}");
                }
            }
        }
    }
}
