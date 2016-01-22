using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;

namespace DemoEcommerce.Models.Pages
{
    [ContentType(DisplayName = "ThankYouPage", GUID = "0089796d-6e76-4b0c-b4f5-53b997a500e2", Description = "")]
    public class ThankYouPage : PageData
    {
                [CultureSpecific]
                [Display(
                    Name = "Main body",
                    Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
                    GroupName = SystemTabNames.Content,
                    Order = 1)]
                public virtual XhtmlString MainBody { get; set; }
    }
}