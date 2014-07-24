using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreezeDemoNoEF.Web.Models
{
    public class Order
    {
        public Order()
        {
            //OrderDetails = new List<OrderDetail>();
        }

        public int Id { get; set; }

        public string Customer { get; set; }

        public DateTime OrderDate { get; set; }

        //Navigation
        public virtual IEnumerable<OrderDetail> OrderDetails { get; set; } 
    }
}
