using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MountFeastMVC.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MountFeastMVC.Controllers.Bookings
{
    [Authorize]
    public class BookingController : Controller
    {
        HotelDBEntities HDE = new HotelDBEntities();
        Booking bi = new Booking();

        // GET: Booking
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CheckIn()
        {
            return View();
        }
        public ActionResult BlockBooking()
        {
            return View();
        }
        public ActionResult Booking()
        {
            return View();
        }
        public ActionResult BoookingPayment()
        {
            return View();
        }
        public JsonResult BookingList(int propertyId, string CheckIn, string CheckOut, string BookingStatus, int Roomcate, int flag = -1)
        {
            try
            {
                // Parse user input dates (optional, only if provided)
                DateTime checkInDate = DateTime.MinValue;
                DateTime checkOutDate = DateTime.MaxValue;

                if (!string.IsNullOrEmpty(CheckIn) && !DateTime.TryParseExact(CheckIn, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out checkInDate))
                {
                    return Json("CheckIn date is not in the correct format (MM/dd/yyyy).", JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrEmpty(CheckOut) && !DateTime.TryParseExact(CheckOut, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out checkOutDate))
                {
                    return Json("CheckOut date is not in the correct format (MM/dd/yyyy).", JsonRequestBehavior.AllowGet);
                }

                // Default query for loading data initially
                var res = (from obj in HDE.Bookings
                           join b in HDE.RoomCategories on obj.RoomCatCode equals b.Id into rooms
                           from r in rooms.DefaultIfEmpty()
                           where obj.HotelCode == propertyId
                                 && obj.BookingStatus != "Blocked"
                           orderby obj.BookingID descending
                           select new
                           {
                               BookingID = obj.BookingID,
                               PaxFName = obj.PaxTitle + " " + obj.PaxFName + " " + obj.PaxLName,
                               PaxPhoneNumber = obj.PaxPhoneNumber,
                               RoomCategory = r.Category,
                               RoomNum = obj.RoomNum,
                               Adults = obj.Adults,
                               Children = obj.Children,
                               CheckIn = obj.CheckIn,
                               CheckOut = obj.CheckOut,
                               Rate = obj.RoomRate,
                               status = obj.BookingStatus,
                               roomCateCode = obj.RoomCatCode,
                               ReservationDate = obj.ReservationDate,
                               NoOfRooms = obj.NoOfRooms,
                               TotalAmount = obj.TotalRoomRate,
                               ConfirmationNo = obj.ConfirmationNo
                           }).ToList();

                // Apply filters only when the `flag` is set
                if (flag != -1)
                {
                    res = (from obj in HDE.Bookings
                           join b in HDE.RoomCategories on obj.RoomCatCode equals b.Id into rooms
                           from r in rooms.DefaultIfEmpty()
                           where obj.HotelCode == propertyId
                                 && obj.CheckIn >= checkInDate
                                 && obj.CheckOut <= checkOutDate
                           orderby obj.BookingID descending
                           select new
                           {
                               BookingID = obj.BookingID,
                               PaxFName = obj.PaxTitle + " " + obj.PaxFName + " " + obj.PaxLName,
                               PaxPhoneNumber = obj.PaxPhoneNumber,
                               RoomCategory = r.Category,
                               RoomNum = obj.RoomNum,
                               Adults = obj.Adults,
                               Children = obj.Children,
                               CheckIn = obj.CheckIn,
                               CheckOut = obj.CheckOut,
                               Rate = obj.RoomRate,
                               status = obj.BookingStatus,
                               roomCateCode = obj.RoomCatCode,
                               ReservationDate = obj.ReservationDate,
                               NoOfRooms = obj.NoOfRooms,
                               TotalAmount = obj.TotalRoomRate,
                               ConfirmationNo = obj.ConfirmationNo
                           }).ToList();
                }

                // Filter by BookingStatus
                if (!string.IsNullOrEmpty(BookingStatus) && BookingStatus != "-1")
                {
                    res = res.Where(r => r.status == BookingStatus).ToList();
                }

                // Filter by Room Category
                if (Roomcate != -1)
                {
                    res = res.Where(r => r.roomCateCode == Roomcate).ToList();
                }

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        //public JsonResult BookingList(int propertyId, DateTime CheckIn, DateTime CheckOut, string BookingStatus, int Roomcate)
        //{
        //    try
        //    {
        //        var res = (from obj in HDE.Bookings
        //                   join b in HDE.RoomCategories
        //                  on obj.RoomCatCode equals b.Id into rooms
        //                   from r in rooms.DefaultIfEmpty()
        //                   where (obj.HotelCode == propertyId && obj.ReservationDate >= CheckIn && obj.ReservationDate <= CheckOut && obj.BookingStatus != "Blocked")
        //                   orderby obj.BookingID descending
        //                   select new
        //                   {
        //                       BookingID = obj.BookingID,
        //                       PaxFName = obj.PaxTitle + " " + obj.PaxFName + " " + obj.PaxLName,
        //                       PaxPhoneNumber = obj.PaxPhoneNumber,
        //                       RoomCategory = r.Category,
        //                       RoomNum = obj.RoomNum,
        //                       Adults = obj.Adults,
        //                       NoOfRooms = obj.NoOfRooms,
        //                       Children = obj.Children,
        //                       CheckIn = obj.CheckIn,
        //                       CheckOut = obj.CheckOut,
        //                       Rate = obj.RoomRate,
        //                       status = obj.BookingStatus,
        //                       roomCateCode = obj.RoomCatCode,
        //                       ReservationDate = obj.DateModified
        //                   }).ToList();

        //        //.Union(from obj in HDE.Bookings
        //        // join b in HDE.RoomCategories
        //        //on obj.RoomCatCode equals b.Id into rooms
        //        // from r in rooms.DefaultIfEmpty()
        //        // where (obj.HotelCode == propertyId && obj.CheckIn >= CheckIn && obj.CheckOut <= CheckOut)
        //        // orderby obj.BookingID
        //        // select new
        //        // {
        //        //     BookingID = obj.BookingID,
        //        //     PaxFName = obj.PaxTitle + " " + obj.PaxFName + " " + obj.PaxLName,
        //        //     PaxPhoneNumber = obj.PaxPhoneNumber,
        //        //     RoomCategory = r.Category,
        //        //     RoomNum = obj.RoomNum,
        //        //     Adults = obj.Adults,
        //        //     Children = obj.Children,
        //        //     CheckIn = obj.CheckIn,
        //        //     CheckOut = obj.CheckOut,
        //        //     Rate = obj.RoomRate,
        //        //     status = obj.BookingStatus,
        //        //     roomCateCode = obj.RoomCatCode
        //        // })).ToList();


        //        if (BookingStatus != "-1")
        //        {
        //            res = res.Where(r => r.status == BookingStatus).ToList();
        //        }
        //        if (Roomcate != -1)
        //        {
        //            res = res.Where(r => r.roomCateCode == Roomcate).ToList();
        //        }
        //        return Json(res, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        //ViewBag.Error = ex.Message;
        //        return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
        //    }
        //}
        public JsonResult LoadRoomNums(int propertyId, DateTime CheckIn, DateTime CheckOut, int Roomcate)
        {
            try
            {
                var list1 = (from a in HDE.RoomNumbers
                             join b in HDE.Rooms
                             on a.RoomCode equals b.RoomCode
                             where (b.RoomCategory == Roomcate)
                             select new
                             {
                                 RoomNums = a.RoomNumber1
                             }).ToList();

                var list2 = (from a in HDE.Bookings
                             where (a.HotelCode == propertyId && a.CheckIn >= CheckIn && a.CheckOut <= CheckOut && a.RoomCatCode == Roomcate)
                             select new
                             {
                                 RoomNums = a.RoomNum
                             }).ToList();

                var res = list1.Except(list2).ToList();

                //if (Roomcate != -1)
                //{
                //    res = res.Where(r => r.roomCateCode == Roomcate);
                //}
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LoadRoomNumsCheckin(int bookingId)
        {
            try
            {
                var paramList = (from a in HDE.Bookings
                                 where (a.BookingID == bookingId)
                                 select new
                                 {
                                     CheckIn = a.CheckIn,
                                     CheckOut = a.CheckOut,
                                     RoomCatCode = a.RoomCatCode,
                                     PropertyId = a.HotelCode
                                 }).FirstOrDefault();

                var list1 = (from a in HDE.RoomNumbers
                             join b in HDE.Rooms
                             on a.RoomCode equals b.RoomCode
                             where (b.RoomCategory == paramList.RoomCatCode)
                             select new
                             {
                                 RoomNums = a.RoomNumber1
                             }).ToList();

                var list2 = (from a in HDE.Bookings
                             where (a.HotelCode == paramList.PropertyId && a.CheckIn >= paramList.CheckIn && a.CheckOut <= paramList.CheckOut &&
                             a.RoomCatCode == paramList.RoomCatCode)
                             select new
                             {
                                 RoomNums = a.RoomNum
                             }).ToList();

                var res = list1.Except(list2).ToList();

                //if (Roomcate != -1)
                //{
                //    res = res.Where(r => r.roomCateCode == Roomcate);
                //}
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LoadRoomRate(int propertyId, DateTime CheckIn, DateTime CheckOut, int Roomcate)
        {
            try
            {
                var dates = new List<DateTime>();
                var days = new List<string>();
                int AdultTotal = 0;
                int ChildTotal = 0;
                DateTime date;
                string day;
                string res = "";

                for (var dt = CheckIn; dt <= CheckOut; dt = dt.AddDays(1))
                {
                    dates.Add(dt);
                    days.Add(dt.ToString("ddd"));
                }

                for (var itm = 0; itm <= dates.Count - 1; itm++)
                {
                    date = dates[itm];
                    day = days[itm];

                    var list1 = (from a in HDE.RateSlabs
                                 where (a.PropertyId == propertyId && a.RoomCategoryCode == Roomcate && a.StartDate <= date && a.EndDate >= date
                                 && a.Days.ToUpper().Contains(day.ToUpper()))
                                 select new
                                 {
                                     AdultPrice = a.ExtraAdultPrice,
                                     ChildPrice = a.ExtraChildPrice,
                                     RoomRate = a.RoomRate
                                 }).FirstOrDefault();

                    if (list1 != null)
                    {
                        AdultTotal = AdultTotal + Convert.ToInt32(list1.AdultPrice);
                        ChildTotal = ChildTotal + Convert.ToInt32(list1.ChildPrice);
                    }

                }
;
                res = AdultTotal.ToString() + "," + ChildTotal.ToString();
                return Json(res, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public string UpdateRoomNum(int bookingID, string RoomNum)
        {
            try
            {

                using (var context = new HotelDBEntities())
                {
                    // Retrieve entity by id
                    var entity = context.Bookings.FirstOrDefault(item => item.BookingID == bookingID);

                    // Validate entity is not null
                    if (entity != null)
                    {
                        entity.RoomNum = RoomNum;
                        entity.DateModified = DateTime.Today;

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
        public string CheckIn(int bookingID, string Status)
        {
            try
            {

                using (var context = new HotelDBEntities())
                {
                    // Retrieve entity by id
                    var entity = context.Bookings.FirstOrDefault(item => item.BookingID == bookingID);

                    // Validate entity is not null
                    if (entity != null)
                    {
                        entity.BookingStatus = Status;
                        entity.DateModified = DateTime.Today;
                        entity.ThemeColor = "blue";

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
        public string Payment(int bookingID, string Status)
        {
            try
            {

                using (var context = new HotelDBEntities())
                {
                    // Retrieve entity by id
                    var entity = context.Bookings.FirstOrDefault(item => item.BookingID == bookingID);

                    // Validate entity is not null
                    if (entity != null)
                    {
                        entity.BookingStatus = Status;
                        entity.DateModified = DateTime.Today;
                        entity.ThemeColor = "green";

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
        //   [ValidateAntiForgeryToken]
        [HttpPost]
        public string Save(Booking pBI) //(FormCollection frm)
        {
            //string password = frm["password"];
            //byte usertype = !string.IsNullOrEmpty(frm["usertype"]) ? Convert.ToByte(frm["usertype"]) : (byte)4;

            try
            {

                int? bookingID = HDE.Bookings.DefaultIfEmpty().Max(p => p == null ? 0 : p.BookingID);
                string confirmationId = "TIH-" + Convert.ToString(bookingID + 1);

                TimeSpan t = Convert.ToDateTime(pBI.CheckOut) - Convert.ToDateTime(pBI.CheckIn);
                int noOfDay = (int)t.TotalDays;

                int? roomCode = HDE.Rooms
                 .Where(p => p.RoomCategory == pBI.RoomCatCode)
                 .Select(p => p.RoomCode)
                 .FirstOrDefault();

                if (!roomCode.HasValue) // Check if roomCode is null
                {
                    return "Room Code not found for the selected category.";
                }

                // Assign fetched RoomCode to pBI before saving
                pBI.RoomCode = roomCode.Value; // Convert nullable int to int
                pBI.ServiceType = "RateBasis";
                pBI.RequestTime = System.DateTime.Now;
                pBI.BookingStatus = "Booked";
                pBI.ConfirmationNo = confirmationId;
                pBI.DestinationID = "ISB";
                pBI.BookingByBO = true;
                pBI.MiscExtrasCharges = 0;
                pBI.AgentCommission = 0;
                pBI.BreakageWaiverFee = 0;
                pBI.BreakageFee = 0;
                pBI.ExtraCleaning = 0;
                pBI.LinenTowelChange = 0;
                pBI.WelcomePack = 0;
                pBI.AirportPickandDrop = 0;
                pBI.TourismTax = 0;
                pBI.NoOfRooms = 1;
                pBI.VAT = 0;
                pBI.UserID = Convert.ToInt32(Session["UserID"]);
                pBI.DateModified = DateTime.Now;
                pBI.ReservationDate = DateTime.Now;
                pBI.ThemeColor = "red";
                pBI.RatePlanId = 1;
                pBI.NoofDay = noOfDay;
                HDE.Entry(pBI).State = System.Data.EntityState.Added;
                HDE.SaveChanges();

                return "Booking done succesfully";
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        [HttpGet]
        public JsonResult ReleaseRoom(int bookingId)
        {
            try
            {
                // Find the booking record
                Booking booking = HDE.Bookings.SingleOrDefault(b => b.BookingID == bookingId);

                if (booking != null)
                {
                    // Update booking status to "Cancelled"
                    booking.BookingStatus = "Cancelled";
                    HDE.SaveChanges(); // Save changes to the database

                    // Return the available room numbers (you can modify this as needed)
                    var availableRooms = HDE.Bookings.Where(b => b.BookingStatus == "Available")
                                                    .Select(b => new { b.RoomNum })
                                                    .ToList();

                    return Json(availableRooms, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Booking not found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult RoomCategories()
        {
            var RoomCatCode = HDE.Rooms.Where(s => s.blocked == false).Select(x => new { RoomID = x.ID, RoomCategory = x.RoomCategory }).ToList();
            return this.Json(RoomCatCode, JsonRequestBehavior.AllowGet);
        }
    }
}


//string PaxTitle = frm["ddlTitle11"];
//string PaxFName = frm["txtFN11"];
//string PaxLName = frm["txtLN11"];
//int ProductID = Convert.ToInt32(frm["ddlRoomCat"];
//int RoomCatCode = Convert.ToInt32(frm["ddlRoomCat"];
//string RoomCategory = frm["ddlRoomCat.SelectedItem"];
//string PaxEmail = frm["txtEmail"];
//string PaxPhoneNumber = frm["txtContactNo"];
//string PaxDescription = frm["txtDescription"];

//DateTime cin = Convert.ToDateTime(frm["txtCheckin"]);
//DateTime cout = Convert.ToDateTime(frm["txtCheckout"]);
//TimeSpan difference = cout - cin;
//int NoofDays = (int)difference.TotalDays;
//float RoomRate = float.Parse(frm["txtRate"]);
//float Roomtax = float.Parse(frm["txtTax"]);
//float TotalRoomRate = float.Parse(frm["txtTotalAmount."]);
//string HotelCode = frm["hdnHotelCode.Value"];
//int NoOfRooms = Convert.ToInt32(frm["txtNoofGuest"]);
//DateTime RequestTime = System.DateTime.Now;
//DateTime CheckIn = Convert.ToDateTime(frm["txtCheckin"]);
//DateTime checkOut = Convert.ToDateTime(frm["txtCheckout"]);
//string ProcessID = frm["ddlRoomCat"];
//string PropertySource = frm["ddlSource"];                       
//float AccomodationRent = float.Parse(frm["txtAccomdationRent"]);
//int adult = Convert.ToInt32(frm["txtNoofGuest"]);
//int UserID = Convert.ToInt32(Session["CustID"]);
//DateTime DateModified = DateTime.Now;
//bi.PaxTitle = PaxTitle;
//bi.PaxFName = PaxFName;
//bi.PaxLName = PaxLName;
//bi.ProductID = ProductID;
//bi.RoomCatCode = RoomCatCode;
//bi.RoomCategory = RoomCategory;
//bi.PaxEmail = PaxEmail;
//bi.PaxPhoneNumber = PaxPhoneNumber;
//bi.PaxDescription = PaxDescription;

//bi.CheckIn = CheckIn;
//bi.CheckOut = checkOut;
//bi.NoofDay = NoofDays;
//bi.RoomRate = RoomRate;
//bi.RoomTax = Roomtax;
//bi.TotalRoomRate = TotalRoomRate;
//bi.HotelCode = HotelCode;
//bi.SupplierID = 1;
//bi.NoOfRooms = NoOfRooms;
//bi.ServiceType = "RateBasis";
//bi.RequestTime = System.DateTime.Now;
//bi.ProcessID = ProcessID;
//bi.PropertySource = PropertySource;
//bi.BookingStatus = "Booked";
//bi.ConfirmationNo = "123";
//bi.DestinationID = "ISB";
//bi.BookingByBO = true;
//bi.MiscExtrasCharges = 0;
//bi.AgentCommission = 0;
//bi.BreakageWaiverFee = 0;
//bi.BreakageFee = 0;
//bi.ExtraCleaning = 0;
//bi.LinenTowelChange = 0;
//bi.WelcomePack = 0;
//bi.AirportPickandDrop = 0;
//bi.TourismTax = 0;
//bi.AccomodationRent = AccomodationRent;

//bi.NoofGuest = adult;
//bi.VAT = 0;
//bi.UserID = Convert.ToInt32(Session["CustID"]);
//bi.DateModified = DateTime.Now;