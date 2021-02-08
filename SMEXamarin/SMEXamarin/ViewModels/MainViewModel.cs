using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using SMEXamarin.Views;

namespace SMEXamarin.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            SimulasiCommand = new Command(async () =>
            {
                var simulasiVM = new SimulasiEntryViewModel();
                var simulasiPage = new SimulasiEntryPage();
                await Application.Current.MainPage.Navigation.PushAsync(simulasiPage);
            });

            SettingCommand = new Command(async () =>
            {
                var settingVM = new SettingViewModel();
                var settingPage = new SettingPage();
                await Application.Current.MainPage.Navigation.PushAsync(settingPage);
            });
        }

        public Command SimulasiCommand { get; }
        public Command SettingCommand { get; }
    }
}
