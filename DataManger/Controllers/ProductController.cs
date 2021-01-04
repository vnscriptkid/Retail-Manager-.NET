using DataManager.Library.DataAccess;
using DataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DataManger.Controllers
{
    [Authorize]
    public class ProductController : ApiController
    {

        public List<ProductModel> GetAll()
        {
            ProductData productData = new ProductData();
            return productData.GetAll();
        } 
    }
}