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
    public partial class HarmonogramPage : ContentPage {

        private ObservableCollection<HarmonogramViewModel> udalosti;

        public HarmonogramPage() {
            InitializeComponent();

            udalosti = new ObservableCollection<HarmonogramViewModel>();

            //zistenie dostupnosti pripojenia k interentu
            if (CrossConnectivity.Current.IsConnected) {
                //nacitanie menu poloziek
                loadEvents();
            }
            else {
                showToast("Pripojenie k internetu zlyhalo");
                return;
            }

        }

        public async void loadEvents() {
            var uri = new Uri(@"http://www.uamt.fei.stuba.sk/MVI/sites/default/files/noviny/harmonogram_studia.csv");
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

                    if (data[0].Equals("Udalosť")) {
                        continue;
                    }

                    if (data[2].Equals("náhrada")) {
                        data[2] = "";
                    }

                    udalosti.Add(new HarmonogramViewModel {
                        udalostName = data[0],
                        udalostDate = data[1],
                        udalostType = data[2]
                    });
                }
            }

            listView.ItemsSource = udalosti;
        }

        private async void showToast(string text) {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(ToastNotificationType.Info, "Upozornenie", text, TimeSpan.FromSeconds(6));
        }
    }
}
