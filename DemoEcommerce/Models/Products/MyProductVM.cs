using EPiServer.Core;
using Mediachase.Commerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoEcommerce.Models.Products
{
    public class MyProductVM
    {
        public MyProduct Product { get; set; }
        public string Name { get; set; }
        public string VariantName { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string VariantCode { get; set; }
        public Money Price { get; set; }
        public ContentReference CheckoutPage { get; set; }
        public decimal CartItemNumber { get; set; }
    }
}