using eShopOnContainers.ViewModels;
using Microsoft.Maui;

namespace eShopOnContainers.Views
{
    public partial class ProfileView : ContentPageBase
    {
        public ProfileView(ProfileViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}