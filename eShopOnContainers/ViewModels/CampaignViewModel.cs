using eShopOnContainers.Models.Marketing;
using eShopOnContainers.Services.Marketing;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui;
using eShopOnContainers.Services;
using eShopOnContainers.Services.AppEnvironment;
using eShopOnContainers.Extensions;

namespace eShopOnContainers.ViewModels
{
    public class CampaignViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IAppEnvironmentService _appEnvironmentService;

        private ObservableCollectionEx<CampaignItem> _campaigns;

        public ObservableCollectionEx<CampaignItem> Campaigns
        {
            get => _campaigns;
            private set
            {
                _campaigns = value;
                RaisePropertyChanged(() => Campaigns);
            }
        }

        public CampaignViewModel(
            IAppEnvironmentService appEnvironmentService,
            IDialogService dialogService, INavigationService navigationService, ISettingsService settingsService)
            : base(dialogService, navigationService, settingsService)
        {
            _appEnvironmentService = appEnvironmentService;
            _settingsService = settingsService;

            Campaigns = new ObservableCollectionEx<CampaignItem>();
        }


        public ICommand GetCampaignDetailsCommand => new Command<CampaignItem>(async (item) => await GetCampaignDetailsAsync(item));

        public override async Task InitializeAsync (IDictionary<string, object> query)
        {
            IsBusy = true;
            // Get campaigns by user
            var campaigns = await _appEnvironmentService.CampaignService.GetAllCampaignsAsync (_settingsService.AuthAccessToken);
            _campaigns.ReloadData(campaigns);
            IsBusy = false;
        }

        private async Task GetCampaignDetailsAsync(CampaignItem campaign)
        {
            if(campaign is null)
            {
                return;
            }

            await NavigationService.NavigateToAsync(
                "CampaignDetails",
                new Dictionary<string, object> { { nameof (Campaign.Id), campaign.Id } });
        }
    }
}