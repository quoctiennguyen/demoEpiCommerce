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
    [CatalogContentType(GUID = "13eebf25-0671-4758-9cc5-848fc4731925", MetaClassName="My_Product")]
    public class MyProduct : ProductContent
    {
        public virtual string MadeIn { get; set; }
        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Description", Order = 2)]
        public virtual XhtmlString Description { get; set; }
    }
}