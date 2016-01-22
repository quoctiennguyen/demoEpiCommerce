using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoEcommerce.Services.Extensions
{
    public static class ContentExtensions
    {
        private static Injected<ReferenceConverter> _referenceConverter;
        private static Injected<UrlResolver> _urlResolver;
        private static Injected<IContentLoader> _contentLoader;
        private static Injected<ILinksRepository> _linksRepository;
        public static string GetUrl(this VariationContent variant)
        {
            return GetUrl(variant, _linksRepository.Service, _urlResolver.Service);
        }
        public static string GetUrl(this LineItem lineItem)
        {
            var variantLink = _referenceConverter.Service.GetContentLink(lineItem.Code);
            var variant = _contentLoader.Service.Get<VariationContent>(variantLink);
            return variant.GetUrl();
        }
        public static string GetUrl(this VariationContent variant, ILinksRepository linksRepository, UrlResolver urlResolver)
        {
            var productLink = variant.GetParentProducts(linksRepository).FirstOrDefault();
            if (productLink == null)
            {
                return string.Empty;
            }
            var link = urlResolver.GetUrl(productLink);
            if (link != null)
            {
                var urlBuilder = new UrlBuilder(urlResolver.GetUrl(productLink));
                if (variant.Code != null)
                {
                    urlBuilder.QueryCollection.Add("variationCode", variant.Code);
                }
                return urlBuilder.ToString();
            }
            else
            {
                return "";
            }
        }

        public static IList<string> GetAssets<TContentMedia>(this IAssetContainer assetContainer, IContentLoader contentLoader, UrlResolver urlResolver)
            where TContentMedia : IContentMedia
        {
            var assets = new List<string>();
            if (assetContainer.CommerceMediaCollection != null)
            {
                assets.AddRange(assetContainer.CommerceMediaCollection.Where(x => ValidateCorrectType<TContentMedia>(x.AssetLink, contentLoader)).Select(media => urlResolver.GetUrl(media.AssetLink)));
            }

            if (!assets.Any())
            {
                assets.Add(string.Empty);
            }

            return assets;
        }

        private static bool ValidateCorrectType<TContentMedia>(ContentReference contentLink, IContentLoader contentLoader)
            where TContentMedia : IContentMedia
        {
            if (typeof(TContentMedia) == typeof(IContentMedia))
            {
                return true;
            }

            if (ContentReference.IsNullOrEmpty(contentLink))
            {
                return false;
            }

            TContentMedia content;
            return contentLoader.TryGet(contentLink, out content);
        }
    }
}