using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using DemoEcommerce.Models.Products;
using EPiServer.Web.Routing;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.ServiceLocation;
using DemoEcommerce.Services;
using Mediachase.Commerce;
using Mediachase.Commerce.Pricing;
using System;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Core;
using DemoEcommerce.Models.Pages;

namespace DemoEcommerce.Controllers
{
    public class MyProductController : ContentController<MyProduct>
    {
        public ActionResult Index(MyProduct currentContent, string variationCode = "")
        {
            ProductService ps = new ProductService();
            var variations = ps.GetVariations(currentContent);

            MyVariantion variation;
            if (!TryGetFashionVariant(variations, variationCode, out variation))
            {
                return HttpNotFound();
            }
            var _currentMarket = ServiceLocator.Current.GetInstance<ICurrentMarket>();
           
            var market = _currentMarket.GetCurrentMarket();
            var curency = market.Currencies.Where(x => x.CurrencyCode == market.DefaultCurrency).Cast<Currency>().FirstOrDefault();
            var defaultPrice = GetDefaultPrice(variation, market, curency);

            MyProductVM model = new MyProductVM();
            model.Product = currentContent;
            model.VariantName = variation.Name;
            model.Price = defaultPrice.UnitPrice;
            model.VariantCode = variationCode;
            model.Image = AssetHelper.GetAssetUrl(variation.CommerceMediaCollection);

            var _cartService = new CartService();
            model.CartItemNumber = _cartService.GetLineItemsTotalQuantity();

            var _contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            model.CheckoutPage = _contentLoader.Get<StartPage>(ContentReference.StartPage).CheckoutPage;
            
            return View(model);
        }
        private static bool TryGetFashionVariant(IEnumerable<MyVariantion> variations, string variationCode, out MyVariantion variation)
        {
            variation = !string.IsNullOrEmpty(variationCode) ?
                variations.FirstOrDefault(x => x.Code == variationCode) :
                variations.FirstOrDefault();

            return variation != null;
        }
        private IPriceValue GetDefaultPrice(MyVariantion variation, IMarket market, Currency currency)
        {
            var _priceService = ServiceLocator.Current.GetInstance<IPriceService>();
            return _priceService.GetDefaultPrice(
                market.MarketId,
                DateTime.Now,
                new CatalogKey(AppContext.Current.ApplicationId, variation.Code),
                currency);
        }
    }
}