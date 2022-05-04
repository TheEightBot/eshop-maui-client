using eShopOnContainers.ViewModels;
using Microsoft.Maui;

namespace eShopOnContainers.Views
{
    public partial class CheckoutView : ContentPage
    {
        public CheckoutView(CheckoutViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}
