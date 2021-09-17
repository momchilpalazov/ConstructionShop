using ConstructionShop.Data;
using ConstructionShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionShop.Controllers
{
    [Authorize(Roles =WebConstants.AdminRole)]
    public class ProductController : Controller
    {
        //dependency injection
        private readonly ConstructionShopDbContext data;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(ConstructionShopDbContext db, IWebHostEnvironment webHost)
        {
            data = db;
            webHostEnvironment = webHost;
        }

        public IActionResult Index()
        {
            //import Eager Loading for productivity and efincety
            IEnumerable<Product> categoryList = data.Products.Include(c=>c.Category).Include(a=>a.ApplicationType);


            //Here to many DB calls
            //foreach ( var obj in categoryList)
            //{
            //    obj.Category = data.Categories.FirstOrDefault(p=>p.Id==obj.CategoryId);
            //    obj.ApplicationType = data.ApplicationTypes.FirstOrDefault(p => p.Id == obj.ApplicationTypeId);
            //}
            return View(categoryList);
        }

        //GET For UpdateAndCreate/ This is method for Update and Create
        public IActionResult UpdateAndCreate(int? id)
        {
            //    IEnumerable<SelectListItem> DropDown = data.Categories.Select(c => new SelectListItem
            //    {
            //        Text=c.Name,
            //        Value=c.Id.ToString()
            //    });

            //    ViewBag.DropDown = DropDown;
            //    var product = new Product();

            ProductViewModel productViweModel = new ProductViewModel
            {
                Product = new Product(),
                CategorySelectList = data.Categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),

                ApplicationTypeSelectList = data.ApplicationTypes.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })

            };
            if (id==null)
            {
                //we must create

                return View(productViweModel);
            }

            else
            {
                productViweModel.Product = data.Products.Find(id);
                if  (productViweModel.Product==null)
                {
                    return NotFound();
                }
                return View(productViweModel);
            }
                  
        }

        //Post for UpdateAndCreate/ This is method for Update and Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateAndCreate(ProductViewModel objProduct)
        {

            if (ModelState.IsValid)
            {

                var file = HttpContext.Request.Form.Files;
                var roothPath = webHostEnvironment.WebRootPath;

                if (objProduct.Product.Id==0)
                {
                    //create
                    var upload = roothPath + WebConstants.PicturesPath;
                    var fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(file[0].FileName);

                    using (var fileStream=new FileStream(Path.Combine(upload,fileName+extension),FileMode.Create))
                    {
                        file[0].CopyTo(fileStream);
                    }

                    objProduct.Product.Pictures = fileName + extension;

                    data.Products.Add(objProduct.Product);
                }
                else
                {
                    //updating
                    var produktFromDB = data.Products.AsNoTracking().FirstOrDefault(p=>p.Id==objProduct.Product.Id);
                    if (file.Count>0)
                    {
                        var upload = roothPath + WebConstants.PicturesPath;
                        var fileName = Guid.NewGuid().ToString();
                        var extension = Path.GetExtension(file[0].FileName);

                        //caring for existing file in db
                        var fileOld = Path.Combine(upload,produktFromDB.Pictures);

                        if (System.IO.File.Exists(fileOld))
                        {
                            System.IO.File.Delete(fileOld);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            file[0].CopyTo(fileStream);
                        }

                        objProduct.Product.Pictures = fileName + extension;

                    }

                    else
                    {
                        objProduct.Product.Pictures = produktFromDB.Pictures;
                    }

                    data.Products.Update(objProduct.Product);
                }

                data.SaveChanges();
                return RedirectToAction("Index");
            }

            objProduct.CategorySelectList = data.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            objProduct.ApplicationTypeSelectList = data.ApplicationTypes.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            return View(objProduct);
            
        }

       

        //GET For Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = data.Products.Include(p=>p.Category).Include(p=>p.ApplicationType).FirstOrDefault(p=>p.Id==id);
            product.Category = data.Categories.Find(product.CategoryId);
            

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        //Post for Delete

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {


            var obj = data.Products.Find(id);
            if (obj==null)
            {
                return NotFound();
               
            }

            var upload = webHostEnvironment.WebRootPath + WebConstants.PicturesPath;
           

            //caring for existing file in db
            var fileOld = Path.Combine(upload, obj.Pictures);

            if (System.IO.File.Exists(fileOld))
            {
                System.IO.File.Delete(fileOld);
            }


            data.Products.Remove(obj);
            data.SaveChanges();
            return RedirectToAction("Index");

           
        }
    }
}
