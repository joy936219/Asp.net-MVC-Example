using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebApplication1.Models;
namespace WebApplication1
{
    public class WebSiteHelper
    {
        public static string CurrentUserName
        {

            get
            {

                var httpContext = HttpContext.Current;

                var identity = httpContext.User.Identity as FormsIdentity;



                if (identity == null)
                {

                    return string.Empty;

                }

                else
                {

                    var userID = identity.Name;

                    return SystemUserName(userID);

                }

            }

        }

        public static string CurrentUserRole
        {
            get
            {
                var httpContext = HttpContext.Current;
                var identity = httpContext.User.Identity as FormsIdentity;

                if (identity == null)
                {
                    return string.Empty;
                }
                else
                {
                    var userID = identity.Name;
                    Models.NorthwindEntities db = new NorthwindEntities();

                    var userrole = (from a in db.Account where a.UserId == identity.Name select a).FirstOrDefault();
                    return SystemUserName(userrole.Role);
                }
            }
        }

        /// <summary>

        /// Systems the name of the user.

        /// </summary>

        /// <param name="id">The identifier.</param>

        /// <returns></returns>

        public static string SystemUserName(Object id)
        {

            string userName = string.Empty;



            string systemUserID = id.ToString();



            

            if (systemUserID.Equals(Guid.Empty))
            {

                userName = "系統預設";

            }
            else
            {

                using (NorthwindEntities db = new NorthwindEntities())
                {

                    //var user1 = db.SystemUsers.FirstOrDefault(x => x.ID == systemUserID);
                     var user = db.Account.FirstOrDefault(x => x.UserId==  systemUserID.ToString());

                    userName = (user == null) ? string.Empty : user.UserId;

                }

            }

            return userName;

        }
    }
}