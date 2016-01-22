using DemoEcommerce.Models.Products;
using DemoEcommerce.Services;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Framework.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using Mediachase.Commerce.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoEcommerce.Controllers
{
    [TemplateDescriptor(Inherited = true)]
    public class ProductPartialController : PartialContentController<ProductContent>
    {
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public override ActionResult Index(ProductContent currentContent)
        {
            var model = new MyProductVM()
            {
                Name = currentContent.Name,
                Image = AssetHelper.GetAssetUrl(currentContent.CommerceMediaCollection)
            };
            ProductService ps = new ProductService();
            var variantProduct = ps.GetProductViewModel(currentContent);
            model.Link = variantProduct.Link;
            return PartialView("_Product", model);
        }

        private void GetImage()
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var loader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var referenceConvertor = ServiceLocator.Current.GetInstance<ReferenceConverter>();

            //var variant = loader.Get<VariationContent>(referenceConvertor.GetContentLink(catalogEntryDto.CatalogEntry[0].Code)); // Get variant from new API
            //var entry = variant.LoadEntry();

            //var contentId = GetContentIDFronFileName(row.VariationImageName);
            //var image = loader.Get<MediaData>(new ContentReference(contentId));
        }
    }

    
}