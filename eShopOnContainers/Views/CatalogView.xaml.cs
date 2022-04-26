using eShopOnContainers.ViewModels;
using eShopOnContainers.ViewModels.Base;
using System;

using CommunityToolkit.Maui.Views;
using Microsoft.Maui;

namespace eShopOnContainers.Views
{
    public partial class CatalogView : ContentPageBase
    {
        private FiltersView _filterView = new FiltersView();

        public CatalogView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _filterView.BindingContext = BindingContext;
        }

        private void OnFilterChanged(object sender, EventArgs e)
        {
            this.ShowPopup (_filterView);
        }
    }
}