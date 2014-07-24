using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Breeze.ContextProvider;
using Newtonsoft.Json.Linq;

namespace BreezeDemo.Web.Models
{
    public interface IBreezeDemoRepository
    {
        string Metadata { get; }

        SaveResult SaveChanges(JObject saveBundle);

        IQueryable<Product> Products();

        IQueryable<Order> Orders();
    }
}
