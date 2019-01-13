using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MrRoboto
{
    public sealed partial class Page2 : Page
    {
        public Page2()
        {
            this.InitializeComponent();
        }

        public async void AddKey_Click(object o, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainViewModel;
            if (vm == null) return;
            await vm.SaveKey(KeyText.Text);
            Frame.Navigate(typeof(MainPage));
        }
    }
}
