using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Commerce.Catalog.ContentTypes;
using Mediachase.Commerce.Catalog.Dto;
using System.Collections.Generic;
using DemoEcommerce.Models.Catalog;

namespace DemoEcommerce.Models.Pages
{
    [ContentType(DisplayName = "StartPage", GUID = "ca35f808-4355-4588-9005-d2a5c7e41902", Description = "")]
    public class StartPage : PageData
    {

        [CultureSpecific]
        [Display(
            Name = "Main body",
            Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual XhtmlString MainBody { get; set; }

        public virtual string Header { get; set; }
        [Display(
            Name = "Product area",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        [AllowedTypes(typeof(EntryContentBase))]
        public virtual ContentArea ProductArea { get; set; }
        [Ignore]
        public List<MyNodeVM> Catelogs { get; set; }

        [Display(
            Name = "Select checkout page",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 3)]
        public virtual PageReference CheckoutPage { get; set; }
        public virtual PageReference ThankyouPage { get; set; }
    }
}