using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace WebApplication1
{
    public class CustomAuthAttribute:AuthorizeAttribute
    {
        private string _authstr;
       public  CustomAuthAttribute(string attrib)
       {
           _authstr = attrib;
       }

       protected override bool AuthorizeCore(HttpContextBase httpContext)
       {
           //return base.AuthorizeCore(httpContext);
           if (httpContext.Request.IsLocal){
               return HttpContext.Current.User.IsInRole(_authstr);
           }
           else
           {
               return true;
           }
               
       }
       protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
       {
           //base.HandleUnauthorizedRequest(filterContext);
           filterContext.Result = new RedirectResult("/Home1/Error");
       }
       
    }
}