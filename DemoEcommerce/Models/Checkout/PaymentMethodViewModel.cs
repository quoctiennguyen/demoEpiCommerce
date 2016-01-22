using Mediachase.Commerce;
using Mediachase.Commerce.Website;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoEcommerce.Models.Checkout
{
    public class PaymentMethodViewModel<T> : IPaymentMethodViewModel<T> where T : IPaymentOption
    {
        public Guid Id { get; set; }
        public string SystemName { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public MarketId MarketId { get; set; }
        public int Ordering { get; set; }
        public bool IsDefault { get; set; }
        public T PaymentMethod { get; set; }
    }
}