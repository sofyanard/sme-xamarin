using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SMEXamarin.Models;
using SMEXamarin.ViewModels;

namespace SMEXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadDetailPage : ContentPage
    {
        UploadDetailViewModel uploadDetailViewModel;

        public UploadDetailPage()
        {
            InitializeComponent();
        }

        public UploadDetailPage(NasabahUploadResponse selectedFile)
        {
            InitializeComponent();

            uploadDetailViewModel = new UploadDetailViewModel(selectedFile);
            this.BindingContext = uploadDetailViewModel;
        }
    }
}