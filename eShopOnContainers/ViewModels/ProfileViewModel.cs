using eShopOnContainers.Extensions;
using eShopOnContainers.Models.Orders;
using eShopOnContainers.Models.User;
using eShopOnContainers.Services.Order;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui;
using eShopOnContainers.Services;
using eShopOnContainers.Services.AppEnvironment;

namespace eShopOnContainers.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IAppEnvironmentService _appEnvironmentService;
        private ObservableCollection<Order> _orders;
        private Order _selectedOrder;

        public ProfileViewModel(
            IAppEnvironmentService appEnvironmentService,
            IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
            : base(dialogService, navigationService, settingsService)
        {
            this.MultipleInitialization = true;

            _appEnvironmentService = appEnvironmentService;
            _settingsService = settingsService;
        }

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                RaisePropertyChanged(() => Orders);
            }
        }

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (value == null)
                    return;
                _selectedOrder = null;
                RaisePropertyChanged(() => SelectedOrder);
            }
        }

        public ICommand LogoutCommand => new Command(async () => await LogoutAsync());

        public ICommand OrderDetailCommand => new Command<Order>(async (order) => await OrderDetailAsync(order));

        public override async Task InitializeAsync (IDictionary<string, object> query)
        {
            IsBusy = true;

            // Get orders
            var authToken = _settingsService.AuthAccessToken;
            var orders = await _appEnvironmentService.OrderService.GetOrdersAsync (authToken);
            Orders = orders.ToObservableCollection ();

            IsBusy = false;
        }

        private async Task LogoutAsync()
        {
            IsBusy = true;

            // Logout
            await NavigationService.NavigateToAsync(
                "//Login",
                new Dictionary<string, object> { { "Logout", true } });

            IsBusy = false;
        }

        private async Task OrderDetailAsync(Order order)
        {
            if(order is null)
            {
                return;
            }

            await NavigationService.NavigateToAsync(
                "OrderDetail",
                new Dictionary<string, object>{ { nameof (Order.OrderNumber), order.OrderNumber } });
        }
    }
}