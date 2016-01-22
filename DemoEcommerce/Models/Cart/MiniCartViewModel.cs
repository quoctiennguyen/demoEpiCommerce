using Mediachase.Commerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoEcommerce.Models.Cart
{
    public class MiniCartViewModel
    {
        public decimal ItemCount { get; set; }
        public IEnumerable<CartItem> CartItems { get; set; }
        public Money Total { get; set; }
    }
}