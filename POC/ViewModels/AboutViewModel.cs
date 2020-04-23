using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace POC.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://github.com/candrewahr"));
        }

        public ICommand OpenWebCommand { get; }
    }
}