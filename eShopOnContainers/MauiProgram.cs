using CommunityToolkit.Maui;
using eShopOnContainers.Services;
using eShopOnContainers.Services.AppEnvironment;
using eShopOnContainers.Services.Basket;
using eShopOnContainers.Services.Catalog;
using eShopOnContainers.Services.FixUri;
using eShopOnContainers.Services.Identity;
using eShopOnContainers.Services.Location;
using eShopOnContainers.Services.Marketing;
using eShopOnContainers.Services.OpenUrl;
using eShopOnContainers.Services.Order;
using eShopOnContainers.Services.RequestProvider;
using eShopOnContainers.Services.Settings;
using eShopOnContainers.Services.Theme;
using eShopOnContainers.Services.User;

namespace eShopOnContainers;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
        => MauiApp
            .CreateBuilder()
            .UseMauiApp<App>()
            .ConfigureEffects(
                effects =>
                {
                })
            .UseMauiCommunityToolkit()
            .ConfigureFonts(
                fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    fonts.AddFont("Font_Awesome_5_Free-Regular-400.otf", "FontAwesome-Regular");
                    fonts.AddFont("Font_Awesome_5_Free-Solid-900.otf", "FontAwesome-Solid");
                    fonts.AddFont("Montserrat-Bold.ttf", "Montserrat-Bold");
                    fonts.AddFont("Montserrat-Regular.ttf", "Montserrat-Regular");
                    fonts.AddFont("SourceSansPro-Regular.ttf", "SourceSansPro-Regular");
                    fonts.AddFont("SourceSansPro-Solid.ttf", "SourceSansPro-Solid");
                })
            .RegisterAppServices()
            .RegisterViewModels()
            .RegisterViews()
            .Build();

    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<IServiceCollection>(mauiAppBuilder.Services);
        mauiAppBuilder.Services.AddSingleton<IAppEnvironmentService, AppEnvironmentService>();
        mauiAppBuilder.Services.AddSingleton<ISettingsService, SettingsService>();
        mauiAppBuilder.Services.AddSingleton<INavigationService, MauiNavigationService>();
        mauiAppBuilder.Services.AddSingleton<IDialogService, DialogService>();
        mauiAppBuilder.Services.AddSingleton<IOpenUrlService, OpenUrlService>();
        mauiAppBuilder.Services.AddSingleton<IRequestProvider, RequestProvider>();
        mauiAppBuilder.Services.AddSingleton<IIdentityService, IdentityService>();
        mauiAppBuilder.Services.AddSingleton<IFixUriService, FixUriService>();
        mauiAppBuilder.Services.AddSingleton<ILocationService, LocationService>();
        mauiAppBuilder.Services.AddSingleton<ICatalogService, CatalogMockService>();
        mauiAppBuilder.Services.AddSingleton<IBasketService, BasketMockService>();
        mauiAppBuilder.Services.AddSingleton<IOrderService, OrderMockService>();
        mauiAppBuilder.Services.AddSingleton<IUserService, UserMockService>();
        mauiAppBuilder.Services.AddSingleton<ICampaignService, CampaignMockService>();

        mauiAppBuilder.Services.AddSingleton<ITheme, Theme>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<ViewModels.LoginViewModel>();

        mauiAppBuilder.Services.AddSingleton<ViewModels.BasketViewModel>();
        mauiAppBuilder.Services.AddTransient<ViewModels.CatalogViewModel>();
        mauiAppBuilder.Services.AddTransient<ViewModels.CheckoutViewModel>();
        mauiAppBuilder.Services.AddTransient<ViewModels.LoginViewModel>();
        mauiAppBuilder.Services.AddTransient<ViewModels.MainViewModel>();
        mauiAppBuilder.Services.AddTransient<ViewModels.OrderDetailViewModel>();
        mauiAppBuilder.Services.AddTransient<ViewModels.ProfileViewModel>();
        mauiAppBuilder.Services.AddTransient<ViewModels.SettingsViewModel>();
        mauiAppBuilder.Services.AddTransient<ViewModels.CampaignViewModel>();
        mauiAppBuilder.Services.AddTransient<ViewModels.CampaignDetailsViewModel>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<Views.BasketView>();
        mauiAppBuilder.Services.AddTransient<Views.CampaignDetailsView>();
        mauiAppBuilder.Services.AddTransient<Views.CampaignView>();
        mauiAppBuilder.Services.AddTransient<Views.CatalogView>();
        mauiAppBuilder.Services.AddTransient<Views.CheckoutView>();
        mauiAppBuilder.Services.AddTransient<Views.FiltersView>();
        mauiAppBuilder.Services.AddTransient<Views.LoginView>();
        mauiAppBuilder.Services.AddTransient<Views.OrderDetailView>();
        mauiAppBuilder.Services.AddTransient<Views.ProfileView>();
        mauiAppBuilder.Services.AddTransient<Views.SettingsView>();

        return mauiAppBuilder;
    }
}