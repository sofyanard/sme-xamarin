using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SMEXamarin.ViewModels;

namespace SMEXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PengajuanDetailPage : ContentPage
    {
        PengajuanDetailViewModel pengajuanDetailViewModel;

        public PengajuanDetailPage()
        {
            InitializeComponent();
        }

        public PengajuanDetailPage(string id)
        {
            InitializeComponent();

            pengajuanDetailViewModel = new PengajuanDetailViewModel(id);
            this.BindingContext = pengajuanDetailViewModel;
        }
    }
}