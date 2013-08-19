using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CloudWars.Web.Code
{
    public class Context
    {

        public static string ConnectionId 
        {
            get 
            {
                return HttpContext.Current.User.Identity.IsAuthenticated ? HttpContext.Current.User.Identity.Name : Guid.NewGuid().ToString() ;
            }
        }
    }
}