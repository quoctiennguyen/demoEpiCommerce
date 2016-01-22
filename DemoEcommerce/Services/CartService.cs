using EPiServer.Security;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce.Website.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mediachase.Commerce.Security;
using Mediachase.Commerce.Orders;
using DemoEcommerce.Models.Cart;
using Mediachase.Commerce;
using DemoEcommerce.Services.Extensions;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using System.Globalization;
using EPiServer.Globalization;
using EPiServer.Core;
using EPiServer.Web.Routing;
using DemoEcommerce.Models.Products;
namespace DemoEcommerce.Services
{
    public class CartService
    {
        private CartHelper _cardhelper;
        private IContentLoader _contentLoader;
        private ReferenceConverter _referenceConverter;
        private readonly CultureInfo _preferredCulture;
        private UrlResolver _urlResolver;
        public CartService()
        {
            _cardhelper = new CartHelper(Mediachase.Commerce.Orders.Cart.DefaultName, PrincipalInfo.CurrentPrincipal.GetContactId());
            _contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            _referenceConverter = ServiceLocator.Current.GetInstance<ReferenceConverter>();
            _preferredCulture = ContentLanguage.PreferredCulture;
            _urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
        }

        public IEnumerable<OrderForm> GetOrderForms()
        {
            return _cardhelper.Cart.OrderForms.Count == 0 ? new[] { new OrderForm() } : _cardhelper.Cart.OrderForms.ToArray();
        }
        public bool AddToCart(string code, out string warningMessage)
        {
            var entry = CatalogContext.Current.GetCatalogEntry(code);

            _cardhelper.AddEntry(entry);
            _cardhelper.Cart.ProviderId = "frontend"; // if this is not set explicitly, place price does not get updated by workflow
            ValidateCart(out warningMessage, _cardhelper);

            return _cardhelper.LineItems.Select(x => x.Code).Contains(code);
        }
        public decimal GetLineItemsTotalQuantity()
        {
            return _cardhelper.Cart.GetAllLineItems().Sum(x => x.Quantity);
        }
        public Money GetSubTotal()
        {
            decimal amount = _cardhelper.Cart.SubTotal + _cardhelper.Cart.OrderForms.SelectMany(x => x.Discounts.Cast<OrderFormDiscount>()).Sum(x => x.DiscountAmount);

            return ConvertToMoney(amount);
        }
        public Money ConvertToMoney(decimal amount)
        {
            return new Money(amount, new Currency(_cardhelper.Cart.BillingCurrency));
        }
        public IEnumerable<CartItem> GetCartItems()
        {
            if (_cardhelper.IsEmpty)
            {
                return Enumerable.Empty<CartItem>();
            }

            var cartItems = new List<CartItem>();
            var lineItems = _cardhelper.Cart.GetAllLineItems();

            //// In order to show the images for the items in the cart, we need to load the variants
            var variants = _contentLoader.GetItems(lineItems.Select(x => _referenceConverter.GetContentLink(x.Code)),
                _preferredCulture).OfType<VariationContent>();

            foreach (var lineItem in lineItems)
            {
                VariationContent variant = variants.FirstOrDefault(x => x.Code == lineItem.Code);
                ProductContent product = _contentLoader.Get<ProductContent>(variant.GetParentProducts().FirstOrDefault());
                CartItem item = new CartItem
                {
                    Code = lineItem.Code,
                    DisplayName = lineItem.DisplayName,
                    ImageUrl = variant.GetAssets<IContentImage>(_contentLoader, _urlResolver).FirstOrDefault() ?? "",
                    Quantity = lineItem.Quantity,
                    Url = lineItem.GetUrl(),
                    Variant = variant,
                    Price = variant.GetPrices().FirstOrDefault().UnitPrice
                };
                cartItems.Add(item);
            }

            return cartItems;
        }

        private void ValidateCart(out string warningMessage, CartHelper cart)
        {
            var workflowResult = OrderGroupWorkflowManager.RunWorkflow(cart.Cart, OrderGroupWorkflowManager.CartValidateWorkflowName);
            var warnings = OrderGroupWorkflowManager.GetWarningsFromWorkflowResult(workflowResult).ToArray();
            warningMessage = warnings.Any() ? String.Join(" ", warnings) : null;
        }

        public void RunWorkflow(string workFlowName)
        {

            _cardhelper.RunWorkflow(workFlowName);
        }
        public void SaveCart()
        {
            _cardhelper.Cart.AcceptChanges();
        }
    }

    public static class CartExtensions
    {
        public static IReadOnlyCollection<LineItem> GetAllLineItems(this Mediachase.Commerce.Orders.Cart cart)
        {
            return cart.OrderForms.Any() ? cart.OrderForms.First().LineItems.ToList() : new List<LineItem>();
        }

        public static LineItem GetLineItem(this Mediachase.Commerce.Orders.Cart cart, string code)
        {
            return cart.GetAllLineItems().FirstOrDefault(x => x.Code == code);
        }
    }
}