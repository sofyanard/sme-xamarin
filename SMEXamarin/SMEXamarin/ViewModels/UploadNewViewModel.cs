using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SMEXamarin.ViewModels
{
    class UploadNewViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly HttpClient _client = new HttpClient();

        private string uploadUrl = App._serverUrl + "/api/nasabahupload";

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UploadNewViewModel()
        {
            PickPhotoCommand = new Command(async () =>
            {
                PickPhoto();
            });

            UploadCommand = new Command(async () =>
            {
                Upload();
            });
        }

        public Command PickPhotoCommand { get; }

        public Command UploadCommand { get; }

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

        FileResult uploadFile;

        private async void PickPhoto()
        {
            uploadFile = null;
            uploadFile = await MediaPicker.PickPhotoAsync();

            if (uploadFile == null)
                return;

            var stream = await uploadFile.OpenReadAsync();
            TheImage = ImageSource.FromStream(() => stream);


        }

        private async void Upload()
        {
            if ((uploadFile == null) || (String.IsNullOrEmpty(Caption)))
            {
                await App.Current.MainPage.DisplayAlert("Invalid Input", "File and Caption are required", "Close");
                return;
            }

            try
            {
                var stream = await uploadFile.OpenReadAsync();

                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(stream), "file", uploadFile.FileName);
                content.Add(new StringContent(Caption), "caption");

                var authHeader = new AuthenticationHeaderValue("Bearer", App._accessToken);
                _client.DefaultRequestHeaders.Authorization = authHeader;

                var result = await _client.PostAsync(uploadUrl, content);
                string resultContent = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert(result.StatusCode.ToString(), "Upload success", "Close");
                    MessagingCenter.Send(this, "FileAdded");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(result.StatusCode.ToString(), resultContent, "Close");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Upload Error", ex.Message, "Close");
            }
        }
    }
}
