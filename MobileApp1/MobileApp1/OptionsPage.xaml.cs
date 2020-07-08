using MobileApp1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OptionsPage : ContentPage
    {
        public OptionsPage()
        {
            InitializeComponent();
        }

        public async void DeleteAllClicked(object sender, EventArgs e)
        {
            var AllItems = await App.Database.GetItemsAsync();

            if (AllItems.Count == 0)
                await DisplayAlert("Błąd", "Brak pozycji do usunięcia.", "OK");


            else
            {
                bool answer = await DisplayAlert("Ostrzeżenie", "Czy na pewno chcesz usunąc wszystkie pozycje?", "Tak", "Nie");

                if (answer == true)
                {

                    foreach (var item in AllItems)
                        await App.Database.DeleteItemAsync(item);

                    Application.Current.MainPage = new NavigationPage(new MainPage());
                }
            }
        }
        
        public async void UncheckAllClicked(object sender, EventArgs e)
        {
            var AllItems = await App.Database.GetItemsAsync();

            List<Item> items = new List<Item>();

            foreach (var item in AllItems)
            {
                if (item.IsChecked == true)
                    items.Add(item);
            }

            if (items.Count == 0)
                await DisplayAlert("Błąd", "Brak pozycji do odznaczenia.", "OK");

            else
            {
                bool answer = await DisplayAlert("Ostrzeżenie", "Czy na pewno chcesz odznaczyc wszystkie pozycje?", "Tak", "Nie");

                if (answer == true)
                {

                    foreach (var item in AllItems)
                    {
                        item.IsChecked = false;
                        await App.Database.UpdateItemAsync(item);
                    }

                    Application.Current.MainPage = new NavigationPage(new MainPage());
                }
            }
        }
        public async void DeleteCheckedClicked(object sender, EventArgs e)
        {
            var AllItems = await App.Database.GetItemsAsync();

            List<Item> items = new List<Item>();

            foreach (var item in AllItems)
            {
                if (item.IsChecked == true)
                    items.Add(item);
            }

            if (items.Count == 0)
                await DisplayAlert("Błąd", "Brak pozycji do usunięcia.", "OK");


            else
            {
                bool answer = await DisplayAlert("Ostrzeżenie", "Czy na pewno chcesz usunąć kupione pozycje?", "Tak", "Nie");

                if (answer == true)
                {

                    foreach (var item in AllItems)
                    {
                        if (item.IsChecked == true)
                            await App.Database.DeleteItemAsync(item);
                    }

                    Application.Current.MainPage = new NavigationPage(new MainPage());
                }
            }
        }
    }
}