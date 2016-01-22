using DemoEcommerce.Models.Products;
using EPiServer.Commerce.Catalog.ContentTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoEcommerce.Services
{
    public interface IProductService
    {
        MyProductVM GetProductViewModel(ProductContent product);
       
    }
}