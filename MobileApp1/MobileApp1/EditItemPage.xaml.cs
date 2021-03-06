﻿using MobileApp1.Model;
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
    public partial class EditItemPage : ContentPage
    {
        Item itm;
        public EditItemPage(Item item)
        {
            InitializeComponent();
            itm = item;
            eName.Text = item.Name;
            eQuantity.Text = item.Quantity.ToString();
            Stepp.Value = item.Quantity;
        }

        public async void OnClick(object sender, EventArgs e)
        {
            var item = await App.Database.GetItemAsync(itm);
            bool check = string.IsNullOrWhiteSpace(eName.Text);
            if (eName.Text != string.Empty && check == false)
            {
                item.Name = eName.Text;
                item.Quantity =  Stepp.Value;
                await App.Database.UpdateItemAsync(item);
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                await DisplayAlert("Błąd", "Wprowadź nazwę produktu", "OK");
            }
        }

        public  void OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            double newValue = e.NewValue;
            eQuantity.Text = newValue.ToString();
        } 
    }
}