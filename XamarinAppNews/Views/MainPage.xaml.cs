using Plugin.Connectivity;
using Plugin.Toasts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using zadanie1_kanas.ViewModels;
using zadanie1_kanas.Views;

namespace zadanie1_kanas {
    public partial class MainPage : MasterDetailPage {

        public ObservableCollection<MainMenuViewModel> menuItems { get; set; }

        public MainPage() {
            InitializeComponent();

            menuItems = new ObservableCollection<MainMenuViewModel>();

            //nastavenie default detail page
            NavigationPage page = new NavigationPage(new ClankyPage());
            page.BarBackgroundColor = Color.FromHex("#00c853");
            Detail = page;
            
            IsPresented = false;

            //zistenie dostupnosti pripojenia k interentu
            if (CrossConnectivity.Current.IsConnected) {
                //nacitanie menu poloziek
                loadMenuItems();
            }
            else {
                showToast("Pripojenie k internetu zlyhalo");
                return;
            }
            
        }

        public void loadMenuItems() {

            menuItems.Add(new MainMenuViewModel {
                Id = 1,
                Title = "Články",
                IconURL = "https://cdn2.iconfinder.com/data/icons/budicon-document-2/16/40-document_-_paper_article_news-512.png"
            });

            menuItems.Add(new MainMenuViewModel {
                Id = 2,
                Title = "Šablóny",
                IconURL = "http://icons.iconarchive.com/icons/custom-icon-design/mono-general-2/512/document-icon.png"
            });

            menuItems.Add(new MainMenuViewModel {
                Id = 3,
                Title = "Otváracie hodiny",
                IconURL = "http://designmuseum.fi/wp-content/uploads/2014/08/icon-clock-b-01.png"
            });

            menuItems.Add(new MainMenuViewModel {
                Id = 4,
                Title = "Zdroje informácií",
                IconURL = "https://cdn4.iconfinder.com/data/icons/eldorado-mobile/40/info_3-512.png"
            });

            menuItems.Add(new MainMenuViewModel {
                Id = 5,
                Title = "Udalosti",
                IconURL = "https://cdn4.iconfinder.com/data/icons/pictograms-1/512/Calendar-512.png"
            });

            menuItems.Add(new MainMenuViewModel {
                Id = 6,
                Title = "Softvér",
                IconURL = "https://cdn4.iconfinder.com/data/icons/mosaicon-05/512/development-512.png"
            });

            menuItems.Add(new MainMenuViewModel {
                Id = 7,
                Title = "Harmonogram štúdia",
                IconURL = "https://cdn0.iconfinder.com/data/icons/mobile-development-svg-icons/60/Time_management-512.png"
            });

            menuItems.Add(new MainMenuViewModel {
                Id = 8,
                Title = "Rozvrh skúšok",
                IconURL = "http://www.sasc.org.br/images/paginas/calender-4fed96414d8012ca8e726384fa007d2c_40302418044880.png"
            });

            menuItemsList.ItemsSource = menuItems;
            menuItemsList.ItemSelected += MenuItemsList_ItemSelected;
        }

        private void MenuItemsList_ItemSelected(object sender, SelectedItemChangedEventArgs e) {
            if (e.SelectedItem == null)
                return;

            NavigationPage page = new NavigationPage(new ClankyPage());
            // presmerovanie na prislusnu Detail Page
            int id = (e.SelectedItem as MainMenuViewModel).Id;

            page = new NavigationPage();

            switch (id) {
                case 1:
                    page = new NavigationPage(new ClankyPage());
                    //page.PushAsync(new ClankyPage());
                    //page.RemovePage(this);
                    break;
                case 2:
                    page = new NavigationPage(new SablonyPage());
                    break;
                case 3:
                    page = new NavigationPage(new OtvHodPage());
                    break;
                case 4:
                    page = new NavigationPage(new ZdrojePage());
                    break;
                case 5:
                    page = new NavigationPage(new UdalostiPage());
                    break;
                case 6:
                    page = new NavigationPage(new SoftPage());
                    break;
                case 7:
                    page = new NavigationPage(new HarmonogramPage());
                    break;
                case 8:
                    page = new NavigationPage(new RozvrhPage());
                    break;
            }
            
            page.BarBackgroundColor = Color.FromHex("#00c853");
            NavigateToPage(page);
        }

        private void Button_Clicked_Clanky(object sender, EventArgs e) {
            NavigateToPage(new ClankyPage());
        }

        private void Button_Clicked_Sablony(object sender, EventArgs e) {
            NavigateToPage(new SablonyPage());
        }

        //presmerovanie na stranku
        public void NavigateToPage(Page page) {
            Detail = page;
            IsPresented = false;
        }

        private async void showToast(string text) {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(ToastNotificationType.Info, "Upozornenie", text, TimeSpan.FromSeconds(6));
        }
    }
}
