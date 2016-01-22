using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Commerce.Catalog.ContentTypes;

namespace DemoEcommerce.Models.Products
{
    [CatalogContentType(GUID = "81912940-ee8b-4d38-9648-a6fda0d6bf44", MetaClassName = "My_Variantion")]
    public class MyVariantion : VariationContent
    {
        public virtual string Color { get; set; }
        public virtual string UserChoice { get; set; }
    }
}