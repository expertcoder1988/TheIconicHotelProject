using MountFeastMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Globalization;
using System.Data.Metadata.Edm;

namespace MountFeastMVC.Controllers
{
    public class SearchResultController : Controller
    {
        HotelDBEntities HDE = new HotelDBEntities();
        Booking bi = new Booking();

        // GET: SearchResult
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchResults(string CheckIn, string CheckOut, int Adults, int Children, int NoOfRooms)
        {
            try
            {
                //int PropertyID = 1;
                // var responseBasicProfile = new ResponsePropertySearch();
                using (var db = new HotelDBEntities())
                {

                    string DateString = CheckIn;
                    IFormatProvider culture = new CultureInfo("en-US", true);
                    DateTime datestart = DateTime.ParseExact(DateString, "MM/dd/yyyy", culture);

                    string DateString2 = CheckOut;
                    IFormatProvider culture2 = new CultureInfo("en-US", true);
                    DateTime dateend = DateTime.ParseExact(DateString2, "MM/dd/yyyy", culture2);

                    // Calculate number of days
                    int NoOfDays = (dateend - datestart).Days;
                    ViewBag.NoOfDays = NoOfDays;
                    //string[] sDate = CheckIn.Split('-');
                    //string startDate = sDate[1] + "/" + sDate[0] + "/" + sDate[2];

                    int id = 1;

                    //sDate = CheckOut.Split('-');
                    //string endDate = sDate[1] + "/" + sDate[0] + "/" + sDate[2];

                    IEnumerable<PropertySearch> rowsAffected = db.Database.SqlQuery<PropertySearch>("exec sp_GetSearchResults {0}, {1}, {2}, {3}, {4}",
                         datestart, dateend, Adults, Children, NoOfRooms).ToList();

                    // Adjusting room availability based on vacant and blocked rooms
                    foreach (var itm in rowsAffected)
                    {
                        int v = VacantRooms(itm.RoomCode, itm.RoomCateCode, CheckIn);
                        int b = BlockedRooms(itm.RoomCode, itm.RoomCateCode, CheckIn, CheckOut);
                        itm.NoOfRooms = itm.NoOfRooms - v - b;

                        if (itm.NoOfRooms < 0)
                        {
                            itm.NoOfRooms = 0;
                        }
                        itm.Id = id;
                        id++;
                    }

                    //foreach (var itm in rowsAffected)
                    //{
                    //    int v = VacantRooms(itm.RoomCode, itm.RoomCateCode, CheckIn);
                    //    int b = BlockedRooms(itm.RoomCode, itm.RoomCateCode, CheckOut);
                    //    itm.NoOfRooms = itm.NoOfRooms - v;
                    //    itm.NoOfRooms = itm.NoOfRooms - b;

                    //    if (itm.NoOfRooms < 0)
                    //    {
                    //        itm.NoOfRooms = 0;
                    //    }
                    //    itm.Id = id;
                    //    id++;
                    //}

                    Session["results"] = rowsAffected;

                    RoomcatePics();

                    return View();
                    //return Json(rowsAffected, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;
                // return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
                return RedirectToAction("Index", "Home");
            }
        }

        public int VacantRooms(int? roomCode, int? RoomCateCode, string startDate)
        {
            try
            {
                int vacantRooms = 0;
                //int TotalRooms = 0;
                DateTime _startDate = Convert.ToDateTime(startDate);

                // string jsonString = "";
                // var responseBasicProfile = new ResponsePropertySearch();
                using (var db = new HotelDBEntities())
                {
                    //TotalRooms = db.RoomNumbers.Count(x => x.RoomCode == roomCode && x.Status == false);

                    vacantRooms = db.Bookings.Count(x => x.RoomCode == roomCode && x.CheckIn == _startDate);
                }

                return vacantRooms; // Content(vacantRooms.ToString());
            }
            catch (Exception ex)
            {
                return 0;// Content(ex.Message);
            }
        }

        public int BlockedRooms(int? roomCode, int? RoomCateCode, string startDate, string endDate)
        {
            try
            {
                // Parse the start and end dates
                DateTime _startDate = DateTime.ParseExact(startDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime _endDate = DateTime.ParseExact(endDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                int blockedRooms = 0;

                using (var db = new HotelDBEntities())
                {
                    // Count rooms that have overlapping blocked dates
                    blockedRooms = db.BlockedRooms.Count(x =>
                        x.CateId == RoomCateCode &&
                        x.Status == true &&
                        (
                            (x.StartDate <= _endDate && x.EndDate >= _startDate) // Overlapping check
                        ));
                }

                return blockedRooms;
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return 0;
            }
        }
        public ActionResult RoomDetails(int Id)
        {
            try
            {
                //if (Session["params"] != null)
                //{
                //    string Params = (string)Session["params"];
                //    string[] p = Params.Split('&');
                //    DateTime checkin = Convert.ToDateTime(p[0]);
                //}
                RoomDetail(Id);
                // RoomPrice(Id, checkin);
                RoomPictures(Id);
                return View();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResult RoomPics(int Roomcate)
        {
            try
            {
                //int Id = requestRoom.Id; // HttpContext.Request.Form["Id"];
                //string jsonString = "";
                // var responseBasicProfile = new ResponsePropertySearch();
                using (var db = new HotelDBEntities())
                {
                    var room = (from a in db.RoomPictures
                                where (a.RoomCode == Roomcate)
                                select new
                                {
                                    ImageName = a.Picture
                                }).ToList();

                    ;
                    return Json(room, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public string RoomcatePics()
        {
            try
            {
                //int Id = requestRoom.Id; // HttpContext.Request.Form["Id"];
                //string jsonString = "";
                // var responseBasicProfile = new ResponsePropertySearch();
                using (var db = new HotelDBEntities())
                {
                    var room = (from a in db.RoomPictures
                                where (a.Active == true)
                                select new RoomImages
                                {
                                    Category = a.RoomCode,
                                    ImageName = a.Picture
                                }).ToList();

                    TempData["Images"] = room;
                }

                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public string RoomDetail(int Id)
        {
            try
            {
                //int Id = requestRoom.Id; // HttpContext.Request.Form["Id"];
                //string jsonString = "";
                // var responseBasicProfile = new ResponsePropertySearch();
                using (var db = new HotelDBEntities())
                {
                    var room = (from a in db.Rooms
                                join b in db.RoomCategories
                                on a.RoomCategory equals b.Id
                                where (b.Id == Id)
                                select new RoomDetails
                                {
                                    Id = a.ID,
                                    RoomCode = a.RoomCode,
                                  
                                    RoomCategoryId = a.RoomCategory,
                                    RoomCategory = b.Category,
                                    RoomDescription = a.RoomDescription,
                                    PropertyId = a.propertyID,
                                    BedType = a.BedType,
                                    CheckIn = a.CheckIn,
                                    CheckOut = a.CheckOut,
                                    NoOfRooms = a.NoOfRooms,
                                    Capacity = a.Capacity,
                                    Adult = a.Adult,
                                    NoOfChild = a.NoOfChild,
                                    ChildAge = a.ChildAge,
                                    ShortCode = a.ShortCode,
                                    MinimumPrice = a.MinimumPrice,
                                    MinimumStay = a.MinimumStay,
                                    MaximumStay = a.MaximumStay,
                                    Size = a.Size,
                                    Features = a.Features
                                }).FirstOrDefault();

                    //  TempData["room"] = room;
                    ViewBag.room = room;
                }



                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string RoomPrice(int Id, DateTime checkin)
        {
            try
            {
                // int Id = requestRoom.Id;
                // DateTime checkin = requestRoom.Checkin;
                //  string jsonString = "";
                // var responseBasicProfile = new ResponsePropertySearch();
                using (var db = new HotelDBEntities())
                {
                    var roomPrice = (from a in db.RateSlabs
                                     where (a.RoomCategoryCode == Id && (a.StartDate < checkin && a.EndDate > checkin))
                                     select new
                                     {
                                         RoomPrice = a.ExtraAdultPrice
                                     }).FirstOrDefault();
                    TempData["roomPrice"] = roomPrice;
                }

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public JsonResult RoomPolicy(int CancellationPolicy)
        {
            try
            {
                using (var db = new HotelDBEntities())
                {
                    var room = (from a in db.RatePlans
                                join b in db.CancellationPolicies
                                        on a.CancellationPolicy equals b.Id
                                where (b.Id == CancellationPolicy && a.Status == true)
                                select new
                                {
                                    RatePlanName = a.Name,
                                    CancellationPolicy = b.Name,
                                    FreeCancelBefore = b.PeriodForFreeCharge,
                                    Breakfst = a.Breakfast,
                                    ChargedForNoShow = b.ChargedForNoShow,
                                    GuestChargedForCancellation = b.GuestChargedForCancellation,
                                    HowlongFreeOfCharge = b.HowlongFreeOfCharge
                                }).ToList();

                    ;
                    return Json(room, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public string RoomPictures(int Id)
        {
            try
            {
                // int Id = requestRoom.Id;
                // DateTime checkin = requestRoom.Checkin;
                //  string jsonString = "";
                // var responseBasicProfile = new ResponsePropertySearch();
                using (var db = new HotelDBEntities())
                {
                    var roomPictures = (from a in db.RoomPictures
                                        join b in db.Rooms
                                        on a.RoomCode equals b.RoomCategory
                                        where (b.RoomCategory == Id && a.Active == true)
                                        select new Pictures
                                        {
                                            Picture = a.Picture
                                        }).ToList();

                    TempData["roomPictures"] = roomPictures;
                }

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public ActionResult PaymentError(string pp_ResponseCode, string pp_ResponseMessage)
        {
            try
            {
                TempData["pp_ResponseMessage"] = pp_ResponseMessage;
                TempData["pp_ResponseCode"] = pp_ResponseCode;
                return View();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public ActionResult Thankyou(string billReference, string Total)
        {
            try
            {
                string confirmationId = "";
                string pp_ResponseCode = Request.Form["pp_ResponseCode"];
                string pp_ResponseMessage = Request.Form["pp_ResponseMessage"];

                if (Request.Form["pp_BillReference"] != null)
                {
                    confirmationId = Request.Form["pp_BillReference"];
                }
                else
                {
                    confirmationId = billReference;
                }

                if (pp_ResponseCode == "000" || pp_ResponseCode == null)
                {
                    var _booking = (from obj in HDE.Bookings
                                    join b in HDE.RoomCategories
                                     on obj.RoomCatCode equals b.Id
                                    where (obj.ConfirmationNo == confirmationId)
                                    select new FinalBooking
                                    {
                                        BookingID = obj.BookingID,
                                        PaxTitle = obj.PaxTitle,
                                        PaxFName = obj.PaxFName,
                                        PaxLName = obj.PaxLName,
                                        NoOfRooms = obj.NoOfRooms,
                                        RoomCode = obj.RoomCode,
                                        RoomCatCode = obj.RoomCatCode,
                                        RoomCategory = b.Category,
                                        RoomNum = obj.RoomNum,
                                        PaxEmail = obj.PaxEmail,
                                        PaxPhoneNumber = obj.PaxPhoneNumber,
                                        PaxDescription = obj.PaxDescription,
                                        Adults = obj.Adults,
                                        Children = obj.Children,
                                        CheckIn = obj.CheckIn,
                                        CheckOut = obj.CheckOut,
                                        RoomRate = obj.RoomRate,
                                        HotelCode = obj.HotelCode,
                                        ConfirmationNo = obj.ConfirmationNo,
                                        NoofDay = obj.NoofDay,
                                        TotalRoomRate = obj.TotalRoomRate,
                                        VoucherAmount = obj.VoucherAmount,
                                        PayableAmount = (obj.RoomRate ?? 0) - (obj.VoucherAmount ?? 0)
                                    }).ToList();

                    //RoomCates = RoomCates.Remove(0, 1);
                    //TempData["RoomCates"] = RoomCates;
                    //TempData["noOfDay"] = noOfDay;
                    Session["booking"] = _booking;
                    Session["confirmationId"] = confirmationId;
                    decimal discountAmount = 0;
                    decimal taxAmount = 0;
                    int adultsAllowed = 0;
                    int childrenAllowed = 0;
                    int VoucherAmount = 0;
                    int propertyId = _booking[0].HotelCode ?? 0;
                    int roomCategoryCode = _booking[0].RoomCatCode ?? 0;
                    int roomCategoryId = Convert.ToInt32(roomCategoryCode);
                    string roomImagePath = "";
                    VoucherAmount = _booking[0].VoucherAmount ?? 0;

                    // Calculate number of days
                    TimeSpan t = Convert.ToDateTime(_booking[0].CheckOut) - Convert.ToDateTime(_booking[0].CheckIn);
                    int noOfDay = (int)t.TotalDays;
                    if (noOfDay < 1) noOfDay = 1;

                    using (var db = new HotelDBEntities())
                    {
                        var myData = db.Bookings.Where(x => x.ConfirmationNo == confirmationId).ToList();
                        myData.ForEach(m => m.BookingStatus = "Booked");
                        db.SaveChanges();

                        // Get Discount from RateSlab
                        var rateSlab = db.RateSlabs.FirstOrDefault(r => r.PropertyId == propertyId && r.RoomCategoryCode == roomCategoryCode && r.Status == true);
                        if (rateSlab != null)
                        {
                            discountAmount = (rateSlab.Discount ?? 0) * noOfDay; // Multiply per-day discount by number of days
                        }

                        // Get Tax Percentage and calculate tax on total discount
                        var tax = db.Taxes.FirstOrDefault(x => x.PropertyId == propertyId && x.RoomCategory == roomCategoryCode && x.Status == true);
                        if (tax != null)
                        {
                            decimal taxPercentage = tax.Amount ?? 0;
                            taxAmount = (discountAmount * taxPercentage) / 100;
                        }
                        // ✅ Get Adults and NoOfChild allowed from Room table
                        var roomInfo = db.Rooms
                            .Where(r => r.propertyID == propertyId && r.RoomCategory == roomCategoryCode && r.Status == true)
                            .Select(r => new { r.Adult, r.NoOfChild })
                            .FirstOrDefault();
                        adultsAllowed = roomInfo?.Adult ?? 0;
                        childrenAllowed = roomInfo?.NoOfChild ?? 0;
                        // Get top 1 active image with PicNum == 1 for the selected room category
                        var roomImageName = db.RoomPictures.Where(x => x.RoomCode == roomCategoryId && x.Active == true)
                                            .OrderBy(x => x.PicNum) // You can change this to any relevant field for ordering
                                            .Select(x => x.Picture)
                                            .FirstOrDefault();

                        // Convert image name to a web-accessible path
                        roomImagePath = roomImageName != null ? Url.Content("~/uploads/" + roomImageName) : Url.Content("~/uploads/no-image.jpg");
                    }
                    // Store the image path in ViewBag (single image only)
                    ViewBag.RoomImage = roomImagePath;

                    // Optional: store list of image paths in session if needed for other purposes
                    List<string> roomImagePathList = GetRoomImagePaths(roomCategoryId);
                    Session["RoomImagePaths"] = roomImagePathList;

                    // ✅ Assign to ViewBag to use in view
                    ViewBag.AdultsAllowed = adultsAllowed;
                    ViewBag.ChildrenAllowed = childrenAllowed;
                    ViewBag.VoucherAmount = VoucherAmount;

                    // Pass to View
                    ViewBag.DiscountAmount = discountAmount;
                    ViewBag.TaxAmount = taxAmount;
                    ViewBag.NoOfDays = noOfDay; // Optional, if you want to display number of days
                    var category = _booking[0].RoomCategory;

                    try
                    {
                        int? TotalGuests = _booking[0].Adults + _booking[0].Children;
                        StringBuilder htmlString = new StringBuilder();
                        htmlString.Append("<html lang=\"en\">");
                        htmlString.Append("<head>");
                        htmlString.Append("<meta charset=\"UTF-8\">");
                        htmlString.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
                        htmlString.Append("<title>Booking Confirmation</title>");
                        htmlString.Append("<style>");
                        htmlString.Append("body {");
                        htmlString.Append("font-family: Arial, sans-serif;");
                        htmlString.Append("background-color: #f4f4f4;");
                        htmlString.Append("margin: 0;");
                        htmlString.Append("padding: 0;");
                        htmlString.Append("}");
                        htmlString.Append(".email-container {");
                        htmlString.Append("width: 100%;");
                        htmlString.Append("max-width: 650px;");
                        htmlString.Append("margin: 0 auto;");
                        htmlString.Append("background-color: #ffffff;");
                        htmlString.Append("border-radius: 8px;");
                        htmlString.Append("overflow: hidden;");
                        htmlString.Append("box-shadow: 0 0 15px rgba(0, 0, 0, 0.1);");
                        htmlString.Append("}");
                        htmlString.Append(".header {");
                        htmlString.Append("background-color: #f5f5f5;");
                        htmlString.Append("color: white;");
                        htmlString.Append("}");
                        htmlString.Append(".header h1 {");
                        htmlString.Append("margin: 0;");
                        htmlString.Append("}");
                        htmlString.Append(".content {");
                        htmlString.Append("padding: 20px;");
                        htmlString.Append("}");
                        htmlString.Append(".content h2 {");
                        htmlString.Append("color: #333;");
                        htmlString.Append("}");
                        htmlString.Append(".details {");
                        htmlString.Append("margin-top: 20px;");
                        htmlString.Append("background-color: #f9f9f9;");
                        htmlString.Append("padding: 15px;");
                        htmlString.Append("border-radius: 5px;");
                        htmlString.Append("}");
                        htmlString.Append(".details p {");
                        htmlString.Append("margin: 5px 0;");
                        htmlString.Append("}");
                        htmlString.Append(".footer {");
                        htmlString.Append("background-color: #f4f4f4;");
                        htmlString.Append("padding: 15px;");
                        htmlString.Append("text-align: center;");
                        htmlString.Append("font-size: 12px;");
                        htmlString.Append("color: #777;");
                        htmlString.Append("}");
                        htmlString.Append(".footer a {");
                        htmlString.Append("color: #0066cc;");
                        htmlString.Append("text-decoration: none;");
                        htmlString.Append("}");
                        htmlString.Append(".button {");
                        htmlString.Append("background-color: #0066cc;");
                        htmlString.Append("color: white;");
                        htmlString.Append("padding: 10px 20px;");
                        htmlString.Append("text-decoration: none;");
                        htmlString.Append("border-radius: 5px;");
                        htmlString.Append("display: inline-block;");
                        htmlString.Append("margin-top: 20px;");
                        htmlString.Append("}");
                        htmlString.Append(".responsive-padding {");
                        htmlString.Append("padding: 0px 20px;");
                        htmlString.Append("}");
                        htmlString.Append("/* Media query for mobile (max-width: 768px) */");
                        htmlString.Append("@media (max-width: 768px) {");
                        htmlString.Append(".responsive-padding {");
                        htmlString.Append("padding: 0px 5px; /* Remove padding for mobile view */");
                        htmlString.Append("}");
                        htmlString.Append("}");
                        htmlString.Append("</style>");

                        htmlString.Append("</head>");

                        htmlString.Append("<body>");
                        htmlString.Append("<div class=\"email-container\">");


                        htmlString.Append("<div class=\"header\">");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"min-width:100%\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"center\">");
                        htmlString.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:640px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td class=\"responsive-padding\">");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:144px\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td style=\"padding:22px 0 0 0\"><a style=\"display:inline-block;font-family:Arial,Helvetica,sans-serif;font-size:22px;color:#ffffff;font-weight:600;text-decoration:none\" target=\"_blank\">");
                        htmlString.Append("<img alt=\"icoic\" height=\"60\" src=\"https://theiconichotels.com/Content/Assets/img/logos/ICONIC_logo.png\" style=\"display:block\" width=\"144\" data-image-whitelisted=\"\" class=\"CToWUd\" data-bit=\"iit\">");
                        htmlString.Append("</a></td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("<table align=\"right\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:480px\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"right\" style=\"padding:40px 8px\">");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td style=\"font-size:14px;line-height:20px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#000\">");
                        htmlString.Append("Booking Ref : <strong>" + _booking[0].ConfirmationNo + "</strong>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td height=\"24\"></td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</div>");

                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");

                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");

                        htmlString.Append("<td align=\"center\" width=\"100%\">");
                        htmlString.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:640px\" width=\"100%\">");

                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td height=\"20\">");

                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"center\">");



                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:640px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"padding:0 8px\">");
                        htmlString.Append("<h1 style=\"font-size:22px;line-height:32px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;margin:0\">");
                        htmlString.Append("Thanks <span class=\"il\">" + _booking[0].PaxTitle + " " + _booking[0].PaxFName + " " + _booking[0].PaxLName + "</span>! ");
                        htmlString.Append("Your booking in ICONIC Hotel & Restaurant is confirmed.");
                        htmlString.Append("</h1>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td height=\"20\">");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");



                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:640px\" width=\"100%\">");

                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:30px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:0 8px\">Reservation details</td>");

                        htmlString.Append("</tr>");
                        htmlString.Append("<tr>");

                        htmlString.Append("<td height=\"16\">");

                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td style=\"padding: 0 8px\">");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"border-collapse:separate\" width=\"100%\">");

                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td style=\"border:1px solid #e6e6e6;border-radius:4px\">");

                        htmlString.Append("<table cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");

                        htmlString.Append("<tbody>");

                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table align=\"left\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#6b6b6b;padding:16px\">");
                        htmlString.Append("Check-in");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("<table align=\"left\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("" + Convert.ToDateTime(_booking[0].CheckIn).ToShortDateString() + "");
                        //htmlString.Append("<span style=\"color:#6b6b6b;white-space:nowrap\">");
                        //htmlString.Append("(from 15:00)");
                        //htmlString.Append("</span>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");


                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#6b6b6b;padding:16px\">");
                        htmlString.Append("Check-out");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("" + Convert.ToDateTime(_booking[0].CheckOut).ToShortDateString() + "");
                        //htmlString.Append("<span style=\"color:#6b6b6b;white-space:nowrap\">");
                        //htmlString.Append("(until 12:00)");
                        //htmlString.Append("</span>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");


                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#6b6b6b;padding:16px\">");
                        htmlString.Append("Your reservation");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("" + _booking[0].NoOfRooms + " Room" + "," + _booking[0].NoofDay + " Nights");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");


                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#6b6b6b;padding:16px\">");
                        htmlString.Append("You booked for");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("" + TotalGuests + " Adults" + "");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");


                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#6b6b6b;padding:16px\">");
                        htmlString.Append("Booking Date");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("<address style=\"font-style:normal\">");
                        htmlString.Append("" + Convert.ToDateTime(_booking[0].ReservationDate).ToShortDateString() + "");
                        htmlString.Append("</address>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");


                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#6b6b6b;padding:16px\">");
                        htmlString.Append("Phone");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("<a  style=\"color:#0071c2\" target=\"_blank\">");
                        htmlString.Append("" + _booking[0].PaxPhoneNumber + "");
                        htmlString.Append("</a>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");


                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#6b6b6b;padding:16px\">");
                        htmlString.Append("Email");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("<a  style=\"color:#0071c2\" target=\"_blank\">");
                        htmlString.Append("" + _booking[0].PaxEmail + "");
                        htmlString.Append("</a>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");


                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#6b6b6b;padding:16px\">");
                        htmlString.Append("Room");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("" + _booking[0].RoomCategory + "");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");

                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("<tr>");

                        htmlString.Append("<td height=\"32\">");

                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");



                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:640px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td style=\"font-size:30px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:0 8px\">");
                        htmlString.Append("Price details");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td height=\"16\"></td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td style=\"padding:0 8px\">");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"border-collapse:separate\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td style=\"border:1px solid #e6e6e6;border-radius:4px\">");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:8px 16px\">");
                        htmlString.Append("" + _booking[0].RoomCategory + ", Non-Smoking.");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:0 16px 8px\">");
                        htmlString.Append("5% Bed Tax & 15% GST Tax is included.");
                        //htmlString.Append("" + _booking[0].RoomTax + " GST is included.");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");




                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:0 16px 8px\">");
                        htmlString.Append("To confirm your booking please make a payment of 50% of the total amount and share a screenshot via WhatsApp");
                        htmlString.Append("<a href=\"https://wa.me/+923117245555? text=Hi! ICONIC Hotel & Restaurant.\" target=\"_blank\">");
                        htmlString.Append("<b>+923117245555</b>");
                        htmlString.Append("</a>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("Account Title");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("ICONIC HOTEL AND RESTAURANT");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");

                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("Bank Name");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("Meezan Bank-LariAdda Branch");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");

                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("Account Number");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("30810110175139");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");

                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("IBAN");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("PK47MEZN0030810110175139");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");








                        htmlString.Append("<tr>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td style=\"background-color:#f5f5f5\">");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333\">");
                        htmlString.Append("Total price");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"right\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                        htmlString.Append("<strong style=\"white-space:nowrap\">" + _booking[0].TotalRoomRate + " PKR</strong>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td height=\"32\"></td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");




                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:640px\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td style=\"padding:0px 8px\">");
                        htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" valign=\"top\" width=\"48\"><img alt=\"\" height=\"auto\" src=\"../Growbiz/info.png\" style=\"display:block\" width=\"32\" data-image-whitelisted=\"\" class=\"CToWUd\" data-bit=\"iit\"></td>");
                        htmlString.Append("<td>");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"font-size:30px;line-height:28px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding-top:2px\">");
                        htmlString.Append("Important Details");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td align=\"left\" style=\"padding-top:8px\">");
                        htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");
                        htmlString.Append("<tbody>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333\">");
                        htmlString.Append("<ul align=\"left\" style=\"margin:0;margin-left:24px;padding:0\" type=\"disc\">");
                        htmlString.Append("<li style=\"margin:0;padding:4px 0\">Check-in time is 2 PM, guest before 2PM will be entertained in case of room availability.</li>");
                        htmlString.Append("<li style=\"margin:0;padding:4px 0\">Check-out time is 12(Noon), Guest will be charged fee after this time.</li>");
                        htmlString.Append("<li style=\"margin:0;padding:4px 0\">Any damage to the property will be charged by the guest.</li>");
                        htmlString.Append("<li style=\"margin:0;padding:4px 0\">Pets are not allowed in the property.</li>");
                        htmlString.Append("<li style=\"margin:0;padding:4px 0\">Smoking is not allowed in the rooms.</li>");
                        htmlString.Append("</ul>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("<tr>");
                        htmlString.Append("<td height=\"32\"></td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");

                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</td>");
                        htmlString.Append("</tr>");
                        htmlString.Append("</tbody>");
                        htmlString.Append("</table>");
                        htmlString.Append("</div>");
                        htmlString.Append("</body>");

                        htmlString.Append("</html>");

                        MailMessage mm = new MailMessage();
                        mm.IsBodyHtml = true;
                        mm.To.Add("info@theiconichotels.com");

                        mm.From = new MailAddress("info@theiconichotels.com");
                        mm.Subject = "Order Confirmation: " + confirmationId;
                        mm.Body = htmlString.ToString();
                        SmtpClient smc = new SmtpClient();
                        smc.Host = "mail.theiconichotels.com";
                        smc.Port = 587;
                        smc.UseDefaultCredentials = false;
                        smc.EnableSsl = false;
                        smc.Credentials = new System.Net.NetworkCredential("info@theiconichotels.com", "hIb6$070e");
                        smc.DeliveryMethod = SmtpDeliveryMethod.Network;

                        smc.Send(mm);


                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = ex.Message;
                    }

                    if (_booking[0].PaxEmail != "")
                    {
                        try
                        {
                            int? TotalGuests = _booking[0].Adults + _booking[0].Children;
                            StringBuilder htmlString = new StringBuilder();
                            htmlString.Append("<html lang=\"en\">");
                            htmlString.Append("<head>");
                            htmlString.Append("<meta charset=\"UTF-8\">");
                            htmlString.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
                            htmlString.Append("<title>Booking Confirmation</title>");
                            htmlString.Append("<style>");
                            htmlString.Append("body {");
                            htmlString.Append("font-family: Arial, sans-serif;");
                            htmlString.Append("background-color: #f4f4f4;");
                            htmlString.Append("margin: 0;");
                            htmlString.Append("padding: 0;");
                            htmlString.Append("}");
                            htmlString.Append(".email-container {");
                            htmlString.Append("width: 100%;");
                            htmlString.Append("max-width: 650px;");
                            htmlString.Append("margin: 0 auto;");
                            htmlString.Append("background-color: #ffffff;");
                            htmlString.Append("border-radius: 8px;");
                            htmlString.Append("overflow: hidden;");
                            htmlString.Append("box-shadow: 0 0 15px rgba(0, 0, 0, 0.1);");
                            htmlString.Append("}");
                            htmlString.Append(".header {");
                            htmlString.Append("background-color: #f5f5f5;");
                            htmlString.Append("color: white;");
                            htmlString.Append("}");
                            htmlString.Append(".header h1 {");
                            htmlString.Append("margin: 0;");
                            htmlString.Append("}");
                            htmlString.Append(".content {");
                            htmlString.Append("padding: 20px;");
                            htmlString.Append("}");
                            htmlString.Append(".content h2 {");
                            htmlString.Append("color: #333;");
                            htmlString.Append("}");
                            htmlString.Append(".details {");
                            htmlString.Append("margin-top: 20px;");
                            htmlString.Append("background-color: #f9f9f9;");
                            htmlString.Append("padding: 15px;");
                            htmlString.Append("border-radius: 5px;");
                            htmlString.Append("}");
                            htmlString.Append(".details p {");
                            htmlString.Append("margin: 5px 0;");
                            htmlString.Append("}");
                            htmlString.Append(".footer {");
                            htmlString.Append("background-color: #f4f4f4;");
                            htmlString.Append("padding: 15px;");
                            htmlString.Append("text-align: center;");
                            htmlString.Append("font-size: 12px;");
                            htmlString.Append("color: #777;");
                            htmlString.Append("}");
                            htmlString.Append(".footer a {");
                            htmlString.Append("color: #0066cc;");
                            htmlString.Append("text-decoration: none;");
                            htmlString.Append("}");
                            htmlString.Append(".button {");
                            htmlString.Append("background-color: #0066cc;");
                            htmlString.Append("color: white;");
                            htmlString.Append("padding: 10px 20px;");
                            htmlString.Append("text-decoration: none;");
                            htmlString.Append("border-radius: 5px;");
                            htmlString.Append("display: inline-block;");
                            htmlString.Append("margin-top: 20px;");
                            htmlString.Append("}");
                            htmlString.Append(".responsive-padding {");
                            htmlString.Append("padding: 0px 20px;");
                            htmlString.Append("}");
                            htmlString.Append("/* Media query for mobile (max-width: 768px) */");
                            htmlString.Append("@media (max-width: 768px) {");
                            htmlString.Append(".responsive-padding {");
                            htmlString.Append("padding: 0px 5px; /* Remove padding for mobile view */");
                            htmlString.Append("}");
                            htmlString.Append("}");
                            htmlString.Append("</style>");

                            htmlString.Append("</head>");

                            htmlString.Append("<body>");
                            htmlString.Append("<div class=\"email-container\">");


                            htmlString.Append("<div class=\"header\">");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"min-width:100%\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"center\">");
                            htmlString.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:640px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td class=\"responsive-padding\">");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:144px\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td style=\"padding:22px 0 0 0\"><a style=\"display:inline-block;font-family:Arial,Helvetica,sans-serif;font-size:22px;color:#ffffff;font-weight:600;text-decoration:none\" target=\"_blank\">");
                            htmlString.Append("<img alt=\"icoic\" height=\"60\" src=\"https://theiconichotels.com/Content/Assets/img/logos/ICONIC_logo.png\" style=\"display:block\" width=\"144\" data-image-whitelisted=\"\" class=\"CToWUd\" data-bit=\"iit\">");
                            htmlString.Append("</a></td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("<table align=\"right\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:480px\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"right\" style=\"padding:40px 8px\">");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td style=\"font-size:14px;line-height:20px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#000\">");
                            htmlString.Append("Booking Ref : <strong>" + _booking[0].ConfirmationNo + "</strong>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td height=\"24\"></td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</div>");

                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");

                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");

