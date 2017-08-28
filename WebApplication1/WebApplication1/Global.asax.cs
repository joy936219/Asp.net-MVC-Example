using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AuthenticateRequest(Object sender,EventArgs e)
        {
            bool User = HttpContext.Current.User != null;
            bool Auth = User && HttpContext.Current.User.Identity.IsAuthenticated;
            bool isIdentity = Auth && HttpContext.Current.User.Identity is System.Web.Security.FormsIdentity;

            if (isIdentity)
            {
                System.Web.Security.FormsIdentity id = (System.Web.Security.FormsIdentity)HttpContext.Current.User.Identity;

                System.Web.Security.FormsAuthenticationTicket ticket = id.Ticket;

                string[] role = new string[1] ;
                role[0] = ticket.UserData;

                HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(id, role);
            }
        }
    }
}
