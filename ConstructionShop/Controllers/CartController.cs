using ConstructionShop.Data;
using ConstructionShop.Models;
using ConstructionShop.Models.ViewModels;
using ConstructionShop.Uttility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConstructionShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {

        private readonly ConstructionShopDbContext data;

        [BindProperty]
        public ProductUserViewModel ProductUserViewModel { get; set; }

        public CartController(ConstructionShopDbContext db )
        {
            data = db;
            
        }
        public IActionResult Index()
        {
            List<BagCart> bagCartsList = new List<BagCart>();

            if (HttpContext.Session.Get<IEnumerable<BagCart>>(WebConstants.CartSession)!=null&&
                HttpContext.Session.Get<IEnumerable<BagCart>>(WebConstants.CartSession).Count() >0)
            {
                //sessionexist
                bagCartsList = HttpContext.Session.Get<List<BagCart>>(WebConstants.CartSession);
            }
            List<int> prodInCart = bagCartsList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productsList = data.Products.Where(u => prodInCart.Contains(u.Id));
            return View(productsList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {

            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {

            var claimsIdentiy = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentiy.FindFirst(ClaimTypes.NameIdentifier);
            //var userId = User.FindFirstValue(ClaimTypes.Name);

            List<BagCart> bagCartsList = new List<BagCart>();

            if (HttpContext.Session.Get<IEnumerable<BagCart>>(WebConstants.CartSession) != null &&
                HttpContext.Session.Get<IEnumerable<BagCart>>(WebConstants.CartSession).Count() > 0)
            {
                //sessionexist
                bagCartsList = HttpContext.Session.Get<List<BagCart>>(WebConstants.CartSession);
            }
            List<int> prodInCart = bagCartsList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productsList = data.Products.Where(u => prodInCart.Contains(u.Id));

            ProductUserViewModel = new ProductUserViewModel()
            {
                ApplicationUser = data.ApplicationUsers.FirstOrDefault(u => u.Id == claim.Value), ProductsList = productsList
                 

            };


            return View(ProductUserViewModel);
        }

        public IActionResult Remove(int id)
        {
            List<BagCart> bagCartsList = new List<BagCart>();

            if (HttpContext.Session.Get<IEnumerable<BagCart>>(WebConstants.CartSession) != null &&
                HttpContext.Session.Get<IEnumerable<BagCart>>(WebConstants.CartSession).Count() > 0)
            {
                //sessionexist
                bagCartsList = HttpContext.Session.Get<List<BagCart>>(WebConstants.CartSession);
            }

            bagCartsList.Remove(bagCartsList.FirstOrDefault(f => f.ProductId == id));
            HttpContext.Session.Set(WebConstants.CartSession, bagCartsList);
            return RedirectToAction(nameof(Index));
        }


       
    }
}
