using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SMEXamarin.Views;
using SMEXamarin.Models;

namespace SMEXamarin.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly HttpClient _client = new HttpClient();

        private string apiUrl = App._serverUrl + "/token";

        public LoginViewModel()
        {
            LoginCommand = new Command(async () =>
            {
                ExecuteLogin();
            });

            SettingCommand = new Command(async () =>
            {
                var settingVM = new SettingViewModel();
                var settingPage = new SettingPage();
                await Application.Current.MainPage.Navigation.PushAsync(settingPage);
            });
        }

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        string username;

        public string Username
        {
            get => username;

            set
            {
                if (username == value)
                    return;

                username = value;

                OnPropertyChanged(nameof(Username));
            }
        }

        string password;

        public string Password
        {
            get => password;

            set
            {
                if (password == value)
                    return;

                password = value;

                OnPropertyChanged(nameof(Password));
            }
        }

        string response;

        public string Response
        {
            get => response;

            set
            {
                if (response == value)
                    return;

                response = value;

                OnPropertyChanged(nameof(Response));
            }
        }

        public Command LoginCommand { get; }

        public Command SettingCommand { get; }

        async private Task ExecuteLogin()
        {
            var loginData = new Dictionary<string, string>
               {
                   {"grant_type", "password"},
                   {"username", Username},
                   {"password", Password},
               };

            try
            {
                var result = await _client.PostAsync(apiUrl, new FormUrlEncodedContent(loginData));
                string responseBody = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    LoginResponse loginResponse = new LoginResponse();
                    loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseBody);

                    App._accessToken = loginResponse.access_token;

                    // Go To Main Page
                    var mainVM = new MainViewModel();
                    var mainPage = new MainPage();
                    await Application.Current.MainPage.Navigation.PushAsync(mainPage);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Result", result.StatusCode.ToString(), "Close");
                    return;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Close");
                return;
            }
        }
    }
}
