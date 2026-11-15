using MountFeastMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MountFeastMVC.Controllers.Dashboard
{
    [Authorize]
    public class DashboardController : Controller
    {
        HotelDBEntities HDE = new HotelDBEntities();

        [Authorize]
        // GET: Dashboard
        public ActionResult Index()
        {
            Counts();
            return View();
        }

        public string Counts()
        {
            try
            {
                int TodaysBooking = HDE.Bookings.Where(x => x.ReservationDate == DateTime.Today).Count();
                int TodaysCheckIn = HDE.Bookings.Where(x => x.CheckIn == DateTime.Today).Count();
                int TodaysCheckOut = HDE.Bookings.Where(x => x.CheckOut == DateTime.Today).Count();
                int TodaysCancelled = HDE.Bookings.Where(x => x.CheckIn == DateTime.Today && x.BookingStatus == "Cancelled").Count();
                int TotalCategories = HDE.RoomCategories.Where(x => x.Status == true).Count();
                int TotalRooms = HDE.RoomNumbers.Where(x => x.Status == true).Count();
                int TotalBookings = HDE.Bookings.Count();

                TempData["TodaysBooking"] = TodaysBooking;
                TempData["TodaysCheckIn"] = TodaysCheckIn;
                TempData["TodaysCheckOut"] = TodaysCheckOut;
                TempData["TodaysCancelled"] = TodaysCancelled;
                TempData["TotalCategories"] = TotalCategories;
                TempData["TotalRooms"] = TotalRooms;
                TempData["TotalBookings"] = TotalBookings;

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}