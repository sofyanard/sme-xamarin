using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using SMEXamarin.Views;

namespace SMEXamarin.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            CheckUserLogin();

            SimulasiCommand = new Command(async () =>
            {
                var simulasiVM = new SimulasiEntryViewModel();
                var simulasiPage = new SimulasiEntryPage();
                await Application.Current.MainPage.Navigation.PushAsync(simulasiPage);
            });

            NasabahCommand = new Command(async () =>
            {
                var nasabahVM = new NasabahViewModel();
                var nasabahPage = new NasabahPage();
                await Application.Current.MainPage.Navigation.PushAsync(nasabahPage);
            });

            SettingCommand = new Command(async () =>
            {
                var settingVM = new SettingViewModel();
                var settingPage = new SettingPage();
                await Application.Current.MainPage.Navigation.PushAsync(settingPage);
            });

            LogoutCommand = new Command(async () =>
            {
                RedirectToLoginPage();
            });

        }

        public Command SimulasiCommand { get; }
        public Command NasabahCommand { get; }
        public Command SettingCommand { get; }
        public Command LogoutCommand { get; }

        private string AccessToken { get; set; }

        string userName;

        public string UserName
        {
            get => userName;

            set
            {
                if (userName == value)
                    return;

                userName = value;

                OnPropertyChanged(nameof(UserName));
            }
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void CheckUserLogin()
        {
            AccessToken = App._accessToken;
            UserName = App._userName;

            if ((String.IsNullOrEmpty(AccessToken)) || (String.IsNullOrEmpty(UserName)))
            {
                RedirectToLoginPage();
            }
        }

        private async Task RedirectToLoginPage()
        {
            var loginVM = new LoginViewModel();
            var loginPage = new LoginPage();
            await Application.Current.MainPage.Navigation.PushAsync(loginPage);
        }
    }
}
