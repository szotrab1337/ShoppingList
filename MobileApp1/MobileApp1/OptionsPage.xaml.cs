using MobileApp1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Xml;
using System.Xml.Serialization;

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
        public async void ShareClicked(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("items");
            xmlDoc.AppendChild(rootNode);

            var AllItems = await App.Database.GetItemsAsync();

            foreach (var item in AllItems)
            {
                XmlNode itemNode = xmlDoc.CreateElement("item");
                XmlNode nameNode = xmlDoc.CreateElement("name");
                XmlNode quantityNode = xmlDoc.CreateElement("quantity");

                nameNode.InnerText = item.Name;
                quantityNode.InnerText = item.Quantity.ToString();

                itemNode.AppendChild(nameNode);
                itemNode.AppendChild(quantityNode);

                rootNode.AppendChild(itemNode);
            }

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = xmlDoc.OuterXml,
                Title = "Udostępniona lista zakupów"
            });
        }

        public async void LoadSharedListClicked(object sender, EventArgs e)
        {
            try
            {
                string Name = "";
                string Quantity = "";

                string listXml = await Clipboard.GetTextAsync();

                XmlDocument xmlList = new XmlDocument();
                xmlList.LoadXml(listXml);

                foreach (XmlNode node in xmlList)
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        foreach (XmlNode lastChild in childNode.ChildNodes)
                        {
                            if (lastChild.Name == "name")
                                Name = lastChild.InnerText;

                            if (lastChild.Name == "quantity")
                                Quantity = lastChild.InnerText;
                        }
                        var AllItems = await App.Database.GetItemsAsync();
                        int inx = AllItems.Count;

                        await App.Database.SaveItemsAsync(new Item
                        {
                            Number = inx + 1,
                            Name = Name,
                            IsChecked = false,
                            Quantity = Convert.ToDouble(Quantity)
                        });
                    }
                }
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", "Nie znaleziono poprawnej listy w schowku.\r\n\r\n" + ex.ToString(), "OK");
            }
        }
    }
}