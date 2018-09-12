using Android.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.Toasts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using zadanie1_kanas.Views;

namespace zadanie1_kanas {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClankyPage : ContentPage {

        public ObservableCollection<ClankyViewModel> clanky { get; set; }
        public ObservableCollection<ClankyViewModel> clankyFiltered { get; set; }
        private Dictionary<int, string> categories;

        public ClankyPage() {
            InitializeComponent();

            clanky = new ObservableCollection<ClankyViewModel>();
            clankyFiltered = new ObservableCollection<ClankyViewModel>();
            categories = new Dictionary<int, string>();

            listView.HasUnevenRows = true;

            //zistenie dostupnosti pripojenia k interentu
            if (CrossConnectivity.Current.IsConnected) {
                //nacitanie vsetkych clankov do listview
                loadAllClanky();
            } else {
                showToast("Pripojenie k internetu zlyhalo");
                return;
            }

            //listView.ItemsSource = clanky;

            
        }

        public async void loadAllClanky() {
            string clankyJsonStr = await GetJsonAsync("http://mechatronika.cool/noviny/wp-json/wp/v2/posts");
            string categoriesJsonStr = await GetJsonAsync("http://mechatronika.cool/noviny/wp-json/wp/v2/categories");
            

            JArray categoriesJson = JArray.Parse(categoriesJsonStr);
            foreach(var categoryJson in categoriesJson) {
                categories.Add((int)categoryJson["id"], categoryJson["name"].ToString());
            }

            createFilters(); //vytvorenie filtrov na kategorie

            JArray clankyArray = JArray.Parse(clankyJsonStr);
            foreach (var clanokJson in clankyArray) {
                //nacitanie vsetkych kategorii clanku
                List<int> categoriesList = new List<int>();
                foreach (int categoryId in clanokJson["categories"]) {
                    categoriesList.Add(categoryId);
                }

                string categoriesNames = "";
                foreach(int categoryId in categoriesList) {
                    string categoryName;
                    categories.TryGetValue(categoryId, out categoryName);
                    categoriesNames += categoryName + ", ";
                }

                // pridanie clanku do zoznamu
                clanky.Add(new ClankyViewModel {
                    Id = (int)clanokJson["id"],
                    Image = "https://cdn2.iconfinder.com/data/icons/budicon-document-2/16/40-document_-_paper_article_news-512.png",
                    KategorieList = categoriesList,
                    Kategoria = "Kategórie: "+categoriesNames,
                    Nadpis = clanokJson["title"]["rendered"].ToString(),
                    Obsah = escapeHtml(clanokJson["content"]["rendered"].ToString().Substring(0, 50))
                });

            }

            listView.ItemsSource = clanky;
            listView.ItemSelected += ListView_ItemSelected;
        }

        //metoda vytvori filtre v toolbare
        public bool createFilters() {
            ToolbarItems.Add(new ToolbarItem("Všetky kategórie", "filter.png", () => {
                listView.ItemsSource = clanky;
            }, ToolbarItemOrder.Secondary));

            //filters
            foreach (KeyValuePair<int, string> category in categories) {
                ToolbarItems.Add(new ToolbarItem(category.Value, "filter.png", () => {
                    int i;

                    if (clanky.Count == 0)
                        return;
                    clankyFiltered = new ObservableCollection<ClankyViewModel>();

                    for (i = 0; i < clanky.Count; i++) {
                        if (clanky[i].KategorieList.Contains(category.Key)) {
                            clankyFiltered.Add(clanky[i]); //filtrovanie prvkov z listu
                        }
                    }

                    listView.ItemsSource = clankyFiltered;
                }, ToolbarItemOrder.Secondary));
            }

            
            return true;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e) {
            if(e.SelectedItem == null)
                return;
            // presmerovanie na prislusnu Detail Page
            int clanokId =  (e.SelectedItem as ClankyViewModel).Id;
            ClanokDetailPage page = new ClanokDetailPage(clanokId);
            //page.BarBackgroundColor = Color.FromHex("#00c853");
            (App.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(page);

            ((ListView)sender).SelectedItem = null;
        }

        public async Task<string> GetJsonAsync(string url) {

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var address = $""+url;

            var response = await client.GetAsync(address);

            var clankyJson = response.Content.ReadAsStringAsync().Result;

            return clankyJson;

        }

        public static string escapeHtml(string value) {
            var step1 = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
            var step2 = Regex.Replace(step1, @"\s{2,}", " ");
            return step2;
        }

        private async void showToast(string text) {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(ToastNotificationType.Info, "Upozornenie", text, TimeSpan.FromSeconds(6));
        }
    }
}
