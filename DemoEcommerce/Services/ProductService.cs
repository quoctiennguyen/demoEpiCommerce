using DemoEcommerce.Models.Products;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Core;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using DemoEcommerce.Services.Extensions;
using Mediachase.Commerce;
using EPiServer.Filters;

namespace DemoEcommerce.Services
{
    /// <summary>
    /// reference: http://world.episerver.com/documentation/Items/Developers-Guide/EPiServer-Commerce/9/Content/Product-variants/
    /// </summary>
    [ServiceConfiguration(typeof(IProductService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class ProductService : IProductService
    {
        private readonly IContentLoader _contentLoader;
        private readonly CultureInfo _preferredCulture;
        private readonly IRelationRepository _relationRepository;
        private readonly ICurrentMarket _currentMarket;
        public ProductService()
        {
            _contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            _relationRepository = ServiceLocator.Current.GetInstance<IRelationRepository>();
            _currentMarket = ServiceLocator.Current.GetInstance<ICurrentMarket>();
            _preferredCulture = ContentLanguage.PreferredCulture;
        }
        public virtual MyProductVM GetProductViewModel(ProductContent product)
        {
            var variations = _contentLoader.GetItems(product.GetVariants(), _preferredCulture).
                                            Cast<VariationContent>()
                                           .ToList();

            var variation = variations.FirstOrDefault();
            var productVM = new MyProductVM();
            productVM.Name = variation.Name;
            productVM.Link = variation.GetUrl();
            return productVM;
        }
        /// <summary>
        /// Getting the variants for a product
        /// </summary>
        /// <param name="referenceToProduct"></param>
        /// <returns></returns>
        public IEnumerable<ProductVariation> ListVariations(ContentReference referenceToProduct)
        {
            var linksRepository = ServiceLocator.Current.GetInstance<ILinksRepository>();
            var allRelations = linksRepository.GetRelationsBySource(referenceToProduct);

            // Relations to Variations are of type ProductVariation
            var productVariation = allRelations.OfType<ProductVariation>().ToList();
            return productVariation;
        }

        public IEnumerable<MyVariantion> GetVariations(MyProduct currentContent)
        {
            return _contentLoader
                .GetItems(currentContent.GetVariants(_relationRepository), _preferredCulture)
                .Cast<MyVariantion>()
                .Where(v => v.IsAvailableInCurrentMarket(_currentMarket) && !(new FilterPublished()).ShouldFilter(v));
        }
        /// <summary>
        /// Getting the product by variant
        /// </summary>
        /// <param name="variation"></param>
        /// <returns></returns>
        public IEnumerable<ProductVariation> GetProductByVariant(ContentReference variation)
        {
            var linksRepository = ServiceLocator.Current.GetInstance<ILinksRepository>();
            var allRelations = linksRepository.GetRelationsByTarget(variation);

            // Relations to Product is ProductVariation
            return allRelations.OfType<ProductVariation>().ToList();
        }
    }
}