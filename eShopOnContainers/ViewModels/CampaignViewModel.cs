using eShopOnContainers.Models.Marketing;
using eShopOnContainers.Services.Marketing;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui;

namespace eShopOnContainers.ViewModels
{
    public class CampaignViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ICampaignService _campaignService;

        private ObservableCollection<CampaignItem> _campaigns;

        public CampaignViewModel()
        {
            _settingsService = DependencyService.Get<ISettingsService> ();
            _campaignService = DependencyService.Get<ICampaignService> ();
        }

        public ObservableCollection<CampaignItem> Campaigns
        {
            get => _campaigns;
            set
            {
                _campaigns = value;
                RaisePropertyChanged(() => Campaigns);
            }
        }

        public ICommand GetCampaignDetailsCommand => new Command<CampaignItem>(async (item) => await GetCampaignDetailsAsync(item));

        public override async Task InitializeAsync (IDictionary<string, object> query)
        {
            IsBusy = true;
            // Get campaigns by user
            Campaigns = await _campaignService.GetAllCampaignsAsync (_settingsService.AuthAccessToken);
            IsBusy = false;
        }

        private async Task GetCampaignDetailsAsync(CampaignItem campaign)
        {
            await NavigationService.NavigateToAsync(
                "CampaignDetails",
                new Dictionary<string, string> { { nameof (Campaign.Id), campaign.Id.ToString () } });
        }
    }
}