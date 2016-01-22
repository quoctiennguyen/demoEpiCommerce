using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using DemoEcommerce.Models.Pages;
using DemoEcommerce.Services;
using DemoEcommerce.Models.Checkout;
using Mediachase.Commerce.Website;
using EPiServer.Web.Mvc.Html;
using Mediachase.Commerce.Orders.Managers;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Orders;
using System;
namespace DemoEcommerce.Controllers
{
    public class CheckoutPageController : PageController<CheckoutPage>
    {
        public ActionResult Index(CheckoutPage currentPage)
        {
            //get all item in cards
            var _cartService = new CartService();
            var items = _cartService.GetCartItems().ToList();
            //get all payment declare in BE
            var checkoutService = new CheckoutService();
            var paymentMethods = checkoutService.GetPaymentMethods();

            CheckoutPageViewModel model = new CheckoutPageViewModel();
            model.CartItems = items;
            model.PaymentMethodViewModels = paymentMethods;
            return View(model);
        }
        [HttpPost]
        public ActionResult Purchase(CheckoutPage currentPage, CheckoutViewModel checkoutViewModel, string selectPayment)
        {
            var _cartService = new CartService();
            var checkoutService = new CheckoutService();
            var paymentMethods = checkoutService.GetPaymentMethods();
            var paymentMethod = paymentMethods.Where(x => x.Id == new Guid(selectPayment)).SingleOrDefault(); //PaymentMethodViewModelFactory.Resolve(selectPayment);
            var choosePayment = PaymentMethodViewModelFactory.Resolve(paymentMethod);
            checkoutService.ClearOrderAddresses();

            
            _cartService.RunWorkflow(OrderGroupWorkflowManager.CartPrepareWorkflowName);
            _cartService.SaveCart();
            //star payment
            var _paymentService = new PaymentService();
            try
            {
                _paymentService.ProcessPayment(choosePayment.PaymentMethod);
            }
            catch (PreProcessException)
            {
                ModelState.AddModelError("PaymentMethod", "Payment Preprocess fail");
            }
            //add order form
            PurchaseOrder purchaseOrder = null;
            _cartService = new CartService();
            OrderForm orderForm = _cartService.GetOrderForms().First();
            decimal totalProcessedAmount = orderForm.Payments.Where(x => x.Status.Equals(PaymentStatus.Processed.ToString())).Sum(x => x.Amount);
            if (totalProcessedAmount != orderForm.Total)
            {
                throw new InvalidOperationException("Wrong amount");
            }
            _cartService.RunWorkflow(OrderGroupWorkflowManager.CartCheckOutWorkflowName);
            purchaseOrder = checkoutService.SaveCartAsPurchaseOrder();
            checkoutService.DeleteCart();

            IContentRepository _contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var startpage = _contentRepository.Get<StartPage>(ContentReference.StartPage);
            var thankyouPage = _contentRepository.GetChildren<ThankYouPage>(startpage.ContentLink).FirstOrDefault();
            return Redirect(new UrlBuilder(thankyouPage.LinkURL).ToString());
        }
    }
}