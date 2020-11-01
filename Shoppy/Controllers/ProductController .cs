using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Shoppy.Data;
using Shoppy.Models;
using Shoppy.Models.ViewModels;

namespace Shoppy.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShoppyDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductController(ShoppyDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        //Getting All Products
        public IActionResult Index()
        {
            IEnumerable<Product> prod = _db.Product.Include(x => x.Category).Include(c=>c.ApplicationType);

            return View(prod);
        
      //      IEnumerable<Product> prodList=  _db.Product;

      ////Since Category Table is mapped to the Product Table then we will include it mannually by populating the Product Model
      //      foreach(var prd in prodList)
      //      {
      //          //Linking the Foreign Key CategoryId to the main Category Table Id
      //          prd.Category = _db.Category.Where(cat => cat.Id == prd.CategoryId).FirstOrDefault();
      //      }
      //      return View(prodList);
        }



        /*
         Upesrt is used to create and edit a Product
        Create a Product- when the Id is Null- Creating an New Instance Object
        Edit the Product when the Id is given-- Fetching from the DataBase
         */
        //public IActionResult Upsert(int? id )
        //{
        //    ProductVM productVM = new ProductVM()
        //    {
        //        Product = new Product(),
        //        CategoryDropDownList = _db.Category.Select(i => new SelectListItem
        //        {
        //            Text = i.Name,
        //            Value = (i.Id).ToString()
        //        })
        //    };
        //    if (id == null)
        //    {
                
        //           return View(productVM);
        //    }


        //    //Creating a DropDown List for Category
        //    //    IEnumerable<SelectListItem> CategoryDropDownList = _db.Category.Select(i => new SelectListItem
        //    //    {
        //    //        Text = i.Name,
        //    //        Value = (i.Id).ToString()
        //    //    }) ;
        //    //    ViewBag.CategoryList = CategoryDropDownList;

        //    //    Product product = new Product();
        //    //    return View(product);
        //    //}
        //    else
        //    {
        //        productVM.Product = _db.Product.Find(id);
        //        if(productVM.Product== null)
        //        {
        //            return NotFound();
        //        }
        //        return View(productVM.Product);
        //    }        

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Upsert(ProductVM productVM)
        //{
        //  if ( ModelState.IsValid) {
        //        var files = HttpContext.Request.Form.Files;
        //        string webRootPath = _webHostEnvironment.WebRootPath;

        //        var upload = webRootPath + WC.ImagePath;
        //        var fileName = Guid.NewGuid().ToString();
        //        var extension = Path.GetExtension(files[0].FileName);

        //        using (var fileStream = new FileStream(Path.Combine(upload,fileName+extension),FileMode.Create) )
        //        {
        //            files[0].CopyTo(fileStream);
        //        }

        //        productVM.Product.Image = fileName + extension;

        //        _db.Product.Add(productVM.Product);
        //        _db.SaveChanges();
        //    }
        //    return View();


        //}



        public IActionResult Upsert(int? id)
        {

            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryDropDownList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ApplicationTypeDropDownList = _db.ApplicationType.Select(x=> new SelectListItem
                {
                    Text = x.Name,
                    Value =x.Id.ToString()
                })
                
            };
            //Create a new Product (there is no need of ID)
            if (id == null)
            {
                //this is for create
                return View(productVM);
            }
            else
            {
                //Edit Product (Fetch record from db using the edit it and pass it to the Product Property )
                productVM.Product = _db.Product.Find(id);
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }


        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;

                    _db.Product.Add(productVM.Product);
                }
                else
                {
                    //updating
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }
                    _db.Product.Update(productVM.Product);
                }


                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            productVM.CategoryDropDownList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            productVM.ApplicationTypeDropDownList = _db.ApplicationType.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(productVM);

        }


        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = _db.Product.Include(u => u.Category).Include(v=>v.ApplicationType).FirstOrDefault(u => u.Id == id);
            //product.Category = _db.Category.Find(product.CategoryId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }


            _db.Product.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");


        }



        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var item = _db.Product.Include(x => x.Id).Include(s => s.ApplicationType).FirstOrDefault(c => c.Id == id);
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(item);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeletePost(int? id)
        //{
        //   var item = _db.Product.Include(c => c.Category).Include(s => s.ApplicationType).FirstOrDefault(x => x.Id == id);
        //    if (item != null)
        //    {
        //        _db.Remove(item);
        //        _db.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}
