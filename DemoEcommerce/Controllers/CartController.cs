using DemoEcommerce.Models.Cart;
using DemoEcommerce.Services;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoEcommerce.Controllers
{
    public class CartController : Controller
    {
        [HttpPost]
        public ActionResult AddToCart(string code)
        {
            ModelState.Clear();
            string warningMessage = null;
            CartService cartService = new CartService();
            if (cartService.AddToCart(code, out warningMessage))
            {
                return MiniCartDetails();
            }

            // HttpStatusMessage can't be longer than 512 characters.
            warningMessage = warningMessage.Length < 512 ? warningMessage : warningMessage.Substring(512);
            return new HttpStatusCodeResult(500, warningMessage);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult MiniCartDetails()
        {
            var _cartService = new CartService();
            var viewModel = new MiniCartViewModel
            {
                ItemCount = _cartService.GetLineItemsTotalQuantity(),
                CartItems = _cartService.GetCartItems(),
                Total = _cartService.GetSubTotal()
            };

            return Json(new {Quantity = viewModel.ItemCount});
        }
    }
}