using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Website;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoEcommerce.Services.paymentMethods
{
    public class DemoPaymentMethod: IPaymentOption
    {
        public DemoPaymentMethod()
        {
            PaymentMethodId = new Guid();
        }
        public Guid PaymentMethodId { get; set; }
        public bool PostProcess(Mediachase.Commerce.Orders.OrderForm orderForm)
        {
            if (orderForm == null)
            {
                throw new ArgumentNullException("form");
            }

            var payment = orderForm.Payments.ToArray().FirstOrDefault(x => x.PaymentMethodId == this.PaymentMethodId);
            if (payment == null)
            {
                return false;
            }

            payment.Status = PaymentStatus.Processed.ToString();
            payment.AcceptChanges();

            return true;
        }

        public Mediachase.Commerce.Orders.Payment PreProcess(Mediachase.Commerce.Orders.OrderForm form)
        {
            if (form == null)
            {
                throw new ArgumentNullException("form");
            }

            var payment = new OtherPayment
            {
                PaymentMethodId = PaymentMethodId,
                PaymentMethodName = "DemoPaymentMethod",
                OrderFormId = form.OrderFormId,
                OrderGroupId = form.OrderGroupId,
                Amount = form.Total,
                Status = PaymentStatus.Pending.ToString(),
                TransactionType = TransactionType.Authorization.ToString()
            };

            return payment;
        }

        public bool ValidateData()
        {
            throw new NotImplementedException();
        }
    }
}