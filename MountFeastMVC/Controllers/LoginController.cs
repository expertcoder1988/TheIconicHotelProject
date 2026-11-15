using MountFeastMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MountFeastMVC.Controllers
{
    public class LoginController : Controller
    {
        HotelDBEntities HDE = new HotelDBEntities();
        EncryptionManager enc = new EncryptionManager();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

            [HttpPost]
        public ActionResult Login(string username, string password, string ReturnUrl)
        {
           //password = Accessories.Decrypt("Z2J0QDEyMw==");
            //  string pas = Accessories.Encrypt("iconic@2024#"); //admin password
            password = Accessories.Encrypt(password);
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var user = (from u in HDE.Users
                            where u.vUserName == username && u.vPassword == password && u.tiStatus == 1
                            select new
                            {
                                UserName = u.vUserName,
                                Password = password,
                                FullName = u.vFirstName  + " " + u.vLastName,
                                Status = u.tiStatus,
                                UserID = u.intUserID,
                                ProfilePic = u.Image,
                                AccessType = u.tiAccessType
                            }).FirstOrDefault();
                if (user != null && !string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password))
                {
                    if (user.Status == 1)
                    {
                        FormsAuthentication.SetAuthCookie(user.UserName, false);
                        var access = (from u in HDE.Users
                                      where u.vUserName == username
                                      select new
                                      {
                                          Access = u.AccessCodes
                                      }).FirstOrDefault();

                        if (access.Access == null)
                        {
                            Session["Access"] = "0,";
                        }
                        else
                        {
                            Session["Access"] = access.Access;
                        }

                        Session["UserInfo"] = user;
                        Session["UserInfo"] = user;
                        Session["UserID"] = user.UserID;
                        //  string TypeID = enc.Decrypt(ConfigurationManager.AppSettings["usernm"]).ToString();

                        return RedirectToAction("Index", "Dashboard");  
                    }
                    else if (user.Status == 0)
                    {
                        ViewBag.Warning = "Your request is pending. Please wait.";
                        return RedirectToAction("Index", "Login");

                    }
                    else if (user.Status == 2)
                    {
                        TempData["Error"] = "Your credentials has been disabled.";
                        return RedirectToAction("Index", "Login");

                    }
                    else if (user.Status == 3)
                    {
                        TempData["Error"] = "An activation link has been sent to your email address.";
                        return RedirectToAction("Index", "Login");

                    }
                    else
                    {
                        TempData["Error"] = "Your account has been disabled.";
                        return RedirectToAction("Index", "Login");
                    }
                }
                else
                {
                    TempData["Error"] = "Invalid user Id or password.";
                }
            }
            else
            {
                TempData["Error"] = "Please provide user Id and password.";
            }
            return RedirectToAction("Login", "Login", new { errorMessage = TempData["Error"].ToString() });
        }
    }
}