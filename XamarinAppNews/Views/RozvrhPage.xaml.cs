using Android.Util;
using CsvHelper;
using Java.IO;
using Plugin.Connectivity;
using Plugin.Toasts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using zadanie1_kanas.ViewModels;

namespace zadanie1_kanas.Views {

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RozvrhPage : ContentPage {

        private ObservableCollection<RozvrhSkusokViewModel> skusky;

        public RozvrhPage() {
            InitializeComponent();

            skusky = new ObservableCollection<RozvrhSkusokViewModel>();

            //zistenie dostupnosti pripojenia k interentu
            if (CrossConnectivity.Current.IsConnected) {
                //nacitanie menu poloziek
                loadSkusky();
            }
            else {
                showToast("Pripojenie k internetu zlyhalo");
                return;
            }

            listView.HasUnevenRows = true;

        }

        public async void loadSkusky() {
            var uri = new Uri(@"http://www.uamt.fei.stuba.sk/MVI/sites/default/files/noviny/Terminy_skusok_FEI_ZS_2016_17f.csv");
            var request = (HttpWebRequest)WebRequest.Create(uri);
            WebResponse responseObject = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request);
            var responseStream = responseObject.GetResponseStream();

            string sringToRead;
            using (StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("windows-1250"))) {
                sringToRead = streamReader.ReadToEnd();
                sringToRead = sringToRead.Replace(", ", " - ");
            }

            using (TextReader textReader = new System.IO.StringReader(sringToRead)) {
                var parser = new CsvParser(textReader);
                parser.Configuration.IgnoreQuotes = true;

                string[] row;
                while ((row = parser.Read()) != null) {
                    string[] data = row[0].Split(';');

                    if (data[0].Equals("PRAC")) {
                        continue;
                    }

                    skusky.Add(new RozvrhSkusokViewModel {
                        pracovisko = data[0],
                        kod = data[1],
                        predmet = data[2],
                        RTdate = "DÁTUM: "+ data[3],
                        RTcas = "BEH: " + data[4],
                        RTmiesto = "MIESTNOSŤ: " + getStrIfAble(data[getIndexIfAble(data,5)]),
                        OTdate = "DÁTUM: " + data[getIndexIfAble(data, 6)],
                        OTcas = "BEH: " + data[getIndexIfAble(data, 7)],
                        OTmiesto = "MIESTNOSŤ: " + getStrIfAble(data[getIndexIfAble(data, 5)]),
                    });
                }
            }

            listView.ItemsSource = skusky;
        }

        public int getIndexIfAble(string[] arr, int index) {
            if(index >= arr.Length) {
                return 0;
            }

            return index;
        }

        private string getStrIfAble(string str) {
            if(str == null) {
                return "";
            }

            return str;
        }

        private async void showToast(string text) {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(ToastNotificationType.Info, "Upozornenie", text, TimeSpan.FromSeconds(6));
        }
    }
}
