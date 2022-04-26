using System;
using eShopOnContainers.ViewModels.Base;
using Microsoft.Maui;

namespace eShopOnContainers.Views
{
    public abstract class ContentPageBase : ContentPage
    {
        public ContentPageBase()
        {
            NavigationPage.SetBackButtonTitle (this, string.Empty);
        }

        protected override async void OnAppearing ()
        {
            base.OnAppearing ();

            if (BindingContext is ViewModelBase vmb)
            {
                if (!vmb.IsInitialized || vmb.MultipleInitialization)
                {
                    await vmb.InitializeAsync (null);
                }
            }
        }
    }
}
