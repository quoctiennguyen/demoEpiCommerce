using EPiServer.Commerce.Catalog.ContentTypes;
using Mediachase.Commerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoEcommerce.Models.Cart
{
    public class CartItem
    {
        public string DisplayName { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public string Code { get; set; }
        public VariationContent Variant { get; set; }
        public decimal Quantity { get; set; }
        public Money Price { get; set; }
    }
}