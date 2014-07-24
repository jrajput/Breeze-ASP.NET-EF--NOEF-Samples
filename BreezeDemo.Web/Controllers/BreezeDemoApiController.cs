using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Breeze.ContextProvider;
using Breeze.WebApi2;
using BreezeDemo.Web.Models;
using Newtonsoft.Json.Linq;

namespace BreezeDemo.Web.Controllers
{
    [BreezeController]
    public class BreezeDemoApiController : ApiController
    {
        private readonly IBreezeDemoRepository _rep;

        //public BreezeDemoApiController(IBreezeDemoRepository repo)
        //{
        //    this._rep = repo;
        //}

        public BreezeDemoApiController()
        {
            this._rep = new BreezeDemoRepository();
        }

        [HttpGet]
        public string Metadata()
        {
            return _rep.Metadata;
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _rep.SaveChanges(saveBundle);
        }

        [HttpGet]
        public IQueryable<Product> Products()
        {
            return _rep.Products();
        }

        [HttpGet]
        public IQueryable<Order> Orders()
        {
            return _rep.Orders();
        }
    }
}
