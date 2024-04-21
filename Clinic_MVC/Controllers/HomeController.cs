using Clinic_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinic_MVC.Controllers
{
    public class HomeController : Controller
    {
        public static bool isHome = false;
        
        public static bool isUser = false;
        Clinic_DBEntities db = new Clinic_DBEntities();
        // GET: Home
        public ActionResult Index()
        {
            isHome= true;
            return View();
        }
        public ActionResult Success()
        {
            
            return View();
        }

        public ActionResult SignUp()
        {
            isHome = true;
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(TBLUserInfo tBLUserInfo)
        {
            if (db.TBLUserInfoes.Any(x => x.UsernameUs == tBLUserInfo.UsernameUs))
            {
                ViewBag.Notification = "This account already exists";
                return View();
            }

            if (ModelState.IsValid)
            {
                db.TBLUserInfoes.Add(tBLUserInfo);
                db.SaveChanges();


                Session["Id"] = tBLUserInfo.IdUs.ToString();
                Session["Username"] = tBLUserInfo.UsernameUs.ToString();
                return RedirectToAction("Login", "Home");

            }
            return View(tBLUserInfo);
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Abandon();
            
            isUser = false;

            return RedirectToAction("Index", "Home");
        }
        public ActionResult Doctors()
        {
         isUser = true;

            return View();
        }
        


        [HttpGet]
        public ActionResult Login()
        {
            isHome=false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TBLUserInfo tBLUserInfo)
        {
            var checkLogin = db.TBLUserInfoes.Where(x => x.UsernameUs.Equals(tBLUserInfo.UsernameUs) && x.Password.Equals(tBLUserInfo.Password)).FirstOrDefault();
            if (checkLogin != null)
            {
                Session["Id"] = tBLUserInfo.IdUs.ToString();
                Session["Username"] = tBLUserInfo.UsernameUs.ToString();
                /*eturn RedirectToAction("Doctors", "Home");*/
                if (tBLUserInfo.UsernameUs == "Admin" && tBLUserInfo.Password == "Admin@1234")
                {
                   
                    ViewBag.msg = "Not user";
                }
                else
                {
                    
                    isUser = true;
                    return RedirectToAction("Doctor_list", "Doctors");
                }
            }

            else
            {
                ViewBag.Notification = "Wrong Username or Password";
            }

            return View();

        }
    }
}
