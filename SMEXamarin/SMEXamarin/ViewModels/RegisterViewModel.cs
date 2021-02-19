using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using SMEXamarin.Models;
using SMEXamarin.Views;
using Newtonsoft.Json;

namespace SMEXamarin.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly HttpClient _client = new HttpClient();

        private string registerUrl = App._serverUrl + "/api/account/register";

        public RegisterViewModel()
        {
            LoginCommand = new Command(async () =>
            {
                var loginVM = new LoginViewModel();
                var loginPage = new LoginPage();
                await Application.Current.MainPage.Navigation.PushAsync(loginPage);
            });

            RegisterCommand = new Command(async () =>
            {
                ExecuteRegister();
            });
        }

        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        string email;

        public string Email
        {
            get => email;

            set
            {
                if (email == value)
                    return;

                email = value;

                OnPropertyChanged(nameof(Email));
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

        string confirmPassword;

        public string ConfirmPassword
        {
            get => confirmPassword;

            set
            {
                if (confirmPassword == value)
                    return;

                confirmPassword = value;

                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        async private Task ExecuteRegister()
        {
            RegisterRequest registerData = new RegisterRequest();
            registerData.Email = Email;
            registerData.Password = Password;
            registerData.ConfirmPassword = ConfirmPassword;

            try
            {
                var serializeObject = JsonConvert.SerializeObject(registerData);
                StringContent stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");

                var result = await _client.PostAsync(registerUrl, stringContent);
                string resultContent = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert("Register Success", "Please Login", "Close");

                    // Back To Login Page
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(result.StatusCode.ToString(), resultContent, "Close");
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
