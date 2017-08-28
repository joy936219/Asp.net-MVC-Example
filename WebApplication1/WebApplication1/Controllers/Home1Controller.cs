using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using System.Data.Entity;
using System.Data;
namespace WebApplication1.Controllers
{
    
    public class Home1Controller : Controller
    {
        // GET: Home1
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]//任何使用者皆可進入
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)//若己經驗證直接導引首頁
            {
                return RedirectToAction("index", "Home1");
            }
            else
            {
                return View();
            }
            
        }
        [HttpPost]//只能透過Post進入
        [ValidateAntiForgeryToken] //防止CSRF
        public ActionResult Login(Models.Account account)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Models.NorthwindEntities db = new Models.NorthwindEntities();

            var test = (from a in db.Account 
                        where a.UserId == account.UserId && a.Password == account.Password 
                        select a).FirstOrDefault();
            
            if (test != null)
            {
                LoginProcess(test);
                return RedirectToAction("index", "Home1");
            }
            else
            {
                ModelState.AddModelError("", "Failed");
                return View();
            }
            
        }
        [CustomAuth("admin")]//身份為admin才可進入
        public ActionResult Edit(string id)
        {
            Models.Orders data;
            if (String.IsNullOrEmpty(id))
            {
                //data = null;
                return View();
            }
            else
            {
                Models.NorthwindEntities db = new Models.NorthwindEntities();
                data = (from o in db.Orders where o.OrderID.IndexOf(id.ToString()) >= 0 select o).FirstOrDefault();
                return View(data);
            }
            
        }
        [HttpPost]
        [Authorize]
        public ActionResult Edit(Models.Orders order)
        {
            
            Models.NorthwindEntities db = new Models.NorthwindEntities();
            //var _order = db.Orders.First(o => o.OrderID == order.OrderID);
            db.Set<Models.Orders>().AsNoTracking().FirstOrDefault(o => o.OrderID == order.OrderID);
            
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return View();
        }
        private void LoginProcess(Models.Account acc)
        {
            var ticket = new FormsAuthenticationTicket(
                version:1,
                name: acc.UserId,
                issueDate:DateTime.Now ,
                expiration: DateTime.Now.AddMinutes(30),
                isPersistent: false,
                userData: acc.Role,
                cookiePath: FormsAuthentication.FormsCookiePath
                );

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie); 
            
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            //清除所有的 session
            Session.RemoveAll();
            //建立一個同名的 Cookie 來覆蓋原本的 Cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            //建立 ASP.NET 的 Session Cookie 同樣是為了覆蓋
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);
            return RedirectToAction("Login", "Home1");
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}