using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using SMEXamarin;

namespace SMEXamarin.ViewModels
{
    class SettingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingViewModel()
        {
            HostAddress = App._serverUrl;

            SaveCommand = new Command(async () =>
            {
                App._serverUrl = HostAddress;

                await App.Current.MainPage.DisplayAlert("Host Address", "Host Address saved", "Close");
            });
        }

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        string hostAddress;

        public string HostAddress
        {
            get => hostAddress;

            set
            {
                if (hostAddress == value)
                    return;

                hostAddress = value;

                OnPropertyChanged(nameof(HostAddress));
            }
        }

        public Command SaveCommand { get; }
    }
}
