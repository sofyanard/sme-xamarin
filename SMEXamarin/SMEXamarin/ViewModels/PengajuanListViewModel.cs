using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Xamarin.Forms;
using SMEXamarin.Views;
using SMEXamarin.Models;
using System.Collections.ObjectModel;

namespace SMEXamarin.ViewModels
{
    public class PengajuanListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly HttpClient _client = new HttpClient();

        private string getAllUrl = App._serverUrl + "/api/pengajuan";

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PengajuanListViewModel()
        {
            NewCommand = new Command(async () =>
            {
                var pengajuanNewVM = new PengajuanNewViewModel();
                var pengajuanNewPage = new PengajuanNewPage();
                await Application.Current.MainPage.Navigation.PushAsync(pengajuanNewPage);
            });

            GetAll();

            MessagingCenter.Subscribe<PengajuanNewViewModel>(this, "PengajuanSubmitted", (sender) =>
            {
                GetAll();
            });
        }

        public Command NewCommand { get; }

        ObservableCollection<Pengajuan> listPengajuan;

        public ObservableCollection<Pengajuan> ListPengajuan
        {
            get => listPengajuan;

            set
            {
                if (listPengajuan == value)
                    return;

                listPengajuan = value;

                OnPropertyChanged(nameof(ListPengajuan));
            }
        }

        Pengajuan selectedPengajuan;

        public Pengajuan SelectedPengajuan
        {
            get => selectedPengajuan;

            set
            {
                if (selectedPengajuan == value)
                    return;

                selectedPengajuan = value;

                OnPropertyChanged(nameof(SelectedPengajuan));

                if (SelectedPengajuan != null)
                {
                    GoToDetail(SelectedPengajuan.Id.ToString());
                }
            }
        }

        private async void GetAll()
        {
            try
            {
                var authHeader = new AuthenticationHeaderValue("Bearer", App._accessToken);
                _client.DefaultRequestHeaders.Authorization = authHeader;

                var result = await _client.GetAsync(getAllUrl);
                string resultContent = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    ListPengajuan = JsonConvert.DeserializeObject<ObservableCollection<Pengajuan>>(resultContent);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(result.StatusCode.ToString(), resultContent, "Close");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Close");
            }
        }

        private async void GoToDetail(string id)
        {
            var pengajuanDetailVM = new PengajuanDetailViewModel(id);
            var pengajuanDetailPage = new PengajuanDetailPage(id);
            await Application.Current.MainPage.Navigation.PushAsync(pengajuanDetailPage);
        }

    }
}
