using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using SMEXamarin.Views;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using SMEXamarin.Models;

namespace SMEXamarin.ViewModels
{
    class UploadListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly HttpClient _client = new HttpClient();

        private string uploadListUrl = App._serverUrl + "/api/nasabahupload";

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UploadListViewModel()
        {
            UploadNewCommand = new Command(async () =>
            {
                var uploadNewVM = new UploadNewViewModel();
                var uploadNewPage = new UploadNewPage();
                await Application.Current.MainPage.Navigation.PushAsync(uploadNewPage);
            });

            ViewUploads();

            if (SelectedFile != null)
                SelectedFile = null;

            TempText = "Hello";

            MessagingCenter.Subscribe<UploadNewViewModel>(this, "FileAdded", (sender) => 
            {
                ViewUploads();
            });

            MessagingCenter.Subscribe<UploadDetailViewModel>(this, "FileDeleted", (sender) =>
            {
                ViewUploads();
            });
        }

        public Command UploadNewCommand { get; }

        ObservableCollection<NasabahUploadResponse> listUpload;

        public ObservableCollection<NasabahUploadResponse> ListUpload
        {
            get => listUpload;

            set
            {
                if (listUpload == value)
                    return;

                listUpload = value;

                OnPropertyChanged(nameof(ListUpload));
            }
        }

        private async void ViewUploads()
        {
            try
            {
                var authHeader = new AuthenticationHeaderValue("Bearer", App._accessToken);
                _client.DefaultRequestHeaders.Authorization = authHeader;

                string listUploadContent = await _client.GetStringAsync(uploadListUrl);
                ListUpload = JsonConvert.DeserializeObject<ObservableCollection<NasabahUploadResponse>>(listUploadContent);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Request Nasabah Error", ex.Message, "Close");
            }
        }

        string tempText;

        public string TempText
        {
            get => tempText;

            set
            {
                if (tempText == value)
                    return;

                tempText = value;

                OnPropertyChanged(nameof(TempText));
            }
        }

        NasabahUploadResponse selectedFile;

        public NasabahUploadResponse SelectedFile
        {
            get => selectedFile;

            set
            {
                if (selectedFile == value)
                    return;

                selectedFile = value;

                OnPropertyChanged(nameof(SelectedFile));

                SelectedFileChanged();
            }
        }

        private async void SelectedFileChanged()
        {
            TempText = SelectedFile.Id.ToString();

            var uploadDetailVM = new UploadDetailViewModel(SelectedFile);
            var uploadDetailPage = new UploadDetailPage(SelectedFile);
            await Application.Current.MainPage.Navigation.PushAsync(uploadDetailPage);
        }
    }
}
