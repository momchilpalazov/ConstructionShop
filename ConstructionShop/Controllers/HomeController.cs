using ConstructionShop.Data;
using ConstructionShop.Models;
using ConstructionShop.Uttility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ConstructionShopDbContext data;

        public HomeController(ILogger<HomeController> logger, ConstructionShopDbContext db)
        {
            _logger = logger;
            data = db;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Products = data.Products.Include(p => p.Category).Include(a => a.ApplicationType),
                Categories = data.Categories

            };

            return View(homeViewModel);
        }

        public IActionResult Details(int id)
        {
            List<BagCart> shopingBagCart = new List<BagCart>();
            if (HttpContext.Session.Get<IEnumerable<BagCart>>(WebConstants.CartSession) != null
                && HttpContext.Session.Get<List<BagCart>>(WebConstants.CartSession).Count() > 0)
            {
                shopingBagCart = HttpContext.Session.Get<List<BagCart>>(WebConstants.CartSession);
            }

            DetailsViewModel detailsViewModels = new DetailsViewModel()
            {
                Product = data.Products.Include(p => p.Category).Include(a => a.ApplicationType)
                .Where(u => u.Id == id).FirstOrDefault(),
                IsItOnCart = false

            };

            foreach (var itemList in shopingBagCart)
            {
                if (itemList.ProductId==id)
                {
                    detailsViewModels.IsItOnCart = true;
                }
            }


            return View(detailsViewModels);

        }

        [HttpPost,ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {

            List<BagCart> shopingBagCart = new List<BagCart>();

            if (HttpContext.Session.Get<IEnumerable<BagCart>>(WebConstants.CartSession)!=null
                && HttpContext.Session.Get<List<BagCart>>(WebConstants.CartSession).Count()>0)
            {
                shopingBagCart = HttpContext.Session.Get<List<BagCart>>(WebConstants.CartSession);
            }
            shopingBagCart.Add(new BagCart {ProductId=id });
            HttpContext.Session.Set(WebConstants.CartSession,shopingBagCart);

            return RedirectToAction(nameof(Index));

        }

        
        public IActionResult RemoveFromCart(int id)
        {

            List<BagCart> shopingBagCart = new List<BagCart>();

            if (HttpContext.Session.Get<IEnumerable<BagCart>>(WebConstants.CartSession) != null
                && HttpContext.Session.Get<List<BagCart>>(WebConstants.CartSession).Count() > 0)
            {
                shopingBagCart = HttpContext.Session.Get<List<BagCart>>(WebConstants.CartSession);
            }
            var productToRemove = shopingBagCart.SingleOrDefault(p => p.ProductId == id);

            if (productToRemove!=null)
            {
                shopingBagCart.Remove(productToRemove);
            }

            
            HttpContext.Session.Set(WebConstants.CartSession, shopingBagCart);

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
