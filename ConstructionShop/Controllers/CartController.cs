using ConstructionShop.Data;
using ConstructionShop.Models;
using ConstructionShop.Models.ViewModels;
using ConstructionShop.Uttility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {

        private readonly ConstructionShopDbContext data;

        private readonly IWebHostEnvironment webHostEnvironment;

        private readonly IEmailSender emailSender;

        [BindProperty]
        public ProductUserViewModel ProductUserViewModel { get; set; }

        public CartController(ConstructionShopDbContext db, IWebHostEnvironment webHost,IEmailSender esender )
        {
            data = db;
            webHostEnvironment = webHost;
            emailSender = esender;

            
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
                ApplicationUser = data.ApplicationUsers.FirstOrDefault(u => u.Id == claim.Value), ProductsList = productsList.ToList()
                 

            };


            return View(ProductUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task< IActionResult> SummaryPost(ProductUserViewModel productUserViewModel)
        {
            var PathToTheTemplate = webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString() + "tamplates" + Path.DirectorySeparatorChar.ToString() +
                 "Inquiry.html";

            var subject = "New Inquiry";
            string HtmlBody = "";

            using (StreamReader sr=System.IO.File.OpenText(PathToTheTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            
            }

            //Name: { 0}
            //Email: { 1}
            //Phone: { 2}
            //Products Interested:{ 3}

            StringBuilder productListSB = new StringBuilder();

            foreach (var prod in productUserViewModel.ProductsList)
            {
                productListSB.Append($" - Name:{prod.Name} <span style='font-size:14px;'>(ID: {prod.Id})</span><br />");
            }

            string messageBody = string.Format(HtmlBody,
                productUserViewModel.ApplicationUser.FullName,
                productUserViewModel.ApplicationUser.Email,
                productUserViewModel.ApplicationUser.PhoneNumber,
                productListSB.ToString());

            await emailSender.SendEmailAsync(WebConstants.EmailAdmin, subject, messageBody);

                return RedirectToAction(nameof(InquiryConfirmation));
        }

        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();
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
