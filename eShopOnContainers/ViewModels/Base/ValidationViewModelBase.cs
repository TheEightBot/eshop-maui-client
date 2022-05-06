using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using eShopOnContainers.Services;
using eShopOnContainers.Services.Settings;

namespace eShopOnContainers.ViewModels.Base
{
    public abstract class ValidationViewModelBase : ObservableValidator, IViewModelBase
    {
        private bool _isInitialized;
        private bool _multipleInitialization;
        private bool _isBusy;

        public IDialogService DialogService { get; private set; }

        public INavigationService NavigationService { get; private set; }

        public ISettingsService SettingsService { get; private set; }

        public bool IsInitialized
        {
            get => _isInitialized;
            set => SetProperty(ref _isInitialized, value);
        }

        public bool MultipleInitialization
        {
            get => _multipleInitialization;
            set => SetProperty(ref _multipleInitialization, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ValidationViewModelBase(IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
        {
            DialogService = dialogService;
            NavigationService = navigationService;
            SettingsService = settingsService;

            GlobalSetting.Instance.BaseIdentityEndpoint = SettingsService.IdentityEndpointBase;
            GlobalSetting.Instance.BaseGatewayShoppingEndpoint = SettingsService.GatewayShoppingEndpointBase;
            GlobalSetting.Instance.BaseGatewayMarketingEndpoint = SettingsService.GatewayMarketingEndpointBase;
        }

        public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
        {
        }

        public virtual Task InitializeAsync(IDictionary<string, object> query)
        {
            return Task.CompletedTask;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            var result = this.GetErrors(e.PropertyName).OfType<ValidationResult>().FirstOrDefault();
        }
    }
}

