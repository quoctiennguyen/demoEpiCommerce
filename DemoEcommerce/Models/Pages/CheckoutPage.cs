using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;

namespace DemoEcommerce.Models.Pages
{
    [ContentType(DisplayName = "CheckoutPage", GUID = "0061a6bf-c83b-402b-85ba-bd7857b3fe44", Description = "")]
    public class CheckoutPage : PageData
    {
    }
}