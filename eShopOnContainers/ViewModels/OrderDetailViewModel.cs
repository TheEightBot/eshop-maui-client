using eShopOnContainers.Extensions;
using eShopOnContainers.Models.Orders;
using eShopOnContainers.Services.Order;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.ViewModels.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui;
using eShopOnContainers.Services;

namespace eShopOnContainers.ViewModels
{
    public class OrderDetailViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IOrderService _orderService;

        private Order _order;
        private bool _isSubmittedOrder;
        private string _orderStatusText;

        public OrderDetailViewModel(
            IOrderService orderService,
            IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
            : base(dialogService, navigationService, settingsService)
        {
            _orderService = orderService;
            _settingsService = settingsService;
        }

        public Order Order
        {
            get => _order;
            set
            {
                _order = value;
                RaisePropertyChanged(() => Order);
            }
        }

        public bool IsSubmittedOrder
        {
            get => _isSubmittedOrder;
            set
            {
                _isSubmittedOrder = value;
                RaisePropertyChanged(() => IsSubmittedOrder);
            }
        }

        public string OrderStatusText
        {
            get => _orderStatusText;
            set
            {
                _orderStatusText = value;
                RaisePropertyChanged(() => OrderStatusText);
            }
        }


        public ICommand ToggleCancelOrderCommand => new Command(async () => await ToggleCancelOrderAsync());

        public override async Task InitializeAsync (IDictionary<string, object> query)
        {
            var orderNumber = query.GetValueAsInt (nameof (Order.OrderNumber));

            if (orderNumber.ContainsKeyAndValue)
            {
                IsBusy = true;

                // Get order detail info
                var authToken = _settingsService.AuthAccessToken;
                Order = await _orderService.GetOrderAsync (orderNumber.Value, authToken);
                IsSubmittedOrder = Order.OrderStatus == OrderStatus.Submitted;
                OrderStatusText = Order.OrderStatus.ToString ().ToUpper ();

                IsBusy = false;
            }
        }

        private async Task ToggleCancelOrderAsync()
        {
            var authToken = _settingsService.AuthAccessToken;

            var result = await _orderService.CancelOrderAsync(_order.OrderNumber, authToken);

            if (result)
            {
                OrderStatusText = OrderStatus.Cancelled.ToString().ToUpper();
            }
            else
            {
                Order = await _orderService.GetOrderAsync(Order.OrderNumber, authToken);
                OrderStatusText = Order.OrderStatus.ToString().ToUpper();
            }

            IsSubmittedOrder = false;
        }
    }
}