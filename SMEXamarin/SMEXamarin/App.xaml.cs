using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SMEXamarin.Views;

namespace SMEXamarin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            _serverUrl = "http://192.168.1.111/smewebapi";

            // MainPage = new NavigationPage(new MainPage());
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static string _serverUrl { get; set; }

        public static string _accessToken { get; set; }
    }
}
