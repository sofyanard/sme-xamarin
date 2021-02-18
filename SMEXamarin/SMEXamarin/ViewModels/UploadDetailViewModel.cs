using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using SMEXamarin.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SMEXamarin.ViewModels
{
    public class UploadDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly HttpClient _client = new HttpClient();

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UploadDetailViewModel()
        {

        }

        public UploadDetailViewModel(NasabahUploadResponse selectedFile)
        {
            ViewDetail(selectedFile);
            
            DeleteCommand = new Command(async () =>
            {
                DeleteFile(selectedFile);
            });
        }

        string fileName;

        public string FileName
        {
            get => fileName;

            set
            {
                if (fileName == value)
                    return;

                fileName = value;

                OnPropertyChanged(nameof(FileName));
            }
        }

        string caption;

        public string Caption
        {
            get => caption;

            set
            {
                if (caption == value)
                    return;

                caption = value;

                OnPropertyChanged(nameof(Caption));
            }
        }

        ImageSource theImage;

        public ImageSource TheImage
        {
            get => theImage;

            set
            {
                if (theImage == value)
                    return;

                theImage = value;

                OnPropertyChanged(nameof(TheImage));
            }
        }

        private void ViewDetail(NasabahUploadResponse selectedFile)
        {
            TheImage = selectedFile.DownloadUrl;
            Caption = selectedFile.Caption;
            FileName = selectedFile.FileName;
        }

        public Command DeleteCommand { get; }

        private async void DeleteFile(NasabahUploadResponse selectedFile)
        {
            string deleteUrl = App._serverUrl + "/api/nasabahupload/" + selectedFile.Id.ToString();

            var authHeader = new AuthenticationHeaderValue("Bearer", App._accessToken);
            _client.DefaultRequestHeaders.Authorization = authHeader;

            try
            {
                var result = await _client.DeleteAsync(deleteUrl);
                string resultContent = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert(result.StatusCode.ToString(), "Deleted successfully", "Close");
                    MessagingCenter.Send(this, "FileDeleted");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(result.StatusCode.ToString(), resultContent, "Close");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Request Error", ex.Message, "Close");
            }
        }
    }
}
