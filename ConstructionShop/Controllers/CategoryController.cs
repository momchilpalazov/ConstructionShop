﻿using ConstructionShop.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionShop.Controllers
{
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
            data.Categories.Add(obj);
            data.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
