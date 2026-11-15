using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MountFeastMVC.Models;

namespace MountFeastMVC.Controllers.Policies
{
    public class PolicyController : Controller
    {
        HotelDBEntities HDE = new HotelDBEntities();

        // GET: Policy
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RatePlan()
        {
            return View();
        }

        public JsonResult Policies(int propertyID)
        {
            var res = from obj in HDE.CancellationPolicies
                      where (obj.PropertyID == propertyID && obj.Status == true)
                      select new
                      {
                          Id = obj.Id,
                          Name = obj.Name,
                          PeriodForFreeCharge = obj.PeriodForFreeCharge,
                          HowlongFreeOfCharge = obj.HowlongFreeOfCharge,
                          GuestChargedForCancellation = obj.GuestChargedForCancellation,
                          ChargedForNoShow = obj.ChargedForNoShow,
                          ChargeGuestsBeforeTheyArrive = obj.ChargeGuestsBeforeTheyArrive,
                          HowToCollectPayment = obj.HowToCollectPayment,
                          HowToReceiveThePrepayment = obj.HowToReceiveThePrepayment,
                          HowLongWillRefundTake = obj.HowLongWillRefundTake
                      };

            return Json(res,
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult RatePlans(int propertyID)
        {
            var res = from obj in HDE.RatePlans
                      where (obj.PropertyID == propertyID && obj.Status == true)
                      select new
                      {
                          Id = obj.Id,
                          Name = obj.Name,
                          WantStayMinimumLength = obj.WantStayMinimumLength,
                          StayMinimumLength = obj.StayMinimumLength,
                          CancellationPolicy = obj.CancellationPolicy,
                          WantToIncludeMeals = obj.WantToIncludeMeals,
                          Breakfast = obj.Breakfast,
                          Lunch = obj.Lunch,
                          Dinner = obj.Dinner,
                          DaysBeforeCheckIn = obj.DaysBeforeCheckIn,
                          NumberOfDaysBeforeCheckIn = obj.NumberOfDaysBeforeCheckIn,
                          Discount = obj.Discount,
                          RoomTypes = obj.RoomTypes,
                          EntryTime = obj.EntryTime,
                          UserId = obj.UserId
                      };
            return Json(res,
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadRooms(int propertyID)
        {
            return Json(from obj in HDE.Rooms
                        where (obj.propertyID == propertyID)
                        select new
                        {
                            ProductID = obj.ID,
                            RoomCode = obj.RoomCode,
                            RoomDescription = obj.RoomDescription
                        },
                JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult loadRatePlan(int ratePlanID)
        {
            return Json(from obj in HDE.RatePlans
                        where (obj.Id == ratePlanID)
                        select new
                        {
                            Id = obj.Id,
                            Name = obj.Name,
                            WantStayMinimumLength = obj.WantStayMinimumLength,
                            StayMinimumLength = obj.StayMinimumLength,
                            CancellationPolicy = obj.CancellationPolicy,
                            WantToIncludeMeals = obj.WantToIncludeMeals,
                            Breakfast = obj.Breakfast,
                            Lunch = obj.Lunch,
                            Dinner = obj.Dinner,
                            DaysBeforeCheckIn = obj.DaysBeforeCheckIn,
                            NumberOfDaysBeforeCheckIn = obj.NumberOfDaysBeforeCheckIn,
                            Discount = obj.Discount,
                            RoomTypes = obj.RoomTypes,
                            EntryTime = obj.EntryTime,
                            UserId = obj.UserId
                        },
                JsonRequestBehavior.AllowGet);
        }


        public JsonResult loadPolicy(int policyId)
        {
            return Json(from obj in HDE.CancellationPolicies
                        where (obj.Id == policyId)
                        select new
                        {
                            Id = obj.Id,
                            Name = obj.Name,
                            PeriodForFreeCharge = obj.PeriodForFreeCharge,
                            HowlongFreeOfCharge = obj.HowlongFreeOfCharge,
                            GuestChargedForCancellation = obj.GuestChargedForCancellation,
                            ChargedForNoShow = obj.ChargedForNoShow,
                            ChargeGuestsBeforeTheyArrive = obj.ChargeGuestsBeforeTheyArrive,
                            HowToCollectPayment = obj.HowToCollectPayment,
                            HowToReceiveThePrepayment = obj.HowToReceiveThePrepayment,
                            HowLongWillRefundTake = obj.HowLongWillRefundTake,
                            Type = obj.Type
                        },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string AddPolicy(CancellationPolicy po) //(FormCollection frm)
        {
            try
            {
                if (po.Id == 0)
                {
                    po.CreatedDate = DateTime.Now;
                    po.UserId = Convert.ToInt32(Session["CustID"]);
                    po.Type = true;
                    po.Status = true;


                    HDE.Entry(po).State = System.Data.EntityState.Added;
                    HDE.SaveChanges();

                    return "New Policy added succesfully";
                }
                else
                {
                    po.CreatedDate = DateTime.Now;
                    po.UserId = Convert.ToInt32(Session["CustID"]);
                    po.Type = true;
                    po.Status = true;

                    HDE.Entry(po).State = System.Data.EntityState.Modified;
                    HDE.SaveChanges();

                    return "New Policy added succesfully";
                }
               
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        [HttpPost]
        public string DeletePolicy(int policyId)
        {
            try
            {

                using (var context = new HotelDBEntities())
                {
                    // Retrieve entity by id
                    var entity = context.CancellationPolicies.FirstOrDefault(item => item.Id == policyId);

                    // Validate entity is not null
                    if (entity != null)
                    {
                        entity.Status = false;
                        entity.CreatedDate = DateTime.Today;

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


        [HttpPost]
        public string AddNonRefundablePolicy(CancellationPolicy po) //(FormCollection frm)
        {
            try
            {
                if (!HDE.CancellationPolicies.Any(u => u.Name == "Non-Refundable" && u.PropertyID == po.PropertyID))
                {
                    po.CreatedDate = DateTime.Now;
                    po.UserId = Convert.ToInt32(Session["CustID"]);
                    po.Type = false;
                    po.Status = true;

                    HDE.Entry(po).State = System.Data.EntityState.Added;
                    HDE.SaveChanges();
                }
                else
                {
                   
                }
                return "New Policy added succesfully";
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        [HttpPost]
        public string AddRatePlan(RatePlan po) //(FormCollection frm)
        {
            try
            {
                if (po.Id == 0)
                {
                    po.EntryTime = DateTime.Now;
                    po.UserId = Convert.ToInt32(Session["CustID"]);
                    po.Status = true;

                    HDE.Entry(po).State = System.Data.EntityState.Added;
                    HDE.SaveChanges();

                    return "New Rate Plan Added Succesfully";
                }
                else
                {
                    po.EntryTime = DateTime.Now;
                    po.UserId = Convert.ToInt32(Session["CustID"]);
                    po.Status = true;

                    HDE.Entry(po).State = System.Data.EntityState.Modified;
                    HDE.SaveChanges();

                    return "Rate Plan updated Succesfully";
                }
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }
    }
}