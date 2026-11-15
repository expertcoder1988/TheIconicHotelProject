using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MountFeastMVC.Models;

namespace HotelWebSite.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        HotelDBEntities HDE = new HotelDBEntities();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddNewUser()
        {
            return View();
        }

        public ActionResult UserEdit(int id)
        {
            return RedirectToAction("EditUser", "User", new { @id = id });
        }

        public ActionResult EditUser(int id)
        {
            TempData["UserId"] = id;
            return View();
        }

        public JsonResult LoadUsers(int propertyID)
        {
            var res = from u in HDE.Users                     
                      where (u.propertyID == propertyID && u.tiStatus == 1)
                      select new
                      {
                          UserId = u.intUserID,
                          FirstName = u.vFirstName,
                          LastName = u.vLastName,
                          UserName = u.vUserName,
                          Email = u.vEmail,
                          MobileNo = u.vMobileNumber,
                          DateCreated = u.dtmDateCreated
                      };

            return Json(res,
                JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult LoadUserData(int id)
        {
            var res = from u in HDE.Users
                      where (u.intUserID == id && u.tiStatus == 1)
                      select new
                      {
                          UserId = u.intUserID,
                          FirstName = u.vFirstName,
                          LastName = u.vLastName,
                          UserName = u.vUserName,
                          Email = u.vEmail,
                          MobileNo = u.vMobileNumber,
                          DateCreated = u.dtmDateCreated,
                          Access = u.AccessCodes,
                          PropertyId = u.propertyID
                      };

            return Json(res,
                JsonRequestBehavior.AllowGet); ;
        }

        [HttpPost]
        public string DeleteUser(int userID)
        {
            try
            {

                using (var context = new HotelDBEntities())
                {
                    // Retrieve entity by id
                    var entity = context.Users.FirstOrDefault(item => item.intUserID == userID);

                    // Validate entity is not null
                    if (entity != null)
                    {
                        entity.tiStatus = 0;

                        // Save changes in database
                        context.SaveChanges();
                    }
                }

                return "Property updated succesfully";
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        //public JsonResult LoadAccessRights()
        //{
        //    var res = from u in HDE.UserAccessCodes                      
        //              select new
        //              {
        //                  Id = u.Id,
        //                  Name = u.Name,
        //                 Description = u.Description
        //              };

        //    return Json(res, JsonRequestBehavior.AllowGet); ;
        //}

        [HttpPost]
        public string AddUser(User po) //(FormCollection frm)
        {
            try
            {
                string password = Accessories.Encrypt(po.vPassword);

                po.vPassword = password;
                po.dtmDateCreated = DateTime.Now;
                po.tiStatus = 1;

                HDE.Entry(po).State = System.Data.EntityState.Added;
                HDE.SaveChanges();

                return "Booking done succesfully";
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        [HttpPost]
        public string UpdateUser(User po) //(FormCollection frm)
        {
            try
            {
               // string password = Accessories.Encrypt(po.vPassword);

               // po.vPassword = password;
             //   po.dtmDateCreated = DateTime.Now;
                //po.tiStatus = 1;

                //HDE.Entry(po).Property(x => x.vPassword).IsModified = false;

                //HDE.Entry(po).State = System.Data.Entity.EntityState.Modified;
                //HDE.SaveChanges();

                using (var db = HDE)
                {
                    db.Users.Attach(po);
                    var entry = db.Entry(po);
                    entry.Property(x => x.AccessCodes).IsModified = true; //Exception thrown
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return e.Message.ToString();
                    }
                    return "Booking done succesfully";
                }

                //return "Booking done succesfully";
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        [HttpPost]
        public string ResetPassword(User po) //(FormCollection frm)
        {
            try
            {
                using (var db = HDE)
                {
                    db.Users.Attach(po);
                    var entry = db.Entry(po);
                    entry.Property(x => x.vPassword).IsModified = true; //Exception thrown
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return e.Message.ToString();
                    }
                    return "Booking done succesfully";
                }

               // return "Booking done succesfully";
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }
    }
}