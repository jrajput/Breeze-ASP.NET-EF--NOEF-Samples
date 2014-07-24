using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Breeze.ContextProvider;
using Newtonsoft.Json.Linq;

namespace BreezeDemoNoEF.Web.Models
{
    /// <summary>In-memory "database" and "data access" layer.</summary>
    /// <remarks>
    /// When web app starts, fills in-mem "database" with sample data
    /// Client changes are accumulated.
    /// When app shuts down, all data are lost.
    /// </remarks>
    public class BreezeDemoContext
    {
        static BreezeDemoContext() { }

        // No one can instantiate
        private BreezeDemoContext() { }

        // Singleton instance of this in-memory "database"
        public static BreezeDemoContext Instance
        {
            get
            {
                if (!__instance._initialized)
                {
                    __instance.PopulateWithSampleData();
                    __instance._initialized = true;
                }
                return __instance;
            }
        }
        public List<Order> Orders
        {
            get
            {
                lock (__lock)
                {
                    return _orders;
                }
            }
        }

        public List<OrderDetail> OrderDetails
        {
            get
            {
                lock (__lock)
                {
                    return _orderDetails;
                }
            }
        }

        public List<Product> Products
        {
            get
            {
                lock (__lock)
                {
                    return _products;
                }
            }
        }

        public void SaveChanges(SaveWorkState saveWorkState)
        {
            lock (__lock)
            {
                _keyMappings.Clear();
                var saveMap = saveWorkState.SaveMap;
                SaveOrders(saveMap);
                // ToList effectively copies the _keyMappings so that an incoming SaveChanges call doesn't clear the 
                // keyMappings before the previous version has completed serializing. 
                saveWorkState.KeyMappings = _keyMappings.ToList();
            }
        }

        #region Private methods

        private void SaveOrders(Dictionary<Type, List<EntityInfo>> saveMap)
        {
            List<EntityInfo> infos;
            if (!saveMap.TryGetValue(typeof(Order), out infos))
            {
                return;
            }
            foreach (var ei in infos)
            {
                var order = (Order)ei.Entity;
                if (ei.EntityState == EntityState.Added)
                {
                    AddOrder(order);
                }
                else if (ei.EntityState == EntityState.Modified)
                {
                    ModifyOrder(order);
                }
                else if (ei.EntityState == EntityState.Deleted)
                {
                    DeletOrder(order);
                }
            }
        }


        private void ModifyOrder(Order item)
        {
            Order order = FindOrder(item.Id);
            order.Customer = item.Customer;
            order.OrderDate = item.OrderDate;
        }

        private void AddOrder(Order item)
        {
            if (item.Id <= 0)
            {
                item.Id = AddMapping(typeof(Order), item.Id);
            }
            Orders.Add(item);
        }

        private void DeletOrder(Order item)
        {
            Orders.Remove(Orders.FirstOrDefault(o=>o.Id==item.Id));
        }

        private Order FindOrder(int orderId, bool okToFail = false)
        {
            var order = Orders.FirstOrDefault(o => orderId == o.Id);
            if (order == null)
            {
                if (okToFail) return null;
                throw new Exception("Can't find order " + orderId);
            }
            return order;
        }

        private int AddMapping(Type type, int tempId)
        {
            var newId = IdGenerator.Instance.GetNextId(type);
            _keyMappings.Add(new KeyMapping
            {
                EntityTypeName = type.FullName,
                RealValue = newId,
                TempValue = tempId
            });
            return newId;
        }

        public void PopulateWithSampleData()
        {
            FillProducts();
            FillOrderDetails();
            FillOrders();
        }

        private void FillProducts()
        {

            Products.Add(new Product() { Id = AddMapping(typeof(Product), 1), Name = "Name1", Price = 11.5m });
            Products.Add(new Product() { Id = AddMapping(typeof(Product), 2), Name = "Name2", Price = 12.5m });
            Products.Add(new Product() { Id = AddMapping(typeof(Product), 3), Name = "Name3", Price = 13.5m });
            Products.Add(new Product() { Id = AddMapping(typeof(Product), 4), Name = "Name4", Price = 14.5m });
            Products.Add(new Product() { Id = AddMapping(typeof(Product), 5), Name = "Name5", Price = 15.5m });
            Products.Add(new Product() { Id = AddMapping(typeof(Product), 6), Name = "Name6", Price = 16.5m });
            Products.Add(new Product() { Id = AddMapping(typeof(Product), 7), Name = "Name7", Price = 17.5m });
        }

        private void FillOrders()
        {
            var order = new Order()
            {
                Id = AddMapping(typeof(Order), 1),
                Customer = "Jane",
                OrderDate = new DateTime(2014, 6, 3)
            };
            order.OrderDetails = new List<OrderDetail>
            {
                OrderDetails[0],
                OrderDetails[1],
                OrderDetails[2]
            };
            Orders.Add(order);

            order = new Order()
            {
                Id = AddMapping(typeof(Order), 2),
                Customer = "John",
                OrderDate = new DateTime(2014, 6, 4)
            };
            order.OrderDetails = new List<OrderDetail>
            {
                OrderDetails[3],
                OrderDetails[4],
                OrderDetails[5],
                OrderDetails[6]
            };
            Orders.Add(order);

            order = new Order()
            {
                Id = AddMapping(typeof(Order), 3),
                Customer = "Adam",
                OrderDate = new DateTime(2014, 6, 5)
            };
            order.OrderDetails = new List<OrderDetail>
            {
                OrderDetails[7],
                OrderDetails[8],
                OrderDetails[9]
            };
            Orders.Add(order);
            
        }

        private void FillOrderDetails()
        {
            OrderDetails.Add(new OrderDetail() { Id = AddMapping(typeof(OrderDetail), 1), OrderId = 1, Quantity = 1 });
            OrderDetails.Add(new OrderDetail() { Id = AddMapping(typeof(OrderDetail), 2), OrderId = 1, Quantity = 2 });
            OrderDetails.Add(new OrderDetail() { Id = AddMapping(typeof(OrderDetail), 3), OrderId = 1, Quantity = 3 });
            OrderDetails.Add(new OrderDetail() { Id = AddMapping(typeof(OrderDetail), 4), OrderId = 2, Quantity = 1 });
            OrderDetails.Add(new OrderDetail() { Id = AddMapping(typeof(OrderDetail), 5), OrderId = 2, Quantity = 1 });
            OrderDetails.Add(new OrderDetail() { Id = AddMapping(typeof(OrderDetail), 6), OrderId = 2, Quantity = 12 });
            OrderDetails.Add(new OrderDetail() { Id = AddMapping(typeof(OrderDetail), 7), OrderId = 2, Quantity = 6 });
            OrderDetails.Add(new OrderDetail() { Id = AddMapping(typeof(OrderDetail), 8), OrderId = 3, Quantity = 1 });
            OrderDetails.Add(new OrderDetail() { Id = AddMapping(typeof(OrderDetail), 9), OrderId = 3, Quantity = 2 });
            OrderDetails.Add(new OrderDetail() { Id = AddMapping(typeof(OrderDetail), 10), OrderId = 3, Quantity = 10 });
        }

        #endregion

        private static readonly Object __lock = new Object();
        private static readonly BreezeDemoContext __instance = new BreezeDemoContext();

        private bool _initialized;
        private readonly List<Order> _orders = new List<Order>();
        private readonly List<Product> _products = new List<Product>();
        private readonly List<OrderDetail> _orderDetails = new List<OrderDetail>();
        private readonly List<KeyMapping> _keyMappings = new List<KeyMapping>();
    }

    public class ValidationError : Exception
    {
        public ValidationError(string message) : base(message) { }
    }

    public sealed class IdGenerator
    {
        // DO NOT REMOVE explicit static constructor which
        // tells C# compiler not to mark type as 'beforefieldinit'
        static IdGenerator() { }

        private IdGenerator() { } // only this class can instantiate

        public static IdGenerator Instance
        {
            get { return _instance; }
        }

        public int GetNextId(Type type)
        {
            lock (_idMap)
            {
                int val;
                if (!_idMap.TryGetValue(type, out val))
                {
                    val = 1;
                }
                _idMap[type] = val + 1;
                return val;
            }
        }

        private static readonly IdGenerator _instance = new IdGenerator();
        private readonly Dictionary<Type, int> _idMap = new Dictionary<Type, int>();
    }
}