using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BreezeDemo.Web.Models
{
    public class BreezeDemoContext : DbContext
    {
        public BreezeDemoContext()
            : base("name=BreezeDemoContext")
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}