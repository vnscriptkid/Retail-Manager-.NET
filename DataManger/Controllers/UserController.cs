using DataManager.Library.DataAccess;
using DataManager.Library.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DataManger.Controllers
{
    [Authorize]
    [RoutePrefix("api/Users")]
    public class UserController : ApiController
    {
        [Route("current")]
        public UserModel GetUserById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();

            return data.GetUserById(userId).FirstOrDefault();
        }
    }
}