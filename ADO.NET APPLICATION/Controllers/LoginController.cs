using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ADO.NET_APPLICATION.Controllers
{
    public class LoginController : Controller
    {

        // GET: Login
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DataAccessContext"].ConnectionString;
                DataAccess.Models.User.Insert(user);
                return RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("validation-error", ex.Message);
                return View();
            }

        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            try
            {
                user.USER_NAME = user.USER_NAME.ToLower();
                user.PASSWORD = user.PASSWORD.ToLower();
                if (!DataAccess.Models.User.IsAuthenticated(user)) 
                {
                    ModelState.AddModelError("validation-error", "user name or password is wrong.");
                    return View();
                }
                FormsAuthentication.SetAuthCookie(user.USER_NAME.ToLower() + "|" + user.PASSWORD.ToLower(), false);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("validation-error", ex.Message);
            }
            return RedirectToAction("Index","Employees");
        }
        public ActionResult Logout() 
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }
    }
}