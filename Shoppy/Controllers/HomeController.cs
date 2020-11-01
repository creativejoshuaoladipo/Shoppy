using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shoppy.Data;
using Shoppy.Models;
using Shoppy.Models.ViewModels;
using Shoppy.Utility;

namespace Shoppy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShoppyDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ShoppyDbContext db)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Products = _db.Product.Include(x => x.Category).Include(x => x.ApplicationType),
                Categories = _db.Category
            };

            return View(homeVM);
        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> cartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.CartSession) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.CartSession).Count() > 0)
            {
                cartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.CartSession);
            }


            DetailVM detail = new DetailVM()
            {
                Product = _db.Product.Include(a => a.Category).Include(b => b.ApplicationType)
                .Where(x => x.Id == id).FirstOrDefault(),

                ExistInCart = false
                
            };

            foreach(var prod in cartList)
            {
                if(prod.ProductId == id)
                {
                    detail.ExistInCart = true;
                }
            }

            return View(detail);
        }

        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShoppingCart> cartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.CartSession)!=null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.CartSession).Count() > 0)
            {
                cartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.CartSession);
            }

            cartList.Add(new ShoppingCart { ProductId = id });

            HttpContext.Session.Set(WC.CartSession, cartList);

            return RedirectToAction(nameof(Index));
        }
        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> cartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.CartSession) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.CartSession).Count() > 0)
            {
                cartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.CartSession);
            }

            //Fetch for the Product in the CartList-- similiar to database 

            var productToRemove = cartList.SingleOrDefault(v => v.ProductId == id); 

            if (productToRemove != null)
            {
                cartList.Remove(productToRemove);
            }

            HttpContext.Session.Set(WC.CartSession, cartList);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
