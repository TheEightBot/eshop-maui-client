using eShopOnContainers.Services.Settings;
using eShopOnContainers.Views.Templates;
using eShopOnContainers.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui;

namespace eShopOnContainers.ViewModels.Base
{
    public abstract class ViewModelBase : ExtendedBindableObject, IQueryAttributable
    {
        public IDialogService DialogService { get; private set; }
        public INavigationService NavigationService { get; private set; }
        public ISettingsService SettingsService { get; private set; }

        private bool _isInitialized;

        public bool IsInitialized
        {
            get => _isInitialized;

            set
            {
                _isInitialized = value;
                OnPropertyChanged(nameof(IsInitialized));
            }
        }

        private bool _multipleInitialization;

        public bool MultipleInitialization
        {
            get => _multipleInitialization;

            set
            {
                _multipleInitialization = value;
                OnPropertyChanged(nameof(MultipleInitialization));
            }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;

            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public ViewModelBase(IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
        {
            DialogService = dialogService;
            NavigationService = navigationService;
            SettingsService = settingsService;

            GlobalSetting.Instance.BaseIdentityEndpoint = SettingsService.IdentityEndpointBase;
            GlobalSetting.Instance.BaseGatewayShoppingEndpoint = SettingsService.GatewayShoppingEndpointBase;
            GlobalSetting.Instance.BaseGatewayMarketingEndpoint = SettingsService.GatewayMarketingEndpointBase;
        }

        public virtual Task InitializeAsync (IDictionary<string, object> query)
        {
            return Task.FromResult (false);
        }

        public async void ApplyQueryAttributes (IDictionary<string, object> query)
        {
            if(!IsInitialized)
            {
                IsInitialized = true;
                await InitializeAsync (query);
            }
        }
    }
}