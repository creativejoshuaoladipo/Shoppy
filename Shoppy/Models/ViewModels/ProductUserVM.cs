using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppy.Models.ViewModels
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            ProductsList =  new List <Product>();
        }
        public ApplicationUser ApplicationUser { get; set; }
        public IList<Product> ProductsList { get; set; }


    }
}
