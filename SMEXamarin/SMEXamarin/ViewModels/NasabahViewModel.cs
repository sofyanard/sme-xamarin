using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using SMEXamarin.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Linq;
using System.Threading;

namespace SMEXamarin.ViewModels
{
    class NasabahViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly HttpClient _client = new HttpClient();

        private string baseApiUrl = App._serverUrl + "/api";

        DateTime today = DateTime.Now;

        public NasabahViewModel()
        {
            RequestParameters();

            TanggalLahir = today;

            GetNasabahId();

            SaveCommand = new Command(async () =>
            {
                SaveNasabah();
            });

            TempLabel = "Hello World!";
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string namaLengkap;

        public string NamaLengkap
        {
            get => namaLengkap;

            set
            {
                if (namaLengkap == value)
                    return;

                namaLengkap = value;

                OnPropertyChanged(nameof(NamaLengkap));
            }
        }

        List<RfSex> listRfSex;

        public List<RfSex> ListRfSex
        {
            get => listRfSex;

            set
            {
                if (listRfSex == value)
                    return;

                listRfSex = value;

                OnPropertyChanged(nameof(ListRfSex));
            }
        }

        RfSex selectedRfSex;

        public RfSex SelectedRfSex
        {
            get => selectedRfSex;

            set
            {
                if (selectedRfSex == value)
                    return;

                selectedRfSex = value;

                OnPropertyChanged(nameof(SelectedRfSex));

                JenisKelamin = (SelectedRfSex != null) ? SelectedRfSex.SexId : String.Empty;
            }
        }

        string JenisKelamin { get; set; }

        string tempatLahir;

        public string TempatLahir
        {
            get => tempatLahir;

            set
            {
                if (tempatLahir == value)
                    return;

                tempatLahir = value;

                OnPropertyChanged(nameof(TempatLahir));
            }
        }

        DateTime tanggalLahir;

        public DateTime TanggalLahir
        {
            get => tanggalLahir;

            set
            {
                if (tanggalLahir == value)
                    return;

                tanggalLahir = value;

                OnPropertyChanged(nameof(TanggalLahir));
            }
        }

        string noIdentitas;

        public string NoIdentitas
        {
            get => noIdentitas;

            set
            {
                if (noIdentitas == value)
                    return;

                noIdentitas = value;

                OnPropertyChanged(nameof(NoIdentitas));
            }
        }

        string alamatRumah;

        public string AlamatRumah
        {
            get => alamatRumah;

            set
            {
                if (alamatRumah == value)
                    return;

                alamatRumah = value;

                OnPropertyChanged(nameof(AlamatRumah));
            }
        }

        ObservableCollection<RefPropinsi> listRefPropinsi;

        public ObservableCollection<RefPropinsi> ListRefPropinsi
        {
            get => listRefPropinsi;

            set
            {
                if (listRefPropinsi == value)
                    return;

                listRefPropinsi = value;

                OnPropertyChanged(nameof(ListRefPropinsi));
            }
        }

        RefPropinsi selectedRefPropinsi;

        public RefPropinsi SelectedRefPropinsi
        {
            get => selectedRefPropinsi;

            set
            {
                if (selectedRefPropinsi == value)
                    return;

                selectedRefPropinsi = value;

                OnPropertyChanged(nameof(SelectedRefPropinsi));

                PropinsiRumah = (SelectedRefPropinsi != null) ? SelectedRefPropinsi.Id : String.Empty;

                if ((SelectedRefPropinsi != null) && (!String.IsNullOrEmpty(SelectedRefPropinsi.Id)))
                {
                    RequestKotaKabupaten(SelectedRefPropinsi.Id);
                }
                else
                {
                    SelectedRefKotaKab = null;
                    ListRefKotaKab.Clear();
                }
            }
        }

        string PropinsiRumah { get; set; }

        ObservableCollection<RefKotaKab> listRefKotaKab;

        public ObservableCollection<RefKotaKab> ListRefKotaKab
        {
            get => listRefKotaKab;

            set
            {
                if (listRefKotaKab == value)
                    return;

                listRefKotaKab = value;

                OnPropertyChanged(nameof(ListRefKotaKab));
            }
        }

        RefKotaKab selectedRefKotaKab;

        public RefKotaKab SelectedRefKotaKab
        {
            get => selectedRefKotaKab;

            set
            {
                if (selectedRefKotaKab == value)
                    return;

                selectedRefKotaKab = value;

                OnPropertyChanged(nameof(SelectedRefKotaKab));

                KotaKabRumah = (SelectedRefKotaKab != null) ? SelectedRefKotaKab.Id : String.Empty;

                if ((SelectedRefKotaKab != null) && (!String.IsNullOrEmpty(SelectedRefKotaKab.Id)))
                {
                    RequestKecamatan(SelectedRefKotaKab.Id);
                }
                else
                {
                    SelectedRefKecamatan = null;
                    ListRefKecamatan.Clear();
                }
            }
        }

        string KotaKabRumah { get; set; }

        ObservableCollection<RefKecamatan> listRefKecamatan;

        public ObservableCollection<RefKecamatan> ListRefKecamatan
        {
            get => listRefKecamatan;

            set
            {
                if (listRefKecamatan == value)
                    return;

                listRefKecamatan = value;

                OnPropertyChanged(nameof(ListRefKecamatan));
            }
        }

        RefKecamatan selectedRefKecamatan;

        public RefKecamatan SelectedRefKecamatan
        {
            get => selectedRefKecamatan;

            set
            {
                if (selectedRefKecamatan == value)
                    return;

                selectedRefKecamatan = value;

                OnPropertyChanged(nameof(SelectedRefKecamatan));

                KecamatanRumah = (SelectedRefKecamatan != null) ? SelectedRefKecamatan.Id : String.Empty;

                if ((SelectedRefKecamatan != null) && (!String.IsNullOrEmpty(SelectedRefKecamatan.Id)))
                {
                    RequestKelurahan(SelectedRefKecamatan.Id);
                }
                else
                {
                    SelectedRefKelurahan = null;
                    ListRefKelurahan.Clear();
                }
            }
        }

        string KecamatanRumah { get; set; }

        ObservableCollection<RefKelurahan> listRefKelurahan;

        public ObservableCollection<RefKelurahan> ListRefKelurahan
        {
            get => listRefKelurahan;

            set
            {
                if (listRefKelurahan == value)
                    return;

                listRefKelurahan = value;

                OnPropertyChanged(nameof(ListRefKelurahan));
            }
        }

        RefKelurahan selectedRefKelurahan;

        public RefKelurahan SelectedRefKelurahan
        {
            get => selectedRefKelurahan;

            set
            {
                if (selectedRefKelurahan == value)
                    return;

                selectedRefKelurahan = value;

                OnPropertyChanged(nameof(SelectedRefKelurahan));

                KelurahanRumah = (SelectedRefKelurahan != null) ? SelectedRefKelurahan.Id : String.Empty;
            }
        }

        string KelurahanRumah { get; set; }

        string kodePosRumah;

        public string KodePosRumah
        {
            get => kodePosRumah;

            set
            {
                if (kodePosRumah == value)
                    return;

                kodePosRumah = value;

                OnPropertyChanged(nameof(KodePosRumah));
            }
        }

        string teleponRumah;

        public string TeleponRumah
        {
            get => teleponRumah;

            set
            {
                if (teleponRumah == value)
                    return;

                teleponRumah = value;

                OnPropertyChanged(nameof(TeleponRumah));
            }
        }

        string teleponGenggam;

        public string TeleponGenggam
        {
            get => teleponGenggam;

            set
            {
                if (teleponGenggam == value)
                    return;

                teleponGenggam = value;

                OnPropertyChanged(nameof(TeleponGenggam));
            }
        }

        string namaIbuKandung;

        public string NamaIbuKandung
        {
            get => namaIbuKandung;

            set
            {
                if (namaIbuKandung == value)
                    return;

                namaIbuKandung = value;

                OnPropertyChanged(nameof(NamaIbuKandung));
            }
        }

        string tempLabel;

        public string TempLabel
        {
            get => tempLabel;

            set
            {
                if (tempLabel == value)
                    return;

                tempLabel = value;

                OnPropertyChanged(nameof(TempLabel));
            }
        }

        async private Task RequestParameters()
        {
            // Jenis Kelamin
            string rfSexApiUrl = baseApiUrl + "/parameter/sex";
            try
            {
                string rfSexApiContent = await _client.GetStringAsync(rfSexApiUrl);
                ListRfSex = JsonConvert.DeserializeObject<List<RfSex>>(rfSexApiContent);
            }
            catch (Exception ex) { await App.Current.MainPage.DisplayAlert("Request Error", ex.Message, "Close"); }

            // Propinsi
            string propApiUrl = baseApiUrl + "/parameter/propinsi";
            try
            {
                string propApiContent = await _client.GetStringAsync(propApiUrl);
                ListRefPropinsi = JsonConvert.DeserializeObject<ObservableCollection<RefPropinsi>>(propApiContent);
            }
            catch (Exception ex) { await App.Current.MainPage.DisplayAlert("Request RefPropinsi Error", ex.Message, "Close"); }

        }

        private async Task RequestKotaKabupaten(string propinsiId)
        {
            string kotakabApiUrl = baseApiUrl + "/parameter/kotakabupaten/" + propinsiId;

            try
            {
                string kotakabApiContent = await _client.GetStringAsync(kotakabApiUrl);
                ListRefKotaKab = JsonConvert.DeserializeObject<ObservableCollection<RefKotaKab>>(kotakabApiContent);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Request RefKotaKabupaten Error", ex.Message, "Close");
            }
        }

        private async Task RequestKecamatan(string kotakabId)
        {
            string kecamatanApiUrl = baseApiUrl + "/parameter/kecamatan/" + kotakabId;

            try
            {
                string kecamatanApiContent = await _client.GetStringAsync(kecamatanApiUrl);
                ListRefKecamatan = JsonConvert.DeserializeObject<ObservableCollection<RefKecamatan>>(kecamatanApiContent);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Request RefKecamatan Error", ex.Message, "Close");
            }
        }

        private async Task RequestKelurahan(string kecamatanId)
        {
            string kelurahanApiUrl = baseApiUrl + "/parameter/kelurahan/" + kecamatanId;
            try
            {
                string kelurahanApiContent = await _client.GetStringAsync(kelurahanApiUrl);
                ListRefKelurahan = JsonConvert.DeserializeObject<ObservableCollection<RefKelurahan>>(kelurahanApiContent);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Request RefKelurahan Error", ex.Message, "Close");
            }
        }

        string nasabahId = null;
        Nasabah nasabah = null;

        private async Task GetNasabahId()
        {
            string getNasabahIdApiUrl = baseApiUrl + "/nasabah/getid";
            try
            {
                var authHeader = new AuthenticationHeaderValue("Bearer", App._accessToken);
                _client.DefaultRequestHeaders.Authorization = authHeader;

                var result = await _client.GetAsync(getNasabahIdApiUrl);
                string resultContent = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    nasabahId = resultContent;

                    if (!String.IsNullOrEmpty(nasabahId))
                    {
                        GetNasabah(nasabahId);
                    }
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
                await App.Current.MainPage.DisplayAlert("Request NasabahId Error", ex.Message, "Close");
            }
        }

        private async Task GetNasabah(string nasabahId)
        {
            string getNasabahApiUrl = baseApiUrl + "/nasabah/" + nasabahId;
            try
            {
                var authHeader = new AuthenticationHeaderValue("Bearer", App._accessToken);
                _client.DefaultRequestHeaders.Authorization = authHeader;
                string nasabahContent = await _client.GetStringAsync(getNasabahApiUrl);
                nasabah = JsonConvert.DeserializeObject<Nasabah>(nasabahContent);

                if (nasabah != null)
                {
                    ViewNasabah(nasabahId);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Request Nasabah Error", ex.Message, "Close");
            }
        }

        private async Task ViewNasabah(string nasabahId)
        {
            NamaLengkap = nasabah.NamaLengkap;
            JenisKelamin = nasabah.JenisKelamin;
            try
            {
                SelectedRfSex = ListRfSex.FirstOrDefault(p => p.SexId == JenisKelamin);
            }
            catch { }
            TempatLahir = nasabah.TempatLahir;
            TanggalLahir = (DateTime)nasabah.TanggalLahir;
            NoIdentitas = nasabah.NoIdentitas;
            AlamatRumah = nasabah.AlamatRumah;
            
            PropinsiRumah = nasabah.PropinsiRumah;
            try
            {
                SelectedRefPropinsi = ListRefPropinsi.FirstOrDefault(p => p.Id == PropinsiRumah);
            }
            catch (Exception ex) { await App.Current.MainPage.DisplayAlert("Selected Propinsi Error", ex.Message, "Close"); }
            // RequestKotaKabupaten(PropinsiRumah);
            KotaKabRumah = nasabah.KotaKabRumah;
            try
            {
                SelectedRefKotaKab = ListRefKotaKab.FirstOrDefault(p => p.Id == KotaKabRumah);
    }
            catch (Exception ex) { await App.Current.MainPage.DisplayAlert("Selected Kabupaten Error", ex.Message, "Close"); }
            // RequestKecamatan(KotaKabRumah);
            KecamatanRumah = nasabah.KecamatanRumah;
            try
            {
                SelectedRefKecamatan = ListRefKecamatan.FirstOrDefault(p => p.Id == KecamatanRumah);
            }
            catch (Exception ex) { await App.Current.MainPage.DisplayAlert("Selected Kecamatan Error", ex.Message, "Close"); }
            // RequestKelurahan(KecamatanRumah);
            KelurahanRumah = nasabah.KelurahanRumah;
            try
            {
                SelectedRefKelurahan = ListRefKelurahan.FirstOrDefault(p => p.Id == KelurahanRumah);
            }
            catch (Exception ex) { await App.Current.MainPage.DisplayAlert("Selected Kelurahan Error", ex.Message, "Close"); }

            KodePosRumah = nasabah.KodePosRumah;
            TeleponRumah = nasabah.TeleponRumah;
            TeleponGenggam = nasabah.TeleponGenggam;
            NamaIbuKandung = nasabah.NamaIbuKandung;
        }

        public Command SaveCommand { get; }

        private async Task SaveNasabah()
        {
            if (String.IsNullOrEmpty(nasabahId))
            {
                // POST

                nasabah = new Nasabah();
                
                nasabah.NamaLengkap = NamaLengkap;
                nasabah.JenisKelamin = JenisKelamin;
                nasabah.TempatLahir = TempatLahir;
                nasabah.TanggalLahir = TanggalLahir;
                nasabah.NoIdentitas = NoIdentitas;
                nasabah.AlamatRumah = AlamatRumah;
                nasabah.PropinsiRumah = PropinsiRumah;
                nasabah.KotaKabRumah = KotaKabRumah;
                nasabah.KecamatanRumah = KecamatanRumah;
                nasabah.KelurahanRumah = KelurahanRumah;
                nasabah.KodePosRumah = kodePosRumah;
                nasabah.TeleponRumah = TeleponRumah;
                nasabah.TeleponGenggam = TeleponGenggam;
                nasabah.NamaIbuKandung = NamaIbuKandung;

                // nasabah.Pendidikan = "7";
                // nasabah.StatusPerkawinan = "1";
                // nasabah.Kewarganegaraan = "1";
                // nasabah.StatusRumah = "1";

                string postNasabahApiUrl = baseApiUrl + "/nasabah/";
                try
                {
                    var serializeObject = JsonConvert.SerializeObject(nasabah);
                    StringContent stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");

                    // TempLabel = stringContent.ToString();

                    var authHeader = new AuthenticationHeaderValue("Bearer", App._accessToken);
                    _client.DefaultRequestHeaders.Authorization = authHeader;

                    var result = await _client.PostAsync(postNasabahApiUrl, stringContent);
                    string resultContent = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode)
                    {
                        await App.Current.MainPage.DisplayAlert("Success", "Data berhasil disimpan", "Close");
                        TempLabel = resultContent;
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(result.StatusCode.ToString(), resultContent, "Close");
                        TempLabel = resultContent;
                    }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Request Error", ex.Message, "Close");
                }
            }
            else
            {
                // PUT

                nasabah.Id = int.Parse(nasabahId);
                nasabah.NamaLengkap = NamaLengkap;
                nasabah.JenisKelamin = JenisKelamin;
                nasabah.TempatLahir = TempatLahir;
                nasabah.TanggalLahir = TanggalLahir;
                nasabah.NoIdentitas = NoIdentitas;
                nasabah.AlamatRumah = AlamatRumah;
                nasabah.PropinsiRumah = PropinsiRumah;
                nasabah.KotaKabRumah = KotaKabRumah;
                nasabah.KecamatanRumah = KecamatanRumah;
                nasabah.KelurahanRumah = KelurahanRumah;
                nasabah.KodePosRumah = kodePosRumah;
                nasabah.TeleponRumah = TeleponRumah;
                nasabah.TeleponGenggam = TeleponGenggam;
                nasabah.NamaIbuKandung = NamaIbuKandung;

                // nasabah.Pendidikan = "7";
                // nasabah.StatusPerkawinan = "1";
                // nasabah.Kewarganegaraan = "1";
                // nasabah.StatusRumah = "1";

                string putNasabahApiUrl = baseApiUrl + "/nasabah/putbasic/" + nasabahId;
                try
                {
                    var serializeObject = JsonConvert.SerializeObject(nasabah);
                    StringContent stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");

                    // TempLabel = stringContent.ToString();

                    var authHeader = new AuthenticationHeaderValue("Bearer", App._accessToken);
                    _client.DefaultRequestHeaders.Authorization = authHeader;

                    var result = await _client.PutAsync(putNasabahApiUrl, stringContent);
                    string resultContent = await result.Content.ReadAsStringAsync();

                    if (result.IsSuccessStatusCode)
                    {
                        await App.Current.MainPage.DisplayAlert("Success", "Data berhasil disimpan", "Close");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(result.StatusCode.ToString(), resultContent, "Close");
                        TempLabel = resultContent;
                    }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Request Error", ex.Message, "Close");
                }

            }
        }
    }
}
