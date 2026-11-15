using MountFeastMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MountFeastMVC.Controllers
{
    public class RestaurantController : Controller
    {
        [Route("~/restaurant-in-naran", Name = "restaurantinnaran")]

        // GET: Restaurant
        public ActionResult Index()
        {
            RoomList();
            return View();

        }
        public string RoomList()
        {
            try
            {

                // string jsonString = "";
                // var responseBasicProfile = new ResponsePropertySearch();
                using (var db = new HotelDBEntities())
                {
                    var roomList = (from a in db.Rooms
                                    join b in db.RoomCategories
                                    on a.RoomCategory equals b.Id
                                    where a.Status == true
                                    select new PropertySearch
                                    {
                                        Id = a.ID,
                                        RoomCode = a.RoomCode,
                                        RoomCateCode = a.RoomCategory,
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
                                        Features = a.Features,
                                        imgURL = ""
                                    }).ToList();

                    foreach (var itm in roomList)
                    {
                        // Calculate Vacant Rooms
                        int vacantRooms = VacantRooms(itm.RoomCode);
                        itm.NoOfRooms = itm.NoOfRooms - vacantRooms;

                        // Get Discount from RateSlabs and update MinimumPrice
                        DateTime today = DateTime.Today;

                        int discount = db.RateSlabs
                            .Where(x => x.RoomCategoryCode == itm.RoomCateCode
                                        && x.Status == true
                                        && today >= x.StartDate
                                        && today <= x.EndDate)
                            .Select(x => x.Discount)
                            .FirstOrDefault() ?? 0;

                        itm.MinimumPrice = discount;
                        // Set the discounted price

                        // Get Tax from Taxes table based on RoomCategory
                        decimal taxRate = db.Taxes
                       .Where(x => x.RoomCategory == itm.RoomCateCode && x.Status == true) // Filter by RoomCategory and Status
                       .Select(x => x.Amount)
                       .FirstOrDefault() ?? 0m; // Ensure non-nullable with default value 0m

                        // Perform tax calculation
                        decimal adjustedPrice = (itm.MinimumPrice ?? 0) + ((itm.MinimumPrice ?? 0) * (taxRate / 100));

                        // Convert adjustedPrice back to int and update MinimumPrice
                        itm.MinimumPrice = (int)Math.Round(adjustedPrice, MidpointRounding.AwayFromZero);

                        // Get Room Picture URL
                        string img = RoomPic(itm.RoomCateCode);
                        itm.imgURL = img;
                    }





                    TempData["roomList"] = roomList;
                }

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public int VacantRooms(int? roomCode)
        {
            try
            {
                int vacantRooms = 0;


                // string jsonString = "";
                // var responseBasicProfile = new ResponsePropertySearch();
                using (var db = new HotelDBEntities())
                {
                    vacantRooms = db.RoomNumbers.Count(x => x.RoomCode == roomCode && x.Status == false);

                    //TempData["vacantRooms"] = vacantRooms;
                }

                return vacantRooms; // Content(vacantRooms.ToString());
            }
            catch (Exception ex)
            {
                return 0;// Content(ex.Message);
            }
        }

        public string RoomPic(int? roomCate)
        {
            try
            {
                string roomPic = "";


                // string jsonString = "";
                // var responseBasicProfile = new ResponsePropertySearch();
                using (var db = new HotelDBEntities())
                {
                    roomPic = db.RoomPictures.Where(x => x.RoomCode == roomCate).Select(x => x.Picture).FirstOrDefault();

                    //TempData["vacantRooms"] = vacantRooms;
                }

                return roomPic; // Content(vacantRooms.ToString());
            }
            catch (Exception ex)
            {
                return "";// Content(ex.Message);
            }
        }
    }
}