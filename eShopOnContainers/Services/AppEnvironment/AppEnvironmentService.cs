using System;
using eShopOnContainers.Services.Basket;
using eShopOnContainers.Services.Catalog;
using eShopOnContainers.Services.Marketing;
using eShopOnContainers.Services.Order;
using eShopOnContainers.Services.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace eShopOnContainers.Services.AppEnvironment
{
    public class AppEnvironmentService : IAppEnvironmentService
    {
        private readonly IServiceCollection _serviceCollection;

        public AppEnvironmentService(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public bool UseMockService { get; private set; }

        public void UpdateDependencies(bool useMockServices)
        {
            UseMockService = useMockServices;

            _serviceCollection.RemoveAll<IBasketService>();
            _serviceCollection.RemoveAll<ICampaignService>();
            _serviceCollection.RemoveAll<ICatalogService>();
            _serviceCollection.RemoveAll<IOrderService>();
            _serviceCollection.RemoveAll<IUserService>();

            if (useMockServices)
            {
                _serviceCollection.AddSingleton<IBasketService, BasketMockService>();
                _serviceCollection.AddSingleton<ICampaignService, CampaignMockService>();
                _serviceCollection.AddSingleton<ICatalogService, CatalogMockService>();
                _serviceCollection.AddSingleton<IOrderService, OrderMockService>();
                _serviceCollection.AddSingleton<IUserService, UserMockService>();

                UseMockService = true;
            }
            else
            {
                _serviceCollection.AddSingleton<IBasketService, BasketService>();
                _serviceCollection.AddSingleton<ICampaignService, CampaignService>();
                _serviceCollection.AddSingleton<ICatalogService, CatalogService>();
                _serviceCollection.AddSingleton<IOrderService, OrderService>();
                _serviceCollection.AddSingleton<IUserService, UserService>();

                UseMockService = false;
            }
        }
    }
}

