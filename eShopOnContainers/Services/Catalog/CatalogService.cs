﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using eShopOnContainers.Models.Catalog;
using eShopOnContainers.Services.RequestProvider;
using eShopOnContainers.Extensions;
using System.Collections.Generic;
using eShopOnContainers.Services.FixUri;
using eShopOnContainers.Helpers;

namespace eShopOnContainers.Services.Catalog
{
    public class CatalogService : ICatalogService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly IFixUriService _fixUriService;
		
        private const string ApiUrlBase = "c/api/v1/catalog";

        public CatalogService(IRequestProvider requestProvider, IFixUriService fixUriService)
        {
            _requestProvider = requestProvider;
            _fixUriService = fixUriService;
        }

        public async Task<IEnumerable<CatalogItem>> FilterAsync(int catalogBrandId, int catalogTypeId)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayShoppingEndpoint, $"{ApiUrlBase}/items/type/{catalogTypeId}/brand/{catalogBrandId}");

            CatalogRoot catalog = await _requestProvider.GetAsync<CatalogRoot>(uri).ConfigureAwait(false);

            if (catalog?.Data != null)
                return catalog?.Data;
            else
                return Enumerable.Empty<CatalogItem>();
        }

        public async Task<IEnumerable<CatalogItem>> GetCatalogAsync()
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayShoppingEndpoint, $"{ApiUrlBase}/items");

            CatalogRoot catalog = await _requestProvider.GetAsync<CatalogRoot>(uri).ConfigureAwait(false);

            if (catalog?.Data != null)
            {
                _fixUriService.FixCatalogItemPictureUri(catalog.Data);
                return catalog.Data;
            }
            else
                return Enumerable.Empty<CatalogItem>();
        }

        public async Task<IEnumerable<CatalogBrand>> GetCatalogBrandAsync()
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayShoppingEndpoint, $"{ApiUrlBase}/catalogbrands");

            IEnumerable<CatalogBrand> brands = await _requestProvider.GetAsync<IEnumerable<CatalogBrand>>(uri).ConfigureAwait(false);

            if (brands != null)
                return brands.ToArray();
            else
                return Enumerable.Empty<CatalogBrand>();
        }

        public async Task<IEnumerable<CatalogType>> GetCatalogTypeAsync()
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.GatewayShoppingEndpoint, $"{ApiUrlBase}/catalogtypes");

            IEnumerable<CatalogType> types = await _requestProvider.GetAsync<IEnumerable<CatalogType>>(uri).ConfigureAwait(false);

            if (types != null)
                return types.ToArray();
            else
                return Enumerable.Empty<CatalogType>();
        }
    }
}
