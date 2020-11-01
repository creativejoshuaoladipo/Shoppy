using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shoppy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppy.Data
{
    public class ShoppyDbContext : IdentityDbContext
    {
        public ShoppyDbContext(DbContextOptions<ShoppyDbContext> options): base(options)
        {

        }
       public DbSet<Category> Category { get; set; }
       public DbSet<ApplicationType> ApplicationType { get; set; }
        public DbSet<Product> Product { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }


    }
}
