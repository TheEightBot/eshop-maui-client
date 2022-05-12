using System;
using eShopOnContainers.Services;
using eShopOnContainers.Services.Settings;

namespace eShopOnContainers.ViewModels.Base
{
    public interface IViewModelBase : IQueryAttributable
    {
        public IDialogService DialogService { get; }
        public INavigationService NavigationService { get; }
        public ISettingsService SettingsService { get; }

        public bool IsInitialized { get; set; }

        public bool MultipleInitialization { get; set; }

        public bool IsBusy { get; set; }

        Task InitializeAsync();
    }
}
