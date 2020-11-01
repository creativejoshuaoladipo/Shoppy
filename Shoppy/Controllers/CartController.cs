using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Shoppy.Data;
using Shoppy.Models;
using Shoppy.Models.ViewModels;
using Shoppy.Utility;

namespace Shoppy.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ShoppyDbContext _db;

        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }

        private IWebHostEnvironment _webHostEnvironment;
        private IEmailSender _emailSender;
        public CartController(ShoppyDbContext db, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            List<ShoppingCart> cartList = new List<ShoppingCart>();

            if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.CartSession)!= null 
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.CartSession).Count()> 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.CartSession).ToList();
            }

            List<int> prdInCart = cartList.Select(i => i.ProductId).ToList();

            IEnumerable<Product> productList = _db.Product.Where(prd => prdInCart.Contains(prd.Id));
            
            return View(productList);
        }
        [HttpPost,ActionName("Index")]
        public IActionResult IndexPost()
        {
            
            return RedirectToAction(nameof(Summary));
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
          //or  var userId = User.FindFirst(ClaimTypes.Name);

            List<ShoppingCart> cartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.CartSession) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.CartSession).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.CartSession).ToList();
            }

            List<int> prdInCart = cartList.Select(i => i.ProductId).ToList();

            IEnumerable<Product> productList = _db.Product.Where(prd => prdInCart.Contains(prd.Id));

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = _db.ApplicationUser.Where(i => i.Id == claim.Value).FirstOrDefault(),
                ProductsList = productList.ToList()
            };

            return View(ProductUserVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserVM ProductUserVM)
        {
            var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "templates" + Path.DirectorySeparatorChar.ToString() +
                "Inquiry.html";

            var subject = "New Inquiry";
            string HtmlBody = "";
            using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }
            //Name: { 0}
            //Email: { 1}
            //Phone: { 2}
            //Products: {3}

             StringBuilder productListSB = new StringBuilder();
            foreach (var prod in ProductUserVM.ProductsList)
            {
                productListSB.Append($" - Name: { prod.Name} <span style='font-size:14px;'> (ID: {prod.Id})</span><br />");
            }

            string messageBody = string.Format(HtmlBody,
                ProductUserVM.ApplicationUser.FullName,
                ProductUserVM.ApplicationUser.Email,
                ProductUserVM.ApplicationUser.PhoneNumber,
                productListSB.ToString());


            await _emailSender.SendEmailAsync(WC.EmailAdmin, subject, messageBody);

            return RedirectToAction(nameof(InquiryConfirmation));
        }
        public IActionResult InquiryConfirmation()
        {

            HttpContext.Session.Clear();
            return View();
        }
        public IActionResult Remove(int id)
        {
            List<ShoppingCart> cartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.CartSession) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.CartSession).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.CartSession).ToList();
            }

            cartList.Remove(cartList.FirstOrDefault(i => i.ProductId == id));

            HttpContext.Session.Set<IEnumerable<ShoppingCart>>(WC.CartSession, cartList);
            return RedirectToAction(nameof(Index));
        }


    }
}
