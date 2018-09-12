using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace zadanie1_kanas.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SoftPage : ContentPage {
        public SoftPage() {
            InitializeComponent();

            btn1.Clicked += Btn1_Clicked;
            btn2.Clicked += Btn2_Clicked;
            btn3.Clicked += Btn3_Clicked;
        }

        private void Btn3_Clicked(object sender, EventArgs e) {
            (App.Current.MainPage as MainPage).Detail.Navigation.PushAsync(new SoftPageDetail(85));
        }

        private void Btn2_Clicked(object sender, EventArgs e) {
            (App.Current.MainPage as MainPage).Detail.Navigation.PushAsync(new SoftPageDetail(83));
        }

        private void Btn1_Clicked(object sender, EventArgs e) {
            (App.Current.MainPage as MainPage).Detail.Navigation.PushAsync(new SoftPageDetail(79));
            //(App.Current.MainPage as MainPage).Detail.Navigation.RemovePage();
        }


    }
}
