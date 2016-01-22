using DemoEcommerce.Models.Cart;
using DemoEcommerce.Models.Checkout;
using Mediachase.Commerce;
using Mediachase.Commerce.Website;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoEcommerce.Models.Pages
{
    public class CheckoutPageViewModel
    {
        public IEnumerable<CartItem> CartItems { get; set; }
        public decimal ItemCount { get; set; }
        public Money Total { get; set; }
        /// <summary>
        /// get all payment in be to display for user choose
        /// </summary>
        public IEnumerable<PaymentMethodViewModel<IPaymentOption>> PaymentMethodViewModels { get; set; }
    }
}