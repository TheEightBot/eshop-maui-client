using CommunityToolkit.Maui.Views;
using eShopOnContainers.ViewModels;

namespace eShopOnContainers.Views
{
    public partial class FiltersView : Popup
    {
        public FiltersView()
        {
            InitializeComponent();

            this.CanBeDismissedByTappingOutsideOfPopup = true;
            this.ResultWhenUserTapsOutsideOfPopup = false;
        }

        void OnFilterClicked(System.Object sender, System.EventArgs e)
        {
            if (BindingContext is CatalogViewModel viewModel)
            {
                viewModel.FilterCommand.Execute(null);
                this.Close(true);
            }
        }
    }
}