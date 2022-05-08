using eShopOnContainers.Models.Basket;
using eShopOnContainers.Models.Catalog;
using eShopOnContainers.Services.Basket;
using eShopOnContainers.Services.Catalog;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.Services.User;
using eShopOnContainers.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui;
using eShopOnContainers.Services;
using eShopOnContainers.Services.AppEnvironment;
using eShopOnContainers.Extensions;

namespace eShopOnContainers.ViewModels
{
    public class CatalogViewModel : ViewModelBase
    {
        private ObservableCollection<CatalogItem> _products;
        private ObservableCollection<CatalogBrand> _brands;
        private ObservableCollection<CatalogType> _types;

        private CatalogItem _selectedProduct;
        private CatalogBrand _brand;
        private CatalogType _type;
        private int _badgeCount;
        private IAppEnvironmentService _appEnvironmentService;
        private ISettingsService _settingsService;

        public ObservableCollection<CatalogItem> Products
        {
            get => _products;
            private set
            {
                _products = value;
                RaisePropertyChanged(() => Products);
            }
        }

        public CatalogItem SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (value == null)
                    return;
                _selectedProduct = null;
                RaisePropertyChanged(() => SelectedProduct);
            }
        }

        public ObservableCollection<CatalogBrand> Brands
        {
            get => _brands;
            private set
            {
                _brands = value;
                RaisePropertyChanged(() => Brands);
            }
        }

        public CatalogBrand Brand
        {
            get => _brand;
            set
            {
                _brand = value;
                RaisePropertyChanged(() => Brand);
                RaisePropertyChanged(() => IsFilter);
            }
        }

        public ObservableCollection<CatalogType> Types
        {
            get => _types;
            private set
            {
                _types = value;
                RaisePropertyChanged(() => Types);
            }
        }

        public CatalogType Type
        {
            get => _type;
            set
            {
                _type = value;
                RaisePropertyChanged(() => Type);
                RaisePropertyChanged(() => IsFilter);
            }
        }


        public int BadgeCount
        {
            get => _badgeCount;
            set
            {
                _badgeCount = value;
                RaisePropertyChanged(() => BadgeCount);
            }
        }

        public bool IsFilter { get { return Brand != null || Type != null; } }

        public CatalogViewModel(
            IAppEnvironmentService appEnvironmentService,
            IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
            : base(dialogService, navigationService, settingsService)
        {
            this.MultipleInitialization = true;

            _appEnvironmentService = appEnvironmentService;
            _settingsService = settingsService;

            Products = new ObservableCollection<CatalogItem>();
            Brands = new ObservableCollection<CatalogBrand>();
            Types = new ObservableCollection<CatalogType>();
        }

        

        public ICommand AddCatalogItemCommand => new Command<CatalogItem>(AddCatalogItem);

        public ICommand FilterCommand => new Command(async () => await FilterAsync());

		public ICommand ClearFilterCommand => new Command(async () => await ClearFilterAsync());

        public ICommand ViewBasketCommand => new Command (async () => await ViewBasket ());

        public override async Task InitializeAsync (IDictionary<string, object> query)
        {
            IsBusy = true;

            // Get Catalog, Brands and Types
            var products = await _appEnvironmentService.CatalogService.GetCatalogAsync ();
            var brands = await _appEnvironmentService.CatalogService.GetCatalogBrandAsync ();
            var types = await _appEnvironmentService.CatalogService.GetCatalogTypeAsync ();

            Products.ReloadData(products);
            Brands.ReloadData(brands);
            Types.ReloadData(types);

            var authToken = _settingsService.AuthAccessToken;
            var userInfo = await _appEnvironmentService.UserService.GetUserInfoAsync (authToken);

            var basket = await _appEnvironmentService.BasketService.GetBasketAsync (userInfo.UserId, authToken);

            BadgeCount = basket?.Items?.Count () ?? 0;

            IsBusy = false;
        }

        private async void AddCatalogItem(CatalogItem catalogItem)
        {
            if(catalogItem is null)
            {
                return;
            }

            var authToken = _settingsService.AuthAccessToken;
            var userInfo = await _appEnvironmentService.UserService.GetUserInfoAsync (authToken);
            var basket = await _appEnvironmentService.BasketService.GetBasketAsync (userInfo.UserId, authToken);
            if(basket != null)
            {
                basket.Items.Add (
                    new BasketItem
                    {
                        ProductId = catalogItem.Id,
                        ProductName = catalogItem.Name,
                        PictureUrl = catalogItem.PictureUri,
                        UnitPrice = catalogItem.Price,
                        Quantity = 1
                    });

                await _appEnvironmentService.BasketService.UpdateBasketAsync (basket, authToken);
                BadgeCount = basket.Items.Count ();
            }

            SelectedProduct = null;
        }

        private async Task FilterAsync()
        {
            try
            {    
                IsBusy = true;

                if (Brand != null && Type != null)
                {
                    var filteredProducts = await _appEnvironmentService.CatalogService.FilterAsync(Brand.Id, Type.Id);
                    Products.ReloadData(filteredProducts);
                }

                await NavigationService.PopAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ClearFilterAsync()
        {
            try
            {
                IsBusy = true;

                Brand = null;
                Type = null;
                var allProducts = await _appEnvironmentService.CatalogService.GetCatalogAsync();
                Products.ReloadData(allProducts);
                 
                await NavigationService.PopAsync(); 
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task ViewBasket()
        {
            return NavigationService.NavigateToAsync ("Basket");
        }
    }
}