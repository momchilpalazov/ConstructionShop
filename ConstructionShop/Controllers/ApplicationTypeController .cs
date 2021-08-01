using ConstructionShop.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionShop.Controllers
{
    public class ApplicationTypeController : Controller
    {

        private readonly ConstructionShopDbContext data;

        public ApplicationTypeController(ConstructionShopDbContext db)
        {
            data = db;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> categoryList = data.ApplicationTypes;
            return View(categoryList);
        }

        //GET For Create
        public IActionResult Create()
        {
            return View();        
        }

        //Post for Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType obj)
        {
            data.ApplicationTypes.Add(obj);
            data.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
