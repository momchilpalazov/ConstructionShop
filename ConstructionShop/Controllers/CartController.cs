using ConstructionShop.Data;
using ConstructionShop.Models;
using ConstructionShop.Uttility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {

        private readonly ConstructionShopDbContext data;

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
