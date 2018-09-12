using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.Toasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace zadanie1_kanas {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SoftPageDetail : ContentPage {
        public SoftPageDetail(int pageId) {
            InitializeComponent();

            //zistenie dostupnosti pripojenia k interentu
            if (CrossConnectivity.Current.IsConnected) {
                loadPage(pageId); //nacitanie stranky do webview
            }
            else {
                showToast("Pripojenie k internetu zlyhalo");
                return;
            }
        }

        public async void loadPage(int id) {
            string pageJsonStr = await GetPageJsonByIdAsync(id);

            JObject clanokJson = JObject.Parse(pageJsonStr);
            string title = clanokJson["title"]["rendered"].ToString();
            string content = clanokJson["content"]["rendered"].ToString();
            //showToast();
            Title = title;
            pageWebView.Source = new HtmlWebViewSource {
                Html = $"" + content
            };
        }

        public async Task<string> GetPageJsonByIdAsync(int pageId) {

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var address = $"http://mechatronika.cool/noviny/wp-json/wp/v2/pages/{pageId}";

            var response = await client.GetAsync(address);

            var pageJson = response.Content.ReadAsStringAsync().Result;

            return pageJson;
        }


        public async Task<string> GetJsonAsync(string url) {

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var address = $"" + url;

            var response = await client.GetAsync(address);

            var clankyJson = response.Content.ReadAsStringAsync().Result;

            return clankyJson;

        }

        private async void showToast(string text) {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(ToastNotificationType.Info, "Upozornenie", text, TimeSpan.FromSeconds(6));
        }
    }
}
