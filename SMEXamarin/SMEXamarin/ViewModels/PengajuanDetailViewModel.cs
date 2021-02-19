using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using SMEXamarin.Models;

namespace SMEXamarin.ViewModels
{
    public class PengajuanDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly HttpClient _client = new HttpClient();

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PengajuanDetailViewModel()
        {

        }

        public PengajuanDetailViewModel(string id)
        {
            GetDetail(id);
        }

        Pengajuan bindPengajuan;

        public Pengajuan BindPengajuan
        {
            get => bindPengajuan;

            set
            {
                if (bindPengajuan == value)
                    return;

                bindPengajuan = value;

                OnPropertyChanged(nameof(BindPengajuan));
            }
        }

        List<TrackHistory> bindHistory;

        public List<TrackHistory> BindHistory
        {
            get => bindHistory;

            set
            {
                if (bindHistory == value)
                    return;

                bindHistory = value;

                OnPropertyChanged(nameof(BindHistory));
            }
        }

        private async void GetDetail(string id)
        {
            string getDetailUrl = App._serverUrl + "/api/pengajuan/" + id;

            try
            {
                var authHeader = new AuthenticationHeaderValue("Bearer", App._accessToken);
                _client.DefaultRequestHeaders.Authorization = authHeader;

                var result = await _client.GetAsync(getDetailUrl);
                string resultContent = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    Pengajuan pengajuan = JsonConvert.DeserializeObject<Pengajuan>(resultContent);

                    BindPengajuan = pengajuan;

                    if (!String.IsNullOrEmpty(pengajuan.LosApRegno))
                    {
                        GetHistory(id);
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(result.StatusCode.ToString(), resultContent, "Close");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Get Detail Error", ex.Message, "Close");
            }
        }

        private async void GetHistory(string id)
        {
            string getHistorylUrl = App._serverUrl + "/api/pengajuan/" + id + "/history";

            try
            {
                var authHeader = new AuthenticationHeaderValue("Bearer", App._accessToken);
                _client.DefaultRequestHeaders.Authorization = authHeader;

                var result = await _client.GetAsync(getHistorylUrl);
                string resultContent = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    List<TrackHistory> listHistory = JsonConvert.DeserializeObject<List<TrackHistory>>(resultContent);

                    BindHistory = listHistory;
                }
                else
                {
                    if (!result.StatusCode.ToString().Equals("NotFound"))
                    {
                        await App.Current.MainPage.DisplayAlert(result.StatusCode.ToString(), resultContent, "Close");
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Get History Error", ex.Message, "Close");
            }
        }
    }
}
