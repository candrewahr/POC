using System;
using System.ComponentModel;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Xamarin.Forms;
using POC.Models;
using POC.ViewModels;
using POC.Services;
using System.Collections.ObjectModel;
using POC.MobileAppService.Models;
using Xamarin.Forms.Maps;
using System.Collections.Generic;
using System.Linq;

namespace POC.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {           
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel(this);           
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (Item)layout.BindingContext;
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.NearbyBreweries.Count == 0)
                viewModel.IsBusy = true;   
        }

        async void Handle_BreweryTapped(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");
        }
    }
}