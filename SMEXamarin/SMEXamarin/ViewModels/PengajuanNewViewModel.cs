using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using SMEXamarin.Views;
using SMEXamarin.Models;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SMEXamarin.ViewModels
{
    public class PengajuanNewViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly HttpClient _client = new HttpClient();

        private string baseApiUrl = App._serverUrl + "/api";

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PengajuanNewViewModel()
        {
            RequestParameters();

            SubmitCommand = new Command(async () =>
            {
                PostPengajuan();
            });
        }

        public Command SubmitCommand { get; }

        List<RfProduct> listRfProduct;

        public List<RfProduct> ListRfProduct
        {
            get => listRfProduct;

            set
            {
                if (listRfProduct == value)
                    return;

                listRfProduct = value;

                OnPropertyChanged(nameof(ListRfProduct));
            }
        }

        RfProduct selectedRfProduct;

        public RfProduct SelectedRfProduct
        {
            get => selectedRfProduct;

            set
            {
                if (selectedRfProduct == value)
                    return;

                selectedRfProduct = value;

                OnPropertyChanged(nameof(SelectedRfProduct));
            }
        }

        double limit;

        public double Limit
        {
            get => limit;

            set
            {
                if (limit == value)
                    return;

                limit = value;

                OnPropertyChanged(nameof(Limit));
            }
        }

        int tenor;

        public int Tenor
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

        List<RfLoanPurpose> listRfLoanPurpose;

        public List<RfLoanPurpose> ListRfLoanPurpose
        {
            get => listRfLoanPurpose;

            set
            {
                if (listRfLoanPurpose == value)
                    return;

                listRfLoanPurpose = value;

                OnPropertyChanged(nameof(ListRfLoanPurpose));
            }
        }

        RfLoanPurpose selectedRfLoanPurpose;

        public RfLoanPurpose SelectedRfLoanPurpose
        {
            get => selectedRfLoanPurpose;

            set
            {
                if (selectedRfLoanPurpose == value)
                    return;

                selectedRfLoanPurpose = value;

                OnPropertyChanged(nameof(SelectedRfLoanPurpose));
            }
        }

        List<ParameterYesNo> listCollateralFlag;

        public List<ParameterYesNo> ListCollateralFlag
        {
            get => listCollateralFlag;

            set
            {
                if (listCollateralFlag == value)
                    return;

                listCollateralFlag = value;

                OnPropertyChanged(nameof(ListCollateralFlag));
            }
        }

        ParameterYesNo selectedCollateralFlag;

        public ParameterYesNo SelectedCollateralFlag
        {
            get => selectedCollateralFlag;

            set
            {
                if (selectedCollateralFlag == value)
                    return;

                selectedCollateralFlag = value;

                OnPropertyChanged(nameof(SelectedCollateralFlag));

                if (SelectedCollateralFlag == ListCollateralFlag[0])
                {
                    ConditioningCollateral(true);
                }
                else
                {
                    ConditioningCollateral(false);
                }
            }
        }

        List<RfCollateralType> listRfCollateralType;

        public List<RfCollateralType> ListRfCollateralType
        {
            get => listRfCollateralType;

            set
            {
                if (listRfCollateralType == value)
                    return;

                listRfCollateralType = value;

                OnPropertyChanged(nameof(ListRfCollateralType));
            }
        }

        RfCollateralType selectedRfCollateralType;

        public RfCollateralType SelectedRfCollateralType
        {
            get => selectedRfCollateralType;

            set
            {
                if (selectedRfCollateralType == value)
                    return;

                selectedRfCollateralType = value;

                OnPropertyChanged(nameof(SelectedRfCollateralType));
            }
        }

        double? collateralValue;

        public double? CollateralValue
        {
            get => collateralValue;

            set
            {
                if (collateralValue == value)
                    return;

                collateralValue = value;

                OnPropertyChanged(nameof(CollateralValue));
            }
        }

        List<RfCertType> listRfCertificateType;

        public List<RfCertType> ListRfCertificateType
        {
            get => listRfCertificateType;

            set
            {
                if (listRfCertificateType == value)
                    return;

                listRfCertificateType = value;

                OnPropertyChanged(nameof(ListRfCertificateType));
            }
        }

        RfCertType selectedRfCertificateType;

        public RfCertType SelectedRfCertificateType
        {
            get => selectedRfCertificateType;

            set
            {
                if (selectedRfCertificateType == value)
                    return;

                selectedRfCertificateType = value;

                OnPropertyChanged(nameof(SelectedRfCertificateType));
            }
        }

        string certificateNo;

        public string CertificateNo
        {
            get => certificateNo;

            set
            {
                if (certificateNo == value)
                    return;

                certificateNo = value;

                OnPropertyChanged(nameof(CertificateNo));
            }
        }

        List<RfBranch> listRfBranch;

        public List<RfBranch> ListRfBranch
        {
            get => listRfBranch;

            set
            {
                if (listRfBranch == value)
                    return;

                listRfBranch = value;

                OnPropertyChanged(nameof(ListRfBranch));
            }
        }

        RfBranch selectedRfBranch;

        public RfBranch SelectedRfBranch
        {
            get => selectedRfBranch;

            set
            {
                if (selectedRfBranch == value)
                    return;

                selectedRfBranch = value;

                OnPropertyChanged(nameof(SelectedRfBranch));
            }
        }

        bool collateralEnabled;

        public bool CollateralEnabled
        {
            get => collateralEnabled;

            set
            {
                if (collateralEnabled == value)
                    return;

                collateralEnabled = value;

                OnPropertyChanged(nameof(CollateralEnabled));
            }
        }

        async private Task RequestParameters()
        {
            // Product
            string rfProductApiUrl = baseApiUrl + "/parameter/product";
            try
            {
                string productResult = await _client.GetStringAsync(rfProductApiUrl);
                ListRfProduct = JsonConvert.DeserializeObject<List<RfProduct>>(productResult);
            }
            catch (Exception ex) { await App.Current.MainPage.DisplayAlert("Request Error", ex.Message, "Close"); }

            // Loan Purpose
            string rfLoanPurposeApiUrl = baseApiUrl + "/parameter/loanpurpose";
            try
            {
                string loanPurposeResult = await _client.GetStringAsync(rfLoanPurposeApiUrl);
                ListRfLoanPurpose = JsonConvert.DeserializeObject<List<RfLoanPurpose>>(loanPurposeResult);
            }
            catch (Exception ex) { await App.Current.MainPage.DisplayAlert("Request Error", ex.Message, "Close"); }

            // Collateral Flag
            List<ParameterYesNo> listYesNo = new List<ParameterYesNo>();
            listYesNo.Add(new ParameterYesNo("1", "Ya"));
            listYesNo.Add(new ParameterYesNo("0", "Tidak"));
            ListCollateralFlag = listYesNo;

            // Collateral Type
            string rfCollateralTypeApiUrl = baseApiUrl + "/parameter/collateraltype";
            try
            {
                string collateralTypeResult = await _client.GetStringAsync(rfCollateralTypeApiUrl);
                ListRfCollateralType = JsonConvert.DeserializeObject<List<RfCollateralType>>(collateralTypeResult);
            }
            catch (Exception ex) { await App.Current.MainPage.DisplayAlert("Request Error", ex.Message, "Close"); }

            // Certificate Type
            string rfCertificateTypeApiUrl = baseApiUrl + "/parameter/certificatetypeall";
            try
            {
                string certificateTypeResult = await _client.GetStringAsync(rfCertificateTypeApiUrl);
                ListRfCertificateType = JsonConvert.DeserializeObject<List<RfCertType>>(certificateTypeResult);
            }
            catch (Exception ex) { await App.Current.MainPage.DisplayAlert("Request Error", ex.Message, "Close"); }

            // Branch
            string rfBranchApiUrl = baseApiUrl + "/parameter/branchall";
            try
            {
                string branchResult = await _client.GetStringAsync(rfBranchApiUrl);
                ListRfBranch = JsonConvert.DeserializeObject<List<RfBranch>>(branchResult);
            }
            catch (Exception ex) { await App.Current.MainPage.DisplayAlert("Request Error", ex.Message, "Close"); }
        }

        private void ConditioningCollateral(bool flag)
        {
            if (flag == true)
            {
                CollateralEnabled = true;
            }
            else
            {
                CollateralEnabled = false;

                if (SelectedRfCollateralType != null)
                    SelectedRfCollateralType = null;

                if (CollateralValue != null)
                    CollateralValue = null;

                if (SelectedRfCertificateType != null)
                    SelectedRfCertificateType = null;

                if (!String.IsNullOrEmpty(CertificateNo))
                    CertificateNo = null;
            }
        }

        private async Task PostPengajuan()
        {
            Pengajuan pengajuan = new Pengajuan();

            pengajuan.Product = SelectedRfProduct.ProductId;
            pengajuan.Limit = Limit;
            pengajuan.Tenor = Tenor;
            pengajuan.Purpose = SelectedRfLoanPurpose.LoanPurpId;
            pengajuan.CollateralFlag = SelectedCollateralFlag.YesNoId;
            if (SelectedCollateralFlag.YesNoId == "1")
            {
                pengajuan.CollateralType = SelectedRfCollateralType.ColTypeSeq;
                pengajuan.CollateralValue = CollateralValue;
                pengajuan.CertificateType = SelectedRfCertificateType.CertTypeId;
                pengajuan.CertificateNo = CertificateNo;
            }
            pengajuan.BranchCode = SelectedRfBranch.BranchCode;

            string postPengajuanApiUrl = baseApiUrl + "/pengajuan";

            try
            {
                var serializeObject = JsonConvert.SerializeObject(pengajuan);
                StringContent stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");

                var authHeader = new AuthenticationHeaderValue("Bearer", App._accessToken);
                _client.DefaultRequestHeaders.Authorization = authHeader;

                var result = await _client.PostAsync(postPengajuanApiUrl, stringContent);
                string resultContent = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert(result.StatusCode.ToString(), "Submit success", "Close");
                    MessagingCenter.Send(this, "PengajuanSubmitted");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(result.StatusCode.ToString(), resultContent, "Close");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Submit Error", ex.Message, "Close");
            }
        }
    }
}
