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

            if (ModelState.IsValid)
            {
                data.ApplicationTypes.Add(obj);
                data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);

        }

        //GET For Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = data.ApplicationTypes.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }


        //Post for Edit

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType obj)
        {

            if (ModelState.IsValid)
            {
                data.ApplicationTypes.Update(obj);
                data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);

        }

        //GET For Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = data.ApplicationTypes.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }


        //Post for Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = data.ApplicationTypes.Find(id);
            if (obj == null)
            {
                return NotFound();

            }

            data.ApplicationTypes.Remove(obj);
            data.SaveChanges();
            return RedirectToAction("Index");


        }
    }
}
