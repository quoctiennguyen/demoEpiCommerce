using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using DemoEcommerce.Models.Pages;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Catalog.Dto;
using DemoEcommerce.Models.Catalog;
using DemoEcommerce.Models.Products;

namespace DemoEcommerce.Controllers
{
    public class StartPageController : PageController<StartPage>
    {
        public ActionResult Index(StartPage currentPage)
        {
            //get catalog list
            ICatalogSystem target = CatalogContext.Current;
            CatalogDto catalogs = target.GetCatalogDto();
            List<MyNodeVM> listCatelog = new List<MyNodeVM>();
            foreach (CatalogDto.CatalogRow catalog in catalogs.Catalog)
            {
                listCatelog.Add(new MyNodeVM()
                {
                    CatalogName = catalog.Name
                });
            }
            currentPage.Catelogs = listCatelog;
            return View(currentPage);
        }
    }
}