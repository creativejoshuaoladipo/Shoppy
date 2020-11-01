using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppy.Models.ViewModels
{
    public class DetailVM
    {
        public DetailVM()
        {
            Product = new Product();
        }
        public Product Product { get; set; }
        public bool ExistInCart { get; set; }
    }
}
