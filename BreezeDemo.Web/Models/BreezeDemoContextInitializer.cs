using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BreezeDemo.Web.Models
{
    public class BreezeDemoContextInitializer : DropCreateDatabaseAlways<BreezeDemoContext>
    {
        protected override void Seed(BreezeDemoContext context)
        {
            var products = new List<Product>
            {
                new Product() {Name = "Name1", Price = 11.5m},
                new Product() {Name = "Name2", Price = 12.5m},
                new Product() {Name = "Name3", Price = 13.5m},
                new Product() {Name = "Name4", Price = 14.5m},
                new Product() {Name = "Name5", Price = 15.5m},
                new Product() {Name = "Name6", Price = 16.5m},
                new Product() {Name = "Name7", Price = 17.5m}
            };

            products.ForEach(b => context.Products.Add(b));
            context.SaveChanges();

            var order = new Order()
            {
                Customer = "Customer1",
                OrderDate = new DateTime(2014, 6, 3)
            };
            var orderDetails = new List<OrderDetail>
            {
                new OrderDetail(){ Product = products[0], Order = order, Quantity = 1},
                new OrderDetail(){ Product = products[1], Order = order, Quantity = 2},
                new OrderDetail(){ Product = products[2], Order = order, Quantity = 3}
            };
            context.Orders.Add(order);
            orderDetails.ForEach(od => context.OrderDetails.Add(od));
            context.SaveChanges();

            order = new Order()
            {
                Customer = "Customer2",
                OrderDate = new DateTime(2014, 6, 4)
            };
            orderDetails = new List<OrderDetail>
            {
                new OrderDetail(){ Product = products[1], Order = order, Quantity = 1},
                new OrderDetail(){ Product = products[1], Order = order, Quantity = 1},
                new OrderDetail(){ Product = products[4], Order = order, Quantity = 12},
                new OrderDetail(){ Product = products[5], Order = order, Quantity = 6}
            };
            context.Orders.Add(order);
            orderDetails.ForEach(od => context.OrderDetails.Add(od));
            context.SaveChanges();

            order = new Order()
            {
                Customer = "Customer3",
                OrderDate = new DateTime(2014, 6, 5)
            };
            orderDetails = new List<OrderDetail>
            {
                new OrderDetail(){ Product = products[2], Order = order, Quantity = 1},
                new OrderDetail(){ Product = products[3], Order = order, Quantity = 2},
                new OrderDetail(){ Product = products[6], Order = order, Quantity = 10}
            };
            context.Orders.Add(order);
            orderDetails.ForEach(od => context.OrderDetails.Add(od));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}