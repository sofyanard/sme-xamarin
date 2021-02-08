using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Xamarin.Forms;
using Newtonsoft.Json;
using SMEXamarin;
using SMEXamarin.Models;

namespace SMEXamarin.ViewModels
{
    class SimulasiEntryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly HttpClient _client = new HttpClient();

        private string apiUrl = App._serverUrl + "/api/simulasi";

        public SimulasiEntryViewModel()
        {
            BasedOnSelect = "BasedOnPlafond";
            Amount = "1000000";
            Interest = "15";
            InterestTypeSelect = "Anuitas";
            Tenor = "12";

            SubmitCommand = new Command(() =>
            {
                ExecuteSubmit();
            });
        }

        /* Based On */
        
        string basedOnSelect;

        public string BasedOnSelect
        {
            get => basedOnSelect;

            set
            {
                if (basedOnSelect == value)
                    return;

                basedOnSelect = value;

                OnPropertyChanged(nameof(BasedOnSelect));
            }
        }

        /* Amount */

        string amount;

        public string Amount
        {
            get => amount;

            set
            {
                if (amount == value)
                    return;

                amount = value;

                OnPropertyChanged(nameof(Amount));
            }
        }

        /* Interest */

        string interest;

        public string Interest
        {
            get => interest;

            set
            {
                if (interest == value)
                    return;

                interest = value;

                OnPropertyChanged(nameof(Interest));
            }
        }

        /* Interest Type */

        string interestTypeSelect;

        public string InterestTypeSelect
        {
            get => interestTypeSelect;

            set
            {
                if (interestTypeSelect == value)
                    return;

                interestTypeSelect = value;

                OnPropertyChanged(nameof(InterestTypeSelect));
            }
        }

        /* Tenor */

        string tenor;

        public string Tenor
        {
            get => tenor;

            set
            {
                if (tenor == value)
                    return;

                tenor = value;

                OnPropertyChanged(nameof(Tenor));
            }
        }

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        string response1;

        public string Response1
        {
            get => response1;

            set
            {
                if (response1 == value)
                    return;

                response1 = value;

                OnPropertyChanged(nameof(Response1));
            }
        }

        string response2;

        public string Response2
        {
            get => response2;

            set
            {
                if (response2 == value)
                    return;

                response2 = value;

                OnPropertyChanged(nameof(Response2));
            }
        }

        string response3;

        public string Response3
        {
            get => response3;

            set
            {
                if (response3 == value)
                    return;

                response3 = value;

                OnPropertyChanged(nameof(Response3));
            }
        }

        string response4;

        public string Response4
        {
            get => response4;

            set
            {
                if (response4 == value)
                    return;

                response4 = value;

                OnPropertyChanged(nameof(Response4));
            }
        }

        async private Task ExecuteSubmit()
        {
            SimulasiInput simulasiInput = new SimulasiInput();
            simulasiInput.CalcType = BasedOnSelect == "BasedOnPlafond" ? 1 : 2;
            double doubleAmount;
            simulasiInput.Amount = Double.TryParse(Amount, out doubleAmount) ? doubleAmount : 0.0;
            int intTenor;
            simulasiInput.Tenor = Int32.TryParse(Tenor, out intTenor) ? intTenor : 0;
            double doubleRate;
            simulasiInput.Rate = Double.TryParse(Interest, out doubleRate) ? doubleRate : 0.0;
            simulasiInput.RateType = InterestTypeSelect == "Anuitas" ? 2 : 1;

            string strSimulasiInput = JsonConvert.SerializeObject(simulasiInput);

            StringContent requestBody = new StringContent(strSimulasiInput, Encoding.UTF8, "application/json");

            try
            {
                var result = await _client.PostAsync(apiUrl, requestBody);
                string responseBody = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    SimulasiOutput simulasiOutput = new SimulasiOutput();
                    simulasiOutput = JsonConvert.DeserializeObject<SimulasiOutput>(responseBody);

                    Response1 = string.Format("Plafond = {0:N}", simulasiOutput.Plafond);
                    Response2 = string.Format("Angsuran Pokok = {0:N}", simulasiOutput.AngsuranPokok);
                    Response3 = string.Format("Angsuran Bunga = {0:N}", simulasiOutput.AngsuranBunga);
                    Response4 = string.Format("Total Angsuran = {0:N}", simulasiOutput.TotalAngsuran);
                }
                else
                {
                    Response1 = $"Response status : {result.StatusCode.ToString()}";
                }
            }
            catch (Exception ex)
            {
                Response1 = ex.Message;
            }
        }

        public Command SubmitCommand { get; }


    }
}
