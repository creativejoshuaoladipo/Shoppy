using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Shoppy.Models
{
    public class ApplicationUser : IdentityUser
    {
        public String FullName { get; set; }
    }
}
