using System;
using System.Collections.Generic;
using Xamarin.Forms;
using POC.ViewModels;

namespace POC.Views
{
    public partial class MapSettingsView : Rg.Plugins.Popup.Pages.PopupPage
    {
        MapSettingsViewModel viewModel;
        public MapSettingsView()
        {
            BindingContext = viewModel = new MapSettingsViewModel();
            InitializeComponent();
        }

        //onclickedevents for changing map settings... 
    }
}
