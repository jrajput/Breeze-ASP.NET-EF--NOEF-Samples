using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreezeDemo.Web.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public int OrderId { get; set; }

        //Navigation
        public virtual Product Product { get; set; }

        public virtual Order Order { get; set; }
    }
}
