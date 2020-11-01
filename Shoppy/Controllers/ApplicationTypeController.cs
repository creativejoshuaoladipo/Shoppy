using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoppy.Data;
using Shoppy.Models;

namespace Shoppy.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly ShoppyDbContext _db;
        public ApplicationTypeController(ShoppyDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> applicationTypes = _db.ApplicationType;
            return View(applicationTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType app)
        {
            if (ModelState.IsValid)
            {
                _db.Add(app);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(app);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var item = _db.ApplicationType.Find(id);

            if(item == null)
            {
                return NotFound();
            }
            
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType app)
        {
            if (ModelState.IsValid)
            {
                _db.Update(app);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(app);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var item = _db.ApplicationType.Find(id);

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
           if (id== null)
            {
                return NotFound();
            }
            var item = _db.ApplicationType.Find(id);
                _db.Remove(item);
                _db.SaveChanges();
                return RedirectToAction("Index");
            
        }
    }
}
