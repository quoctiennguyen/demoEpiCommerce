using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Commerce.Catalog.ContentTypes;

namespace DemoEcommerce.Models.Catalog
{
    [CatalogContentType(GUID = "d20112bc-f595-47b4-a2cb-fae710edd4d6", MetaClassName="My_Node")]
    public class MyNode : NodeContent
    {
    }
}