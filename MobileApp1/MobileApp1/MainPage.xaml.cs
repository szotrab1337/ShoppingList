using MobileApp1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using System.Collections.Concurrent;

namespace MobileApp1
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Item> Items
        {
            get
            {
                return _Items ?? (_Items = new ObservableCollection<Item>());
            }
            set { _Items = value; OnPropertyChanged("Items"); }
        }
        private ObservableCollection<Item> _Items;

        public string Text
        {
            get { return _Text; }
            set { _Text = value; OnPropertyChanged("Text"); }
        }
        private string _Text;

        public bool IsChecked
        {
            get { return _IsChecked; }
            set { _IsChecked = value; OnPropertyChanged("IsChecked"); }
        }
        private bool _IsChecked;
        public Item SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged("SelectedItem"); }
        }
        private Item _SelectedItem;

        public MainPage()
        {
            InitializeComponent();

            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            Text = string.Empty;
            base.OnAppearing();
            Items.Clear();
            int index = 1;
            var AllItems = await App.Database.GetItemsAsync();
            foreach (var item in AllItems)
            {
                item.Number = index;
                index++;
                Items.Add(item);
                //await App.Database.DeleteItemAsync(item);
            }
        }

        public async void OnTapped(object sender, EventArgs e)
        {
            //await DisplayAlert("Test", id.ToString(), "Test");

            var mi = ((Grid)sender);
            //int id = Convert.ToInt32(mi.CommandParameter);
            var gesture = (TapGestureRecognizer)mi.GestureRecognizers[0];
            int id = Convert.ToInt32(gesture.CommandParameter);
            var item = Items.Where(x => x.ID_Item == id).FirstOrDefault();
            Item itemToChange = await App.Database.GetItemAsync(item);

            itemToChange.IsChecked = !itemToChange.IsChecked;
            await App.Database.UpdateItemAsync(itemToChange);
            //Items.Where(x => x.ID_Item == id).FirstOrDefault().IsChecked = !item.IsChecked;

            //item.IsChecked = itemToChange.IsChecked;
            //Items.Up

            Items.Clear();
            int index = 1;
            var AllItems = await App.Database.GetItemsAsync();
            foreach (var itm in AllItems)
            {
                itm.Number = index;
                index++;
                Items.Add(itm);
                //await App.Database.DeleteItemAsync(item);
            }
        }
        //public void OnSwiped(object sender, EventArgs e)
        //{
        //    DisplayAlert("Test", "Zaznaczono", "asdas");
        //}

        public async void OnCheckChanged(object sender, EventArgs e)
        {

            for (int i = 0; i < Items.Count(); i++)
            {
                Item itemToChange = await App.Database.GetItemAsync(Items[i]);

                if (itemToChange.IsChecked == false && Items[i].IsChecked == true)
                {
                    itemToChange.IsChecked = true;
                   
                }

                if (itemToChange.IsChecked == true && Items[i].IsChecked == false)
                {
                    itemToChange.IsChecked = false;
                   
                }

                await App.Database.UpdateItemAsync(itemToChange);
            }

        }

        async void OnButtonClicked(object sender, EventArgs e)
        {
            bool check = string.IsNullOrWhiteSpace(Text);
            if (Text != string.Empty && check == false)
            {


                int index = Items.Count();
                await App.Database.SaveItemsAsync(new Item
                {
                    Number = index + 1,
                    Name = Text,
                    IsChecked = false
                });

                Text = string.Empty;
                var AllItemsx = await App.Database.GetItemsAsync();
                Items.Clear();
                int indexx = 1;
                foreach (var item in AllItemsx)
                {
                    item.Number = indexx;
                    indexx++;
                    Items.Add(item);
                }
            }
            else
            {
                Text = string.Empty;
                await DisplayAlert("Błąd", "Wprowadź nazwę produktu", "OK");
            }

        }

        public async void DeleteClicked(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            int id = Convert.ToInt32(mi.CommandParameter);
            var itm = Items.Where(x => x.ID_Item == id).FirstOrDefault();
            await App.Database.DeleteItemAsync(itm);

            Items.Clear();
            int index = 1;
            var AllItems = await App.Database.GetItemsAsync();
            foreach (var item in AllItems)
            {
                item.Number = index;
                index++;
                Items.Add(item);
                //await App.Database.DeleteItemAsync(item);
            }
        }

        public async void EditItemPage(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            int id = Convert.ToInt32(mi.CommandParameter);
            var item = Items.Where(x => x.ID_Item == id).FirstOrDefault();

            await Navigation.PushAsync(new NavigationPage(new EditItemPage(item)));
        }

    }
}