                            htmlString.Append("<td align=\"center\" width=\"100%\">");
                            htmlString.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:640px\" width=\"100%\">");

                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td height=\"20\">");

                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"center\">");



                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:640px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"padding:0 8px\">");
                            htmlString.Append("<h1 style=\"font-size:22px;line-height:32px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;margin:0\">");
                            htmlString.Append("Thanks <span class=\"il\">" + _booking[0].PaxTitle + " " + _booking[0].PaxFName + " " + _booking[0].PaxLName + "</span>! ");
                            htmlString.Append("Your booking in ICONIC Hotel & Restaurant is confirmed.");
                            htmlString.Append("</h1>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td height=\"20\">");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");



                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:640px\" width=\"100%\">");

                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:30px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:0 8px\">Reservation details</td>");

                            htmlString.Append("</tr>");
                            htmlString.Append("<tr>");

                            htmlString.Append("<td height=\"16\">");

                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td style=\"padding: 0 8px\">");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"border-collapse:separate\" width=\"100%\">");

                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td style=\"border:1px solid #e6e6e6;border-radius:4px\">");

                            htmlString.Append("<table cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");

                            htmlString.Append("<tbody>");

                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table align=\"left\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#6b6b6b;padding:16px\">");
                            htmlString.Append("Check-in");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("<table align=\"left\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("" + Convert.ToDateTime(_booking[0].CheckIn).ToShortDateString() + "");
                            //htmlString.Append("<span style=\"color:#6b6b6b;white-space:nowrap\">");
                            //htmlString.Append("(from 15:00)");
                            //htmlString.Append("</span>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");


                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#6b6b6b;padding:16px\">");
                            htmlString.Append("Check-out");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("" + Convert.ToDateTime(_booking[0].CheckOut).ToShortDateString() + "");
                            //htmlString.Append("<span style=\"color:#6b6b6b;white-space:nowrap\">");
                            //htmlString.Append("(until 12:00)");
                            //htmlString.Append("</span>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");


                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#6b6b6b;padding:16px\">");
                            htmlString.Append("Your reservation");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("" + _booking[0].NoOfRooms + " Room" + "," + _booking[0].NoofDay + " Nights");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");


                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#6b6b6b;padding:16px\">");
                            htmlString.Append("You booked for");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("" + TotalGuests + " Adults" + "");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");


                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#6b6b6b;padding:16px\">");
                            htmlString.Append("Booking Date");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("<address style=\"font-style:normal\">");
                            htmlString.Append("" + Convert.ToDateTime(_booking[0].ReservationDate).ToShortDateString() + "");
                            htmlString.Append("</address>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");


                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#6b6b6b;padding:16px\">");
                            htmlString.Append("Phone");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("<a  style=\"color:#0071c2\" target=\"_blank\">");
                            htmlString.Append("" + _booking[0].PaxPhoneNumber + "");
                            htmlString.Append("</a>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");


                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#6b6b6b;padding:16px\">");
                            htmlString.Append("Email");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("<a  style=\"color:#0071c2\" target=\"_blank\">");
                            htmlString.Append("" + _booking[0].PaxEmail + "");
                            htmlString.Append("</a>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");


                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#6b6b6b;padding:16px\">");
                            htmlString.Append("Room");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("" + _booking[0].RoomCategory + "");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");

                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("<tr>");

                            htmlString.Append("<td height=\"32\">");

                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");



                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:640px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td style=\"font-size:30px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:0 8px\">");
                            htmlString.Append("Price details");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td height=\"16\"></td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td style=\"padding:0 8px\">");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"border-collapse:separate\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td style=\"border:1px solid #e6e6e6;border-radius:4px\">");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:8px 16px\">");
                            htmlString.Append("" + _booking[0].RoomCategory + ", Non-Smoking.");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:0 16px 8px\">");
                            htmlString.Append("5% Bed Tax & 15% GST Tax is included.");
                            //htmlString.Append("" + _booking[0].RoomTax + " GST is included.");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");




                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:0 16px 8px\">");
                            htmlString.Append("To confirm your booking please make a payment of 50% of the total amount and share a screenshot via WhatsApp");
                            htmlString.Append("<a href=\"https://wa.me/+923117245555? text=Hi! ICONIC Hotel & Restaurant.\" target=\"_blank\">");
                            htmlString.Append("<b>+923117245555</b>");
                            htmlString.Append("</a>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("Account Title");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("ICONIC HOTEL AND RESTAURANT");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");

                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("Bank Name");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("Meezan Bank-LariAdda Branch");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");

                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("Account Number");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("30810110175139");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");

                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("IBAN");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("PK47MEZN0030810110175139");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");








                            htmlString.Append("<tr>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:622px;border-top:1px solid #e6e6e6\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td style=\"background-color:#f5f5f5\">");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:224px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333\">");
                            htmlString.Append("Total price");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:382px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"right\" style=\"font-size:16px;line-height:24px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding:16px\">");
                            htmlString.Append("<strong style=\"white-space:nowrap\">" + _booking[0].TotalRoomRate + " PKR</strong>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td height=\"32\"></td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");




                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"max-width:640px\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td style=\"padding:0px 8px\">");
                            htmlString.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" valign=\"top\" width=\"48\"><img alt=\"\" height=\"auto\" src=\"../Growbiz/info.png\" style=\"display:block\" width=\"32\" data-image-whitelisted=\"\" class=\"CToWUd\" data-bit=\"iit\"></td>");
                            htmlString.Append("<td>");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"font-size:30px;line-height:28px;font-weight:bold;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333;padding-top:2px\">");
                            htmlString.Append("Important Details");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td align=\"left\" style=\"padding-top:8px\">");
                            htmlString.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");
                            htmlString.Append("<tbody>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td style=\"font-size:16px;line-height:24px;font-weight:normal;font-family:BlinkMacSystemFont,-apple-system,Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333333\">");
                            htmlString.Append("<ul align=\"left\" style=\"margin:0;margin-left:24px;padding:0\" type=\"disc\">");
                            htmlString.Append("<li style=\"margin:0;padding:4px 0\">Check-in time is 2 PM, guest before 2PM will be entertained in case of room availability.</li>");
                            htmlString.Append("<li style=\"margin:0;padding:4px 0\">Check-out time is 12(Noon), Guest will be charged fee after this time.</li>");
                            htmlString.Append("<li style=\"margin:0;padding:4px 0\">Any damage to the property will be charged by the guest.</li>");
                            htmlString.Append("<li style=\"margin:0;padding:4px 0\">Pets are not allowed in the property.</li>");
                            htmlString.Append("<li style=\"margin:0;padding:4px 0\">Smoking is not allowed in the rooms.</li>");
                            htmlString.Append("</ul>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("<tr>");
                            htmlString.Append("<td height=\"32\"></td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");

                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</tbody>");
                            htmlString.Append("</table>");
                            htmlString.Append("</div>");
                            htmlString.Append("</body>");

                            htmlString.Append("</html>");

                            MailMessage mm = new MailMessage();
                            mm.IsBodyHtml = true;
                            mm.To.Add(_booking[0].PaxEmail);

                            mm.From = new MailAddress("info@theiconichotels.com");
                            mm.Subject = "Order Confirmation: " + confirmationId;
                            mm.Body = htmlString.ToString();
                            SmtpClient smc = new SmtpClient();
                            smc.Host = "mail.theiconichotels.com";
                            smc.Port = 587;
                            smc.UseDefaultCredentials = false;
                            smc.EnableSsl = false;
                            smc.Credentials = new System.Net.NetworkCredential("info@theiconichotels.com", "hIb6$070e");
                            smc.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smc.Send(mm);


                        }
                        catch (Exception ex)
                        {
                            TempData["error"] = ex.Message;
                        }

                    }

                    return View();
                }
                else
                {
                    return RedirectToAction("PaymentError", "SearchResult", new { @pp_ResponseCode = pp_ResponseCode, @pp_ResponseMessage = pp_ResponseMessage });
                }



            }
            catch (Exception ex)
            {

                return Json(new { status = "error", message = ex.ToString(), JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public ActionResult Thankyou(FormCollection frm)
        {
            try
            {

                string PaxTitle = Request.Form["PaxTitle"];
                string PaxFName = Request.Form["PaxFName"];
                string PaxLName = Request.Form["PaxLName"];
                string CheckIn = Convert.ToString(Request.Form["CheckIn"]);
                string CheckOut = Convert.ToString(Request.Form["CheckOut"]);
                int RoomCatCode = 0;  //Convert.ToInt32(Request.Form["RoomCatCode"]);
                string PaxPhoneNumber = Request.Form["PaxPhoneNumber"];
                string PaxEmail = Request.Form["PaxEmail"];
                string PaxDescription = Request.Form["PaxDescription"];
                int Adults = Convert.ToInt32(Request.Form["Adults"]);
                int Children = Convert.ToInt32(Request.Form["Children"]);
                int NoOfRooms = Convert.ToInt32(Request.Form["hdTotalRooms"]);
                int RoomRate = 0; // Convert.ToInt32(Request.Form["RoomRate"]);
                int TotalAmount = Convert.ToInt32(Request.Form["hdTotalAmount"]);
                string RoomCatCodes = Request.Form["RoomCatCode"];
                string RoomCateRoomNums = Request.Form["RoomCateRoomNums"];
                string RatePlanIDs = Request.Form["RatePlanIDs"];
                string RoomRates = Request.Form["RoomRates"];

                string[] _CheckIn = CheckIn.Split('-');
                string[] _CheckOut = CheckOut.Split('-');

                CheckIn = _CheckIn[1] + "/" + _CheckIn[0] + "/" + _CheckIn[2];
                CheckOut = _CheckOut[1] + "/" + _CheckOut[0] + "/" + _CheckOut[2];

                if (Adults == 0)
                {
                    Adults = 1;
                }

                SaveBooking(PaxTitle, PaxFName, PaxLName, Convert.ToDateTime(CheckIn), Convert.ToDateTime(CheckOut), RoomCatCode, PaxPhoneNumber, PaxEmail, PaxDescription, Adults,
                    Children, NoOfRooms, RoomRate, TotalAmount, RoomCatCodes, RoomCateRoomNums, RatePlanIDs, RoomRates);

                return View();
            }
            catch (Exception ex)
            {

                return Json(new { status = "error", message = ex.ToString() });
            }
        }

        public ActionResult Reservation(DateTime checkIn, DateTime checkOut, int Adults, int Children, int NoOfRooms, int RoomRate, int TotalAmount, string RoomCatCodes,
            string Quatntity, string RatePlanIDs, string RoomRates)
        {
            var db = new HotelDBEntities();
            var roomcategory = Convert.ToInt32(RoomCatCodes);
            ViewBag.RoomCategory = db.RoomPictures.Where(x => x.RoomCode == roomcategory).FirstOrDefault();
            ViewBag.Category = db.RoomCategories.Where(x => x.Id == roomcategory).FirstOrDefault();

            List<string> roomImagePathList = new List<string>();
            var roomImageName = db.RoomPictures
                .Where(x => x.RoomCode == roomcategory && x.Active == true)
                .Select(x => x.Picture)
                .FirstOrDefault();

            string roomImagePath = roomImageName != null ? Url.Content("~/uploads/" + roomImageName) : Url.Content("~/uploads/no-image.jpg");
            roomImagePathList.Add(roomImagePath);
            Session["RoomImagePaths"] = roomImagePathList;
            ViewBag.RoomImage = roomImagePathList.FirstOrDefault();  
            ViewBag.checkIn = checkIn;
            ViewBag.checkOut = checkOut;
            ViewBag.Adults = Adults;
            ViewBag.Children = Children;
            ViewBag.NoOfRooms = NoOfRooms;
            ViewBag.RoomRate = RoomRate;
            ViewBag.TotalAmount = TotalAmount;
            ViewBag.RoomCatCodes = RoomCatCodes;
            ViewBag.RoomCateRoomNums = Quatntity;
            ViewBag.RatePlanIDs = RatePlanIDs;
            ViewBag.RoomRates = RoomRates;

            return View();
        }

        [HttpPost]
        public ActionResult ReservationPost()
        {

            string checkIn = Convert.ToString(Request.Form["CheckIn"]);
            string checkOut = Convert.ToString(Request.Form["CheckOut"]);
            int RoomCatCode = 0;  //Convert.ToInt32(Request.Form["RoomCatCode"]);
            int Adults = Convert.ToInt32(Request.Form["Adults"]);
            int Children = Convert.ToInt32(Request.Form["Children"]);
            int NoOfRooms = Convert.ToInt32(Request.Form["hdTotalRooms"]);
            int RoomRate = 0; // Convert.ToInt32(Request.Form["RoomRate"]);
            int TotalAmount = Convert.ToInt32(Request.Form["hdTotalAmount"]);
            string RoomCatCodes = Request.Form["RoomCatCode"];
            string RoomCateRoomNums = Request.Form["RoomCateRoomNums"];
            string RatePlanIDs = Request.Form["RatePlanIDs"];
            string RoomRates = Request.Form["RoomRates"];

            string[] _CheckIn = checkIn.Split('-');
            string[] _CheckOut = checkOut.Split('-');

            checkIn = _CheckIn[1] + "/" + _CheckIn[0] + "/" + _CheckIn[2];
            checkOut = _CheckOut[1] + "/" + _CheckOut[0] + "/" + _CheckOut[2];

            DateTime CheckIn = Convert.ToDateTime(checkIn);
            DateTime CheckOut = Convert.ToDateTime(checkOut);
            return RedirectToAction("Reservation", "SearchResult", new { @checkIn = CheckIn, @checkOut = CheckOut, @Adults = Adults, @Children = Children, @NoOfRooms = NoOfRooms, @RoomRate = RoomRate, @TotalAmount = TotalAmount, @RoomCatCodes = RoomCatCodes, @RoomCateRoomNums = RoomCateRoomNums, @RatePlanIDs = RatePlanIDs, @RoomRates = RoomRates });
        }

        [HttpPost]
        public ActionResult ReservationPost2()
        {

            string checkIn = Convert.ToString(Request.Form["CheckIn"]);
            string checkOut = Convert.ToString(Request.Form["CheckOut"]);
            //  int RoomCatCode = 0;  //Convert.ToInt32(Request.Form["RoomCatCode"]);
            int Adults = Convert.ToInt32(Request.Form["Adults"]);
            int Children = Convert.ToInt32(Request.Form["Children"]);
            int NoOfRooms = Convert.ToInt32(Request.Form["hdTotalRooms"]);
            int RoomRate = 0; // Convert.ToInt32(Request.Form["RoomRate"]);
            int TotalAmount = Convert.ToInt32(Request.Form["hdTotalAmount"]);
            string RoomCatCodes = Request.Form["RoomCate"];
            string txtQuatntity = Request.Form["txtQuatntity"];
            string RatePlanIDs = Request.Form["RatePlanIDs"];
            string RoomRates = Request.Form["RoomRates"];

            //string[] _CheckIn = checkIn.Split('-');
            //string[] _CheckOut = checkOut.Split('-');

            //checkIn = checkIn[1] + "/" + checkIn[0] + "/" + checkIn[2];
            //checkOut = checkIn[1] + "/" + checkIn[0] + "/" + checkIn[2];

            string DateString = checkIn;
            IFormatProvider culture = new CultureInfo("en-US", true);
            DateTime datestart = DateTime.ParseExact(DateString, "MM/dd/yyyy", culture);

            string DateString2 = checkOut;
            IFormatProvider culture2 = new CultureInfo("en-US", true);
            DateTime dateend = DateTime.ParseExact(DateString2, "MM/dd/yyyy", culture2);


            return RedirectToAction("Reservation", "SearchResult", new { @checkIn = datestart, @checkOut = dateend, @Adults = Adults, @Children = Children, @NoOfRooms = NoOfRooms, @RoomRate = RoomRate, @TotalAmount = TotalAmount, @RoomCatCodes = RoomCatCodes, @Quatntity = txtQuatntity, @RatePlanIDs = RatePlanIDs, @RoomRates = RoomRates });
        }


        public string SaveBooking(string PaxTitle, string PaxFName, string PaxLName, DateTime CheckIn, DateTime CheckOut, int RoomCatCode,
            string PaxPhoneNumber, string PaxEmail, string PaxDescription, int Adults, int Children, int NoOfRooms, int RoomRate, int TotalAmount,
            string RoomCatCodes, string RoomCateRoomNums, string RatePlanIDs, string RoomRates/*,string Taxes,string AmountWithoutTaxes*/)
        {
            try
            {
                using (var db = new HotelDBEntities())
                {
                    var rowsAffected = db.Bookings.Where(x => x.PaxFName == PaxFName && x.PaxLName == PaxLName &&
                                x.CheckIn == CheckIn).FirstOrDefault();

                    if (true == true)
                    {
                        int? bookingID = db.Bookings.DefaultIfEmpty().Max(p => p == null ? 0 : p.BookingID);
                        string confirmationId = "MHH-" + Convert.ToString(bookingID + 1);
                        int? PropertyId = db.Rooms.Where(p => p.RoomCategory == RoomCatCode).Select(p => p.propertyID).FirstOrDefault();
                        int? RoomCode = 0;
                        string RoomCate = "";
                        string RoomCates = "";
                        TimeSpan t = Convert.ToDateTime(CheckOut) - Convert.ToDateTime(CheckIn);
                        int noOfDay = (int)t.TotalDays;
                        string[] _RoomCatCodes;
                        string[] _RoomCateRoomNums;
                        string[] _RatePlanIDs;
                        string[] _RoomRates;

                        RoomRates = RoomRates.Remove(0, 1);
                        _RoomRates = RoomRates.Split(',');

                        RoomCatCodes = RoomCatCodes.Remove(0, 1);
                        _RoomCatCodes = RoomCatCodes.Split(',');

                        RatePlanIDs = RatePlanIDs.Remove(0, 1);
                        _RatePlanIDs = RatePlanIDs.Split(',');

                        RoomCateRoomNums = RoomCateRoomNums.Remove(0, 1);
                        _RoomCateRoomNums = RoomCateRoomNums.Split(',');

                        //foreach (var itm in _RoomCatCodes)
                        for (int itm = 0; itm < _RoomCatCodes.Length; itm++)
                        {
                            int CateCode = Convert.ToInt32(_RoomCatCodes[itm]);
                            RoomCode = db.Rooms.Where(p => p.RoomCategory == CateCode).Select(p => p.RoomCode).FirstOrDefault();
                            RoomCate = db.RoomCategories.Where(p => p.Id == CateCode).Select(p => p.Category).FirstOrDefault();
                            RoomCates = RoomCates + "," + RoomCate;

                            //foreach (var itm1 in _RoomCateRoomNums)
                            {
                                int NoofRooms = Convert.ToInt32(_RoomCateRoomNums[itm]);

                                for (int i = 1; i <= NoofRooms; i++)
                                {
                                    var bookedRooms = db.Bookings.Where(x => x.RoomCode == RoomCode && x.CheckIn == CheckIn).Select(p => p.RoomNum).ToList();
                                    var RoomNum = (db.RoomNumbers.Where(p => p.RoomCode == RoomCode).Select(p => p.RoomNumber1).ToList());
                                    //var roomnum = db.RoomNumbers.Where(p => p.RoomNumber1 == RoomNum).FirstOrDefault();

                                    var roomnum = RoomNum.Except(bookedRooms).FirstOrDefault();

                                    if (roomnum != null)
                                    {
                                        //roomnum.Booked = true;

                                        //db.Entry(roomnum).State = System.Data.EntityState.Modified;
                                        //db.SaveChanges();

                                        Booking booking = new Booking();

                                        booking.PaxTitle = PaxTitle;
                                        booking.PaxFName = PaxFName;
                                        booking.PaxLName = PaxLName;
                                        booking.NoOfRooms = 1;
                                        booking.RoomCode = RoomCode;
                                        booking.RoomCatCode = CateCode;
                                        booking.RoomNum = roomnum;
                                        booking.PaxEmail = PaxEmail;
                                        booking.PaxPhoneNumber = PaxPhoneNumber;
                                        booking.PaxDescription = PaxDescription;
                                        booking.Adults = Adults;
                                        booking.Children = Children;
                                        booking.CheckIn = CheckIn;
                                        booking.CheckOut = CheckOut;
                                        booking.ReservationDate = DateTime.Now;
                                        booking.RoomRate = Convert.ToInt32(_RoomRates[itm]);
                                        booking.TotalRoomRate = Convert.ToInt32(_RoomRates[itm]);
                                        booking.HotelCode = 1;
                                        booking.ConfirmationNo = confirmationId;
                                        booking.DateModified = DateTime.Now;
                                        booking.BookingStatus = "Booked";
                                        booking.NoofDay = noOfDay;
                                        booking.TotalRoomRate = TotalAmount;
                                        booking.ThemeColor = "red";
                                        booking.RatePlanId = Convert.ToInt32(_RatePlanIDs[itm]);

                                        db.Entry(booking).State = EntityState.Added;
                                        db.SaveChanges();




                                    }
                                }
                            }
                        }

                        var _booking = (from obj in HDE.Bookings
                                        join b in HDE.RoomCategories
                                         on obj.RoomCatCode equals b.Id
                                        where (obj.ConfirmationNo == confirmationId)
                                        select new FinalBooking
                                        {
                                            BookingID = obj.BookingID,
                                            PaxTitle = obj.PaxTitle,
                                            PaxFName = obj.PaxFName,
                                            PaxLName = obj.PaxLName,
                                            NoOfRooms = obj.NoOfRooms,
                                            RoomCode = obj.RoomCode,
                                            RoomCatCode = obj.RoomCatCode,
                                            RoomCategory = b.Category,
                                            RoomNum = obj.RoomNum,
                                            PaxEmail = obj.PaxEmail,
                                            PaxPhoneNumber = obj.PaxPhoneNumber,
                                            PaxDescription = obj.PaxDescription,
                                            Adults = obj.Adults,
                                            Children = obj.Children,
                                            CheckIn = obj.CheckIn,
                                            CheckOut = obj.CheckOut,
                                            RoomRate = obj.RoomRate,
                                            HotelCode = obj.HotelCode,
                                            ConfirmationNo = obj.ConfirmationNo,
                                            NoofDay = obj.NoofDay,
                                            TotalRoomRate = obj.TotalRoomRate
                                        }).ToList();

                        RoomCates = RoomCates.Remove(0, 1);
                        TempData["RoomCates"] = RoomCates;
                        TempData["noOfDay"] = noOfDay;
                        Session["booking"] = _booking;
                        Session["confirmationId"] = confirmationId;

                        try
                        {
                            StringBuilder htmlString = new StringBuilder();
                            htmlString.Append(@"<html>");
                            htmlString.Append("<body><div>");
                            htmlString.Append("<div style='background-color:#b7b7b7;color:white;font-size:22px;line-height:44px;padding:0 10px'>The Iconic Hotel</div>");
                            htmlString.Append("<p>Thank you for choosing Amadoka Inn & Suites, located at 123 Calle 70 in San Diego, for your stay. We have successfully confirmed your reservation and have issued you the following confirmation number: <b>" + confirmationId + "</b></p>");
                            htmlString.Append("<br /><p>If you need to make changes or require assistance, please call +923155557221 or email us at info@theiconichotels.com. We look forward to your stay.</p><br /><br />");
                            htmlString.Append("<table border='0'><tr>");
                            htmlString.Append("<table border='0'><tr>");
                            htmlString.Append("</tr></table>");
                            htmlString.Append("<table border='0'><tr>");
                            htmlString.Append("<th>Guest Name</th><th>Phone Num.</th><th>Room Category</th><th>CheckIn Date/Time</th><th>CheckOut</th></tr>");
                            htmlString.Append("<tr><td>" + PaxTitle + " " + PaxFName + " " + PaxLName + " </td><td> " + PaxPhoneNumber + "</td>");
                            htmlString.Append("<td>" + RoomCates + "</td>");
                            htmlString.Append("<td>" + CheckIn + "</td> ");
                            htmlString.Append("<td>" + CheckOut + "</td>");
                            htmlString.Append("</tr>");
                            htmlString.Append("</table><br />");
                            htmlString.Append("<span><b>Adults:</b></span><span>" + Adults + "</span><br />");
                            htmlString.Append("<span><b>Children:</b></span><span>" + Children + "</span><br />");
                            htmlString.Append("<span><b>No. Of Rooms:</b></span><span>" + NoOfRooms + "</span><br />");
                            htmlString.Append("<span><b>No. Of Night:</b></span><span>" + noOfDay + "</span><br />");
                            htmlString.Append("<span><b>Total Amount:</b></span><span>" + TotalAmount + "</span><br />");

                            htmlString.Append("</div></body>");
                            htmlString.Append("</html>");

                            MailMessage mm = new MailMessage();
                            mm.IsBodyHtml = true;
                            mm.To.Add("info@manilaheights.com");

                            mm.From = new MailAddress("growbiztec@gmail.com");
                            mm.Subject = "Order Confirmation: " + confirmationId;
                            mm.Body = htmlString.ToString();
                            SmtpClient smc = new SmtpClient();
                            smc.Host = "smtp.gmail.com";
                            smc.Port = 587;
                            // smc.UseDefaultCredentials = false;
                            smc.EnableSsl = true;
                            smc.Credentials = new System.Net.NetworkCredential("growbiztec@gmail.com", "growbiz@2022");
                            smc.DeliveryMethod = SmtpDeliveryMethod.Network;

                            smc.Send(mm);


                        }
                        catch (Exception ex)
                        {
                            TempData["error"] = ex.Message;
                        }

                        if (PaxEmail != "")
                        {
                            try
                            {
                                StringBuilder htmlString = new StringBuilder();
                                htmlString.Append(@"<html>");
                                htmlString.Append("<body>");
                                htmlString.Append("<h3>Thanks</h3>");
                                htmlString.Append("<p> We have received your booking order and are working on a confirmation.</p>");
                                htmlString.Append("Your Confirmation Num.:  <b>" + confirmationId + "</b><br />");
                                htmlString.Append("<table border='1'><tr>");
                                htmlString.Append("<th>Guest Nmae</th><th>Phone Num.</th><th>Room Category</th><th>CheckIn Date/Time</th><th>CheckOut</th></tr>");
                                htmlString.Append("<tr><td>" + PaxTitle + " " + PaxFName + " " + PaxLName + " </td><td>" + PaxPhoneNumber + " </td>");
                                htmlString.Append("<td>" + RoomCates + "</td>");
                                htmlString.Append("<td>" + CheckIn + "</td> ");
                                htmlString.Append("<td>" + CheckOut + "</td>");
                                htmlString.Append("</tr>");
                                htmlString.Append("</table><br />");
                                htmlString.Append("<span><b>Adults:</b></span><span>" + Adults + "</span><br />");
                                htmlString.Append("<span><b>Children:</b></span><span>" + Children + "</span><br />");
                                htmlString.Append("<span><b>No. Of Rooms:</b></span><span>" + NoOfRooms + "</span><br />");
                                htmlString.Append("<span><b>No. Of Night:</b></span><span>" + noOfDay + "</span><br />");
                                htmlString.Append("<span><b>Total Amount:</b></span><span>" + TotalAmount + "</span><br />");
                                htmlString.Append("</body>");
                                htmlString.Append("</html>");

                                MailMessage mm = new MailMessage();
                                mm.IsBodyHtml = true;
                                mm.To.Add(PaxEmail);

                                mm.From = new MailAddress("info@manilaheights.com");
                                mm.Subject = "Order Confirmation: " + confirmationId;
                                mm.Body = htmlString.ToString();
                                SmtpClient smc = new SmtpClient();
                                smc.Host = "smtp.gmail.com";
                                smc.Port = 587;
                                // smc.UseDefaultCredentials = false;
                                smc.EnableSsl = true;
                                smc.Credentials = new System.Net.NetworkCredential("info@manilaheights.com", "T15sqy1@9");
                                smc.DeliveryMethod = SmtpDeliveryMethod.Network;
                                smc.Send(mm);


                            }
                            catch (Exception ex)
                            {
                                TempData["error"] = ex.Message;
                            }

                        }

                        return ("Booking done succesfully");
                        //jsonString = JsonSerializer.Serialize(confirmationId);

                    }
                    else
                    {
                        // responseBasicProfile.MessageList.Add("Not Record Found.");
                        return ("Something went wrong.");
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return ("Something went wrong.");
            }
        }

        [HttpPost]
        public ActionResult SaveBooking(FormCollection frm)
        {
            try
            {
                string PaymentType = Request.Form["PaymentType"];
                string PaxTitle = Request.Form["PaxTitle"];
                string PaxFName = Request.Form["PaxFName"];
                string PaxLName = Request.Form["PaxLName"];
                string checkIn = Convert.ToString(Request.Form["CheckIn"]);
                string checkOut = Convert.ToString(Request.Form["CheckOut"]);
                int RoomCatCode = 0;  //Convert.ToInt32(Request.Form["RoomCatCode"]);
                string PaxPhoneNumber = Request.Form["PaxPhoneNumber"];
                string PaxEmail = Request.Form["PaxEmail"];
                string PaxDescription = Request.Form["PaxDescription"];
                int Adults = Convert.ToInt32(Request.Form["Adults"]);
                int Children = Convert.ToInt32(Request.Form["Children"]);
                int NoOfRooms = Convert.ToInt32(Request.Form["hdTotalRooms"]);
                int RoomRate = 0; // Convert.ToInt32(Request.Form["RoomRate"]);
                int TotalAmount = Convert.ToInt32(Request.Form["TotalAmount"]);
                string RoomCatCodes = Request.Form["RoomCatCode"];
                string RoomCateRoomNums = Request.Form["RoomCateRoomNums"];
                string RatePlanIDs = Request.Form["RatePlanIDs"];
                string RoomRates = Request.Form["RoomRates"];
                string VoucherCode = Request.Form["VoucherCode"];
                int DiscountedAmount = Convert.ToInt32(Request.Form["DiscountedAmount"]);

                // string[] _CheckIn = checkIn.Split('-');
                // string[] _CheckOut = checkOut.Split('-');

                // checkIn = _CheckIn[1] + "/" + _CheckIn[0] + "/" + _CheckIn[2];
                // checkOut = _CheckOut[1] + "/" + _CheckOut[0] + "/" + _CheckOut[2];

                DateTime CheckIn = Convert.ToDateTime(checkIn);
                DateTime CheckOut = Convert.ToDateTime(checkOut);

                if (Adults == 0)
                {
                    Adults = 1;
                }

                using (var db = new HotelDBEntities())
                {
                    var rowsAffected = db.Bookings.Where(x => x.PaxFName == PaxFName && x.PaxLName == PaxLName &&
                                x.CheckIn == CheckIn).FirstOrDefault();

                    if (true == true)
                    {
                        int? bookingID = db.Bookings.DefaultIfEmpty().Max(p => p == null ? 0 : p.BookingID);
                        string confirmationId = "TIH-" + Convert.ToString(bookingID + 1);
                        int? PropertyId = db.Rooms.Where(p => p.RoomCategory == RoomCatCode).Select(p => p.propertyID).FirstOrDefault();
                        int? RoomCode = 0;
                        string RoomCate = "";
                        string RoomCates = "";
                        TimeSpan t = Convert.ToDateTime(CheckOut) - Convert.ToDateTime(CheckIn);
                        int noOfDay = (int)t.TotalDays;
                        double GrandTotal = 0;

                        {
                            int CateCode = Convert.ToInt32(RoomCatCodes);
                            RoomCode = db.Rooms.Where(p => p.RoomCategory == CateCode).Select(p => p.RoomCode).FirstOrDefault();
                            RoomCate = db.RoomCategories.Where(p => p.Id == CateCode).Select(p => p.Category).FirstOrDefault();
                            RoomCates = RoomCates + "," + RoomCate;

                            //foreach (var itm1 in _RoomCateRoomNums)
                            {
                                int NoofRooms = Convert.ToInt32(RoomCateRoomNums);

                                {
                                    var bookedRooms = db.Bookings.Where(x => x.RoomCode == RoomCode && x.CheckIn == CheckIn).Select(p => p.RoomNum).ToList();
                                    var RoomNum = (db.RoomNumbers.Where(p => p.RoomCode == RoomCode).Select(p => p.RoomNumber1).ToList());
                                    var roomnum = RoomNum.Except(bookedRooms).FirstOrDefault();

                                    if (roomnum != null)
                                    {
                                        GrandTotal = GrandTotal + TotalAmount;

                                        Booking booking = new Booking();

                                        booking.PaxTitle = PaxTitle;
                                        booking.PaxFName = PaxFName;
                                        booking.PaxLName = PaxLName;
                                        booking.NoOfRooms = 1;
                                        booking.RoomCode = RoomCode;
                                        booking.RoomCatCode = CateCode;
                                        booking.RoomNum = roomnum;
                                        booking.PaxEmail = PaxEmail;
                                        booking.PaxPhoneNumber = PaxPhoneNumber;
                                        booking.PaxDescription = PaxDescription;
                                        booking.Adults = Adults;
                                        booking.Children = Children;
                                        booking.CheckIn = CheckIn;
                                        booking.CheckOut = CheckOut;
                                        booking.RoomRate = Convert.ToInt32(RoomRates);
                                        booking.TotalRoomRate = Convert.ToInt32(RoomRates);
                                        booking.HotelCode = 5;
                                        booking.ConfirmationNo = confirmationId;
                                        booking.DateModified = DateTime.Now;
                                        booking.ReservationDate = DateTime.Now;
                                        booking.BookingStatus = "Pending";
                                        booking.NoofDay = noOfDay;
                                        booking.TotalRoomRate = TotalAmount;
                                        booking.ThemeColor = "red";
                                        booking.RatePlanId = Convert.ToInt32(RatePlanIDs);
                                        decimal discountAmount = GetDiscountAmountDecimal(VoucherCode);
                                        if (discountAmount > 0)
                                        {
                                            booking.VoucherAmount = Convert.ToInt32(discountAmount);
                                        }
                                        else
                                        {
                                            booking.VoucherAmount = 0;
                                        }

                                        GrandTotal -= (double)discountAmount;
                                        DiscountedAmount -= (int)discountAmount;

                                        // Insert logic to set TotalRoomRate
                                        if (discountAmount > 0)
                                        {
                                            booking.TotalRoomRate = Convert.ToInt32(GrandTotal);
                                        }
                                        else
                                        {
                                            booking.TotalRoomRate = TotalAmount;
                                        }

                                        db.Entry(booking).State = EntityState.Added;
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }


                        if (PaymentType == "1" || PaymentType == null)
                        {
                            return RedirectToAction("Thankyou", "SearchResult", new { @billReference = confirmationId, @Total = GrandTotal, @err_code = "00", @err_msg = "" });
                        }
                        else
                        {
                            return RedirectToAction("GetAccessToken", "Payfast", new { @billReference = confirmationId, @GrandTotal = GrandTotal });
                        }
                    }
                    else
                    {
                        // responseBasicProfile.MessageList.Add("Not Record Found.");
                        return RedirectToAction("Index", "Home");

                        //return null;
                    }
                }

            }
            catch (Exception ex)
            {
                //TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Home");
                // return null;
            }
        }
        public JsonResult GetDiscountAmount(string code)
        {
            var voucher = HDE.Vouchers.FirstOrDefault(v => v.Code == code);

            if (voucher != null)
            {
                return Json(new { success = true, discountAmount = voucher.DiscountAmount }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, discountAmount = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        private List<string> GetRoomImagePaths(int roomCategoryId)
        {
            var db = new HotelDBEntities();
            var roomImages = db.RoomPictures
                .Where(x => x.RoomCode == roomCategoryId && x.Active == true)
                .Select(x => x.Picture)
                .ToList();

            string imagePathPrefix = "~/uploads/";
            return roomImages.Select(image => imagePathPrefix + image).ToList();
        }
        private decimal GetDiscountAmountDecimal(string code)
        {
            var voucher = HDE.Vouchers.FirstOrDefault(v => v.Code == code);
            return voucher?.DiscountAmount ?? 0;
        }
        public List<string> RoomImagesCat(int roomCategoryId)
        {
            try
            {
                using (var db = new HotelDBEntities())
                {
                    var roomPictures = db.RoomPictures
                        .Where(a => a.RoomCode == roomCategoryId && a.Active == true)
                        .Select(a => a.Picture)
                        .ToList();

                    return roomPictures;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as necessary
                return new List<string>(); // Return an empty list if an exception occurs
            }
        }
    }


    public class BookedRooms
    {
        public string RoomCate { get; set; }
        public int? Quantity { get; set; }
    }

    public class RoomImages
    {
        public int? Category { get; set; }
        public string ImageName { get; set; }
    }

    public class FinalBooking
    {
        public int? BookingID { get; set; }
        public string PaxTitle { get; set; }
        public string PaxFName { get; set; }
        public string PaxLName { get; set; }
        public int? NoOfRooms { get; set; }
        public int? RoomCode { get; set; }
        public int? RoomCatCode { get; set; }
        public string RoomCategory { get; set; }
        public string RoomNum { get; set; }
        public string PaxEmail { get; set; }
        public string PaxPhoneNumber { get; set; }

        public DateTime? ReservationDate { get; set; }
        public string PaxDescription { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int? RoomRate { get; set; }
        public int? HotelCode { get; set; }
        public string ConfirmationNo { get; set; }
        public int? NoofDay { get; set; }
        public int? TotalRoomRate { get; set; }
        public int? VoucherAmount { get; set; }
        public int? PayableAmount { get; set; }
    }
}
