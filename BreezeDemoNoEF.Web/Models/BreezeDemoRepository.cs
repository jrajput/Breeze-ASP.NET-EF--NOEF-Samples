using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Breeze.ContextProvider;

namespace BreezeDemoNoEF.Web.Models
{
    public class BreezeDemoRepository : ContextProvider
    {
        private BreezeDemoContext _contextProvider { get { return BreezeDemoContext.Instance; } }

        public IQueryable<Product> Products()
        {
            return _contextProvider.Products.AsQueryable();
        }

        public IQueryable<Order> Orders()
        {
            return _contextProvider.Orders.AsQueryable();
        }

        public IQueryable<OrderDetail> OrderDetails()
        {
            return _contextProvider.OrderDetails.AsQueryable();
        }

        protected override string BuildJsonMetadata()
        {
            return null;
        }

        protected override void CloseDbConnection()
        {

        }

        public override System.Data.IDbConnection GetDbConnection()
        {
            return null;
        }

        protected override void OpenDbConnection()
        {

        }

        protected override void SaveChangesCore(SaveWorkState saveWorkState)
        {
            _contextProvider.SaveChanges(saveWorkState);
        }
    }
}