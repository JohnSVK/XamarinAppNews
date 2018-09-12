using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using XamForms.Controls;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using zadanie1_kanas.ViewModels;

namespace zadanie1_kanas.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UdalostiPage : ContentPage {
        Calendar calendar;
        CalendarViewModel _vm;

        public UdalostiPage() {
            InitializeComponent();

            _vm = new CalendarViewModel();
            // Handle when your app starts
            //calendar.SpecialDates.Add(new SpecialDate(DateTime.Now.AddDays(5)) { BackgroundColor = Color.Fuchsia, TextColor = Color.Accent, BorderColor = Color.Maroon, BorderWidth = 8 });
            //calendar.SpecialDates.Add(new SpecialDate(DateTime.Now.AddDays(6)) { BackgroundColor = Color.Fuchsia, TextColor = Color.Accent, BorderColor = Color.Maroon, BorderWidth = 8 });
            //calendar.RaiseSpecialDatesChanged();

            var dates = new List<SpecialDate>();

            var specialDate = new SpecialDate(new DateTime(2017, 04, 28));
            specialDate.BackgroundColor = Color.Green;
            specialDate.TextColor = Color.White;

            dates.Add(specialDate);

            //_vm.Attendances = new ObservableCollection<SpecialDate>(dates);


            calendar = new Calendar {
                MaxDate = DateTime.Now.AddDays(60),
                MinDate = DateTime.Now.AddDays(-1),
                MultiSelectDates = false,
                DisableAllDates = false,
                WeekdaysShow = true,
                ShowNumberOfWeek = false,
                ShowNumOfMonths = 1,
                EnableTitleMonthYearView = true,
                SelectedDate = DateTime.Now,
                WeekdaysTextColor = Color.Teal,
                StartDay = DayOfWeek.Monday,
                SelectedTextColor = Color.Fuchsia,

                SpecialDates = new List<SpecialDate>{
                    //new SpecialDate(DateTime.Now.AddDays(2)) { BackgroundColor = Color.Green, TextColor = Color.Accent, BorderColor = Color.Lime, BorderWidth=8, Selectable = true },
                    /*
                    new SpecialDate(DateTime.Now.AddDays(3))
                    {
                        BackgroundColor = Color.Green,
                        TextColor = Color.Blue,
                        Selectable = true,
                        BackgroundPattern = new BackgroundPattern(1) {
                            Pattern = new List<Pattern>
                            {
                                new Pattern{ WidthPercent = 1f, HightPercent = 0.25f, Color = Color.Red},
                                new Pattern{ WidthPercent = 1f, HightPercent = 0.25f, Color = Color.Purple},
                                new Pattern{ WidthPercent = 1f, HightPercent = 0.25f, Color = Color.Green},
                                new Pattern{ WidthPercent = 1f, HightPercent = 0.25f, Color = Color.Yellow}
                            }

                        }
                    },
                    */
                    new SpecialDate(DateTime.Now.AddDays(4))
                    {
                        Selectable = true,
                        BackgroundColor = Color.Green
                        //BackgroundImage = FileImageSource.FromFile("icon.png") as FileImageSource
                    }
                }
            };

            calendar.DateClicked += (sender, e) => {
                System.Diagnostics.Debug.WriteLine(calendar.SelectedDates);
            };
            
            //calendar.SetBinding(Calendar.DateCommandProperty, nameof(_vm.DateChosen));
            //calendar.SetBinding(Calendar.SpecialDatesProperty, nameof(_vm.Attendances));

            // The root page of your application
            BackgroundColor = Color.White;
            Content = new ScrollView {
                Content = new StackLayout {
                    Padding = new Thickness(5, Device.RuntimePlatform == Device.iOS ? 25 : 5, 5, 5),
                    Children = {
                            calendar//,c2
						}
                }
            };
        }

    }
}
