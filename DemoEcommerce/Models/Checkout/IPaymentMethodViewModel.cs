using Mediachase.Commerce.Website;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoEcommerce.Models.Checkout
{
    public interface IPaymentMethodViewModel<out T> where T : IPaymentOption
    {
        T PaymentMethod { get; }
        string Description { get; set; }
        string SystemName { get; set; }
    }
}