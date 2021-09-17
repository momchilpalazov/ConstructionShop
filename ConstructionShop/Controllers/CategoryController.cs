using ConstructionShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionShop.Controllers
{
    [Authorize(Roles = WebConstants.AdminRole)]
    public class CategoryController : Controller
    {

        private readonly ConstructionShopDbContext data;

        public CategoryController(ConstructionShopDbContext db)
        {
            data = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categoryList = data.Categories;
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
        public IActionResult Create(Category obj)
        {

            if (ModelState.IsValid)
            {
                data.Categories.Add(obj);
                data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
            
        }

        //GET For Edit
        public IActionResult Edit(int? id)
        {
            if (id==null || id==0)
            {
                return NotFound();
            }

            var obj =data.Categories.Find(id);

            if (obj==null)
            {
                return NotFound();
            }

            return View(obj);
        }


        //Post for Edit

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                data.Categories.Update(obj);
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

            var obj = data.Categories.Find(id);

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
            var obj = data.Categories.Find(id);
            if (obj==null)
            {
                return NotFound();
               
            }

            data.Categories.Remove(obj);
            data.SaveChanges();
            return RedirectToAction("Index");

           
        }
    }
}
