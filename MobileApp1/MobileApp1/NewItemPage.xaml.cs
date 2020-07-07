using MobileApp1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public NewItemPage()
        {
            InitializeComponent();
            eName.Text = string.Empty;
            eQuantity.Text = "1";
            Stepp.Value = 1;
        }
        public async void OnClick(object sender, EventArgs e)
        {
            var AllItems = await App.Database.GetItemsAsync();
            int inx = AllItems.Count;
            bool check = string.IsNullOrWhiteSpace(eName.Text);
            if (eName.Text != string.Empty && check == false)
            {
                await App.Database.SaveItemsAsync(new Item
                {
                    Number = inx + 1,
                    Name = eName.Text,
                    IsChecked = false,
                    Quantity = Stepp.Value
                });
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                eName.Text = string.Empty;
                await DisplayAlert("Błąd", "Wprowadź nazwę produktu", "OK");
            }
        }

        public void OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            double newValue = e.NewValue;
            eQuantity.Text = newValue.ToString();
        }
    }
}