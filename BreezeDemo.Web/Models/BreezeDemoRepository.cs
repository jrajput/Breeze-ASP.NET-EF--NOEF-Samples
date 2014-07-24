using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;

namespace BreezeDemo.Web.Models
{
    public class BreezeDemoRepository : IBreezeDemoRepository
    {
        private readonly EFContextProvider<BreezeDemoContext> _contextProvider =
           new EFContextProvider<BreezeDemoContext>();

        public string Metadata
        {
            get { return _contextProvider.Metadata(); }
        }

        public SaveResult SaveChanges(Newtonsoft.Json.Linq.JObject saveBundle)
        {
            return _contextProvider.SaveChanges(saveBundle);
        }

        public IQueryable<Product> Products()
        {
            return _contextProvider.Context.Products;
        }

        public IQueryable<Order> Orders()
        {
            return _contextProvider.Context.Orders;
        }
    }
}