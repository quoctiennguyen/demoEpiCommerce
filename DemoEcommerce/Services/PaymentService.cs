using Mediachase.Commerce.Website;
using Mediachase.Commerce.Website.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoEcommerce.Services
{
    public class PaymentService
    {
        public void ProcessPayment(IPaymentOption method)
        {
            var cart = CartHelper.Cart;

            if (!cart.OrderForms.Any())
            {
                cart.OrderForms.AddNew();
            }

            var payment = method.PreProcess(cart.OrderForms[0]);

            if (payment == null)
            {
                throw new PreProcessException();
            }

            cart.OrderForms[0].Payments.Add(payment);
            cart.AcceptChanges();

            method.PostProcess(cart.OrderForms[0]);
        }
        private CartHelper CartHelper
        {
            get { return new CartHelper(Mediachase.Commerce.Orders.Cart.DefaultName); }
        }
    }

    public class PreProcessException : Exception
    {
    }
}