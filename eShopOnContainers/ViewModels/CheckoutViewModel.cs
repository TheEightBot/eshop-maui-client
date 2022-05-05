using eShopOnContainers.Models.Basket;
using eShopOnContainers.Models.Navigation;
using eShopOnContainers.Models.Orders;
using eShopOnContainers.Models.User;
using eShopOnContainers.Services.Basket;
using eShopOnContainers.Services.Order;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.Services.User;
using eShopOnContainers.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui;
using eShopOnContainers.Services;

namespace eShopOnContainers.ViewModels
{
    public class CheckoutViewModel : ViewModelBase
    {
        private ObservableCollection<BasketItem> _orderItems;
        private Order _order;
        private Address _shippingAddress;

        private readonly ISettingsService _settingsService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        private readonly BasketViewModel _basketViewModel;

        public CheckoutViewModel(
            IBasketService basketService, IOrderService orderService, IUserService userService,
            IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService,
            BasketViewModel basketViewModel)
            : base(dialogService, navigationService, settingsService)
        {
            _basketService = basketService;
            _orderService = orderService;
            _userService = userService;
            _settingsService = settingsService;

            _basketViewModel = basketViewModel;
        }

        public ObservableCollection<BasketItem> OrderItems
        {
            get => _orderItems;
            set
            {
                _orderItems = value;
                RaisePropertyChanged(() => OrderItems);
            }
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

        public Address ShippingAddress
        {
            get => _shippingAddress;
            set
            {
                _shippingAddress = value;
                RaisePropertyChanged(() => ShippingAddress);
            }
        }

        public ICommand CheckoutCommand => new Command(async () => await CheckoutAsync());

        public override async Task InitializeAsync (IDictionary<string, object> query)
        {
            IsBusy = true;

            var basketItems = _basketService.LocalBasketItems;
            OrderItems = new ObservableCollection<BasketItem>(basketItems);

            var authToken = _settingsService.AuthAccessToken;
            var userInfo = await _userService.GetUserInfoAsync (authToken);

            // Create Shipping Address
            ShippingAddress = new Address
            {
                Id = !string.IsNullOrEmpty (userInfo?.UserId) ? new Guid (userInfo.UserId) : Guid.NewGuid (),
                Street = userInfo?.Street,
                ZipCode = userInfo?.ZipCode,
                State = userInfo?.State,
                Country = userInfo?.Country,
                City = userInfo?.Address
            };

            // Create Payment Info
            var paymentInfo = new PaymentInfo
            {
                CardNumber = userInfo?.CardNumber,
                CardHolderName = userInfo?.CardHolder,
                CardType = new CardType { Id = 3, Name = "MasterCard" },
                SecurityNumber = userInfo?.CardSecurityNumber
            };

            var orderItems = CreateOrderItems (basketItems);

            // Create new Order
            Order = new Order
            {
                BuyerId = userInfo.UserId,
                OrderItems = orderItems,
                OrderStatus = OrderStatus.Submitted,
                OrderDate = DateTime.Now,
                CardHolderName = paymentInfo.CardHolderName,
                CardNumber = paymentInfo.CardNumber,
                CardSecurityNumber = paymentInfo.SecurityNumber,
                CardExpiration = DateTime.Now.AddYears (5),
                CardTypeId = paymentInfo.CardType.Id,
                ShippingState = _shippingAddress.State,
                ShippingCountry = _shippingAddress.Country,
                ShippingStreet = _shippingAddress.Street,
                ShippingCity = _shippingAddress.City,
                ShippingZipCode = _shippingAddress.ZipCode,
                Total = CalculateTotal (orderItems),
            };

            if (_settingsService.UseMocks)
            {
                // Get number of orders
                var orders = await _orderService.GetOrdersAsync (authToken);

                // Create the OrderNumber
                Order.OrderNumber = orders.Count + 1;
                RaisePropertyChanged (() => Order);
            }

            IsBusy = false;
        }

        private async Task CheckoutAsync()
        {
            try
            {
                var authToken = _settingsService.AuthAccessToken;

                var basket = _orderService.MapOrderToBasket(Order);
                basket.RequestId = Guid.NewGuid();

                // Create basket checkout
                await _basketService.CheckoutAsync(basket, authToken);

                if (_settingsService.UseMocks)
                {
                    await _orderService.CreateOrderAsync(Order, authToken);
                }

                // Clean Basket
                await _basketService.ClearBasketAsync(_shippingAddress.Id.ToString(), authToken);

                // Reset Basket badge
                _basketViewModel.BadgeCount = 0;

                // Navigate to Orders
                await NavigationService.NavigateToAsync("//Main/Catalog");

                // Show Dialog
                await DialogService.ShowAlertAsync("Order sent successfully!", "Checkout", "Ok");
            }
            catch
            {
                await DialogService.ShowAlertAsync("An error ocurred. Please, try again.", "Oops!", "Ok");
            }
        }

        private List<OrderItem> CreateOrderItems(IEnumerable<BasketItem> basketItems)
        {
            var orderItems = new List<OrderItem>();

            foreach (var basketItem in basketItems)
            {
                if (!string.IsNullOrEmpty(basketItem.ProductName))
                {
                    orderItems.Add(new OrderItem
                    {
                        OrderId = null,
                        ProductId = basketItem.ProductId,
                        ProductName = basketItem.ProductName,
                        PictureUrl = basketItem.PictureUrl,
                        Quantity = basketItem.Quantity,
                        UnitPrice = basketItem.UnitPrice
                    });
                }
            }

            return orderItems;
        }

        private decimal CalculateTotal(List<OrderItem> orderItems)
        {
            decimal total = 0;

            foreach (var orderItem in orderItems)
            {
                total += (orderItem.Quantity * orderItem.UnitPrice);
            }

            return total;
        }
    }
}