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
    public class CatalogViewModel : ObservableViewModelBase
    {
        private ObservableCollectionEx<CatalogItem> _products;
        private ObservableCollectionEx<CatalogBrand> _brands;
        private ObservableCollectionEx<CatalogType> _types;

        private CatalogItem _selectedProduct;
        private CatalogBrand _brand;
        private CatalogType _type;
        private int _badgeCount;
        private IAppEnvironmentService _appEnvironmentService;
        private ISettingsService _settingsService;

        public ObservableCollectionEx<CatalogItem> Products
        {
            get => _products;
            private set => SetProperty(ref _products, value);
        }

        public CatalogItem SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        public ObservableCollectionEx<CatalogBrand> Brands
        {
            get => _brands;
            set => SetProperty(ref _brands, value);
        }

        public CatalogBrand Brand
        {
            get => _brand;
            set
            {
                SetProperty(ref _brand, value);
                this.OnPropertyChanged(nameof(IsFilter));
            }
        }

        public ObservableCollectionEx<CatalogType> Types
        {
            get => _types;
            set => SetProperty(ref _types, value);
        }

        public CatalogType Type
        {
            get => _type;
            set
            {
                SetProperty(ref _type, value);
                this.OnPropertyChanged(nameof(IsFilter));
            }
        }


        public int BadgeCount
        {
            get => _badgeCount;
            set => SetProperty(ref _badgeCount, value);
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

            Products = new ObservableCollectionEx<CatalogItem>();
            Brands = new ObservableCollectionEx<CatalogBrand>();
            Types = new ObservableCollectionEx<CatalogType>();
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

            var authToken = _settingsService.AuthAccessToken;
            var userInfo = await _appEnvironmentService.UserService.GetUserInfoAsync (authToken);

            var basket = await _appEnvironmentService.BasketService.GetBasketAsync (userInfo.UserId, authToken);

            BadgeCount = basket?.Items?.Count () ?? 0;

            _products.ReloadData(products);
            _brands.ReloadData(brands);
            _types.ReloadData(types);

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
                    _products.ReloadData(filteredProducts);
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
                _products.ReloadData(allProducts);
                 
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