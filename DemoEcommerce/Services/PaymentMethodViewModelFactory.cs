using DemoEcommerce.Models.Checkout;
using DemoEcommerce.Services.paymentMethods;
using Mediachase.Commerce.Website;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoEcommerce.Services
{
    public class PaymentMethodViewModelFactory
    {
        public static IPaymentMethodViewModel<IPaymentOption> Resolve(PaymentMethodViewModel<IPaymentOption> paymentMethod)
        {
            var paymentMethodName = paymentMethod.SystemName;
            switch (paymentMethodName)
            {
                case "CashOnDelivery":
                    return null;//new CashOnDeliveryViewModel() { PaymentMethod = new CashOnDeliveryPaymentMethod() };
                case "DemoPaymentMethod"://DemoPaymentMethod
                    var returnPayment = new DemoPaymentMethodViewModel();
                    returnPayment.PaymentMethod = new DemoPaymentMethod();
                    returnPayment.PaymentMethod.PaymentMethodId = paymentMethod.Id;
                    return returnPayment;
                case "GenericCreditCard":

                    return null;//new GenericCreditCardViewModel() { PaymentMethod = new GenericCreditCardPaymentMethod() };
                default:
                    return new DemoPaymentMethodViewModel()
                    {
                        PaymentMethod = new DemoPaymentMethod()
                    };
            }

            throw new ArgumentException("No view model has been implemented for the method " + paymentMethodName, "paymentMethodName");
        }
    }
}