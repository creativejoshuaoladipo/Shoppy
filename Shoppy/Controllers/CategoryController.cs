using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoppy.Data;
using Shoppy.Models;

namespace Shoppy.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ShoppyDbContext _db;
        public CategoryController(ShoppyDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> catList=  _db.Category;
            return View(catList);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category cat)
        {
            _db.Add(cat);
            _db.SaveChanges();
            return RedirectToAction("Index");

            //Using ServerSide Validation
            //if (ModelState.IsValid)
            //{
            //    _db.Add(cat);
            //    _db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(cat);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var item = _db.Category.Find(id);

            if(item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category cat)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(cat);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cat);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var item = _db.Category.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {       
            var item = _db.Category.Find(id);
            if (item != null)
            {
                _db.Remove(item);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
