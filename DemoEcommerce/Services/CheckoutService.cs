using DemoEcommerce.Models.Checkout;
using EPiServer.ServiceLocation;
using Mediachase.Commerce;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce.Website;
using Mediachase.Commerce.Website.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoEcommerce.Services
{
    public class CheckoutService
    {
        private ICurrentMarket _currentMarket;
        public CheckoutService()
        {
            _currentMarket = ServiceLocator.Current.GetInstance<ICurrentMarket>();
        }
        private MarketId CurrentMarketId
        {
            get { return _currentMarket.GetCurrentMarket().MarketId; }
        }
        private string CurrentLanguageIsoCode
        {
            get { return _currentMarket.GetCurrentMarket().DefaultLanguage.TwoLetterISOLanguageName ;}
        }
        public IEnumerable<PaymentMethodViewModel<IPaymentOption>> GetPaymentMethods()
        {
            var methods = PaymentManager.GetPaymentMethodsByMarket(CurrentMarketId.Value).PaymentMethod.Where(c => c.IsActive);
            var currentLanguage = CurrentLanguageIsoCode;
            return methods.
                Where(paymentRow => currentLanguage.Equals(paymentRow.LanguageId, StringComparison.OrdinalIgnoreCase)).
                OrderBy(paymentRow => paymentRow.Ordering).
                Select(paymentRow => new PaymentMethodViewModel<IPaymentOption>
                {
                    Id = paymentRow.PaymentMethodId,
                    SystemName = paymentRow.SystemKeyword,
                    FriendlyName = paymentRow.Name,
                    MarketId = CurrentMarketId,
                    Ordering = paymentRow.Ordering,
                    IsDefault = paymentRow.IsDefault,
                    Description = paymentRow.Description,
                }).ToList();
        }
        public PurchaseOrder SaveCartAsPurchaseOrder()
        {
            return CartHelper.Cart.SaveAsPurchaseOrder();
        }

        public void DeleteCart()
        {
            var cart = CartHelper.Cart;
            foreach (OrderForm orderForm in cart.OrderForms)
            {
                foreach (Shipment shipment in orderForm.Shipments)
                {
                    shipment.Delete();
                }
                orderForm.Delete();
            }
            foreach (OrderAddress address in cart.OrderAddresses)
            {
                address.Delete();
            }

            CartHelper.Delete();

            cart.AcceptChanges();
        }
        public void ClearOrderAddresses()
        {
            CartHelper.Cart.OrderAddresses.Clear();
        }
        private CartHelper CartHelper
        {
            get { return new CartHelper(Mediachase.Commerce.Orders.Cart.DefaultName); }
        }
    }
}