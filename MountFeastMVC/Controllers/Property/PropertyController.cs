using MountFeastMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft;
using System.IO;
using System.Data.Entity;
using System.Data;

namespace HotelWebsite.Controllers
{
    [Authorize]
    public class PropertyController : Controller
    {
        HotelDBEntities HDE = new HotelDBEntities();
        Property py = new Property();

        // GET: Property
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DeletePropertyImages()
        {
            return View();
        }
        public ActionResult NewRoomCategory()
        {
            return View();
        }
        public ActionResult PropertyImages()
        {
            return View();
        }
        public ActionResult Features(int? id)
        {
            return View();
        }
        public ActionResult RoomRates()
        {
            return View();
        }
        public ActionResult Rooms()
        {
            return View();
        }

        public ActionResult RoomImages()
        {
            return View();
        }

        public ActionResult Taxes()
        {
            return View();
        }

        public ActionResult Halls()
        {
            return View();
        }

        public ActionResult BlockRoom()
        {
            return View();
        }
        public JsonResult Properties()
        {
            try
            {
                return Json(from obj in HDE.Properties
                            where obj.Status == true
                            select new
                            {
                                Id = obj.Id,
                                PropertyID = obj.PropertyId,
                                PropertyName = obj.PropertyName,
                                Fax = obj.Fax,
                                Website = obj.Website,
                                CurrencyCode = obj.CurrencyCode,
                                BookingType = obj.BookingType,
                                Phone = obj.Phone,
                                Email = obj.Email,
                                ContactPerson = obj.ContactPerson,
                                CurrencySymbol = obj.CurrencySymbol,
                                Address = obj.Address,
                                Country = obj.CountryCode,
                                State = obj.State,
                                City = obj.City,
                                PostalCode = obj.PostalCode,
                                Latitude = obj.Latitude,
                                Longitude = obj.Longitude,
                                Features = obj.Features
                            },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResult Property(int propertyID)
        {
            return Json(from obj in HDE.Properties
                        where (obj.PropertyId == propertyID)
                        select new
                        {
                            PropertyID = obj.PropertyId,
                            PropertyTypeId = obj.PropertyTypeId,
                            PropertyName = obj.PropertyName,
                            Fax = obj.Fax,
                            Website = obj.Website,
                            CurrencyCode = obj.CurrencyCode,
                            BookingType = obj.BookingType,
                            Phone = obj.Phone,
                            Email = obj.Email,
                            ContactPerson = obj.ContactPerson,
                            CurrencySymbol = obj.CurrencySymbol,
                            Address = obj.Address,
                            CountryCode = obj.CountryCode,
                            State = obj.State,
                            City = obj.City,
                            PostalCode = obj.PostalCode,
                            Latitude = obj.Latitude,
                            Longitude = obj.Longitude,
                            Features = obj.Features
                        },
                JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult LoadCategories()
        {
            var res = from obj in HDE.RoomCategories
                      where (obj.Status == true)
                      select new
                      {
                          CateId = obj.Id,
                          Category = obj.Category
                      };

            return Json(res,
                JsonRequestBehavior.AllowGet); ;
        }
        public JsonResult LoadpropertyCategories(int propertyID)
        {
            var res = from obj in HDE.Rooms
                      join b in HDE.RoomCategories
                       on obj.RoomCategory equals b.Id into rooms
                      from r in rooms.DefaultIfEmpty()
                      where (obj.propertyID == propertyID && obj.Status == true)
                      select new
                      {
                          CateId = r.Id,
                          Category = r.Category
                      };

            return Json(res,
                JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult CheckCategories(int propertyID, int RoomCate)
        {
            int count = HDE.Rooms.Where(x => x.propertyID == propertyID && x.RoomCategory == RoomCate).Count();

            return Json(count, JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadRooms(int propertyID)
        {
            return Json(from obj in HDE.Rooms
                        join b in HDE.RoomCategories
                        on obj.RoomCategory equals b.Id into rooms
                        from r in rooms.DefaultIfEmpty()
                        where (obj.propertyID == propertyID && obj.Status == true)
                        select new
                        {
                            ProductID = obj.ID,
                            RoomCode = obj.RoomCode,
                            RoomCategory = obj.RoomCategory,
                            RoomCategoryDesc = r.Category,
                            RoomDescription = obj.RoomDescription,
                            BedType = obj.BedType,
                            CheckIn = obj.CheckIn,
                            CheckOut = obj.CheckOut,
                            dateCreated = obj.dateCreated,
                            dateModified = obj.dateModified,
                            NoOfRooms = obj.NoOfRooms,
                            Capacity = obj.Capacity,
                            Adult = obj.Adult,
                            NoOfChild = obj.NoOfChild,
                            ChildAge = obj.ChildAge,
                            blocked = obj.blocked,
                            ShortCode = obj.ShortCode,
                            MinimumPrice = obj.MinimumPrice,
                            MinimumStay = obj.MinimumStay,
                            MaximumStay = obj.MaximumStay,
                            Size = obj.Size
                        },
                JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult loadRoomDetails(int RoomCode)
        {
            return Json(from obj in HDE.Rooms
                        where (obj.RoomCode == RoomCode)
                        select new
                        {
                            ProductID = obj.ID,
                            RoomCode = obj.RoomCode,
                            RoomCategory = obj.RoomCategory,
                            RoomDescription = obj.RoomDescription,
                            BedType = obj.BedType,
                            CheckIn = obj.CheckIn,
                            CheckOut = obj.CheckOut,
                            dateCreated = obj.dateCreated,
                            dateModified = obj.dateModified,
                            NoOfRooms = obj.NoOfRooms,
                            Capacity = obj.Capacity,
                            Adult = obj.Adult,
                            NoOfChild = obj.NoOfChild,
                            ChildAge = obj.ChildAge,
                            blocked = obj.blocked,
                            ShortCode = obj.ShortCode,
                            MinimumPrice = obj.MinimumPrice,
                            MinimumStay = obj.MinimumStay,
                            MaximumStay = obj.MaximumStay,
                            Size = obj.Size,
                            Features = obj.Features
                        },
                JsonRequestBehavior.AllowGet); ;
        }
        public JsonResult Countries()
        {
            return Json(from obj in HDE.Coutnries
                        select new
                        {
                            Code = obj.Code,
                            Name = obj.Name
                        },
                JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult PropertyType()
        {
            //return Newtonsoft.Json.JsonConvert.SerializeObject(GlobalVariables.PropertyType);
            return Json(GlobalVariables.PropertyType, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public string AddProperty(Property pPY) //(FormCollection frm)
        {
            try
            {
                int? ID = HDE.Properties.DefaultIfEmpty().Max(p => p == null ? 0 : p.Id);
                int? propertyId = HDE.Properties.DefaultIfEmpty().Max(p => p == null ? 14000 : p.PropertyId);

                pPY.PropertyId = propertyId + 1;
                pPY.PropertyName = "Property " + (ID + 1);
                pPY.UserID = Convert.ToInt32(Session["CustID"]);
                pPY.EntryTime = DateTime.Now;
                pPY.Status = true;


                HDE.Entry(pPY).State = System.Data.EntityState.Added;
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
        public string AddRoom(int propertyID) //(FormCollection frm)
        {
            try
            {
                int? ID = HDE.Rooms.DefaultIfEmpty().Max(p => p == null ? 0 : p.ID);
                int? RoomCode = HDE.Rooms.DefaultIfEmpty().Max(p => p == null ? 15000 : p.RoomCode);
                Room pRO = new Room();
                pRO.RoomCode = RoomCode + 1;
                pRO.RoomDescription = "Room " + (ID + 1);
                pRO.UserID = Convert.ToInt32(Session["CustID"]);
                pRO.EntryTime = DateTime.Now;
                pRO.propertyID = propertyID;
                pRO.Status = true;


                HDE.Entry(pRO).State = System.Data.EntityState.Added;
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
        public string AddRoomCategory(string cate) //(FormCollection frm)
        {
            try
            {
                RoomCategory rc = new RoomCategory();
                rc.Category = cate;

                rc.Status = true;
                HDE.Entry(rc).State = System.Data.EntityState.Added;
                HDE.SaveChanges();

                return "New Category added succesfully";
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        [HttpPost]
        public string AddPropertyBasicInfo(Property pPY) //(FormCollection frm)
        {
            try
            {
                pPY.Id = HDE.Properties.FirstOrDefault(x => x.PropertyId == pPY.PropertyId).Id; ;
                pPY.UserID = Convert.ToInt32(Session["CustID"]);
                pPY.EntryTime = DateTime.Now;
                pPY.Status = true;

                var local = HDE.Set<Property>()
                         .Local
                         .FirstOrDefault(f => f.Id == pPY.Id);
                if (local != null)
                {
                    HDE.Entry(local).State = System.Data.EntityState.Detached;
                }

                HDE.Entry(pPY).State = System.Data.EntityState.Modified;
                HDE.SaveChanges();

                return "Property updated succesfully";
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        [HttpPost]
        public string DeleteProperty(int propertyID)
        {
            try
            {

                using (var context = new HotelDBEntities())
                {
                    // Retrieve entity by id
                    var entity = context.Properties.FirstOrDefault(item => item.PropertyId == propertyID);

                    // Validate entity is not null
                    if (entity != null)
                    {
                        entity.Status = false;
                        entity.EntryTime = DateTime.Today;

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
        public string DeleteRoom(int roomID)
        {
            try
            {

                using (var context = new HotelDBEntities())
                {
                    // Retrieve entity by id
                    var entity = context.Rooms.FirstOrDefault(item => item.RoomCode == roomID);

                    // Validate entity is not null
                    if (entity != null)
                    {
                        entity.Status = false;
                        entity.EntryTime = DateTime.Today;

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
        public string DeleteRoomNumber(int roomID)
        {
            try
            {

                using (var context = new HotelDBEntities())
                {
                    // Retrieve entity by id
                    var entity = context.RoomNumbers.FirstOrDefault(item => item.Id == roomID);

                    // Validate entity is not null
                    if (entity != null)
                    {
                        if (entity.Status == false)
                        {
                            entity.Status = true;
                        }
                        else
                        {
                            entity.Status = false;
                        }
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
        public string DeletePic(int picID)
        {
            try
            {

                using (var context = new HotelDBEntities())
                {
                    // Retrieve entity by id
                    var entity = context.RoomPictures.FirstOrDefault(item => item.Id == picID);

                    // Validate entity is not null
                    if (entity != null)
                    {
                        entity.Active = false;

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
        public string AddRoomBasicInfo(Room pRO) //(FormCollection frm)
        {
            try
            {
                var cate = HDE.Rooms.Where(x => x.RoomCategory == pRO.RoomCategory && x.propertyID == pRO.propertyID && x.Status != false).Select(x => x.ID).FirstOrDefault();
                if (cate == 0)
                {
                    var id = HDE.Rooms.Where(x => x.RoomCode == pRO.RoomCode && x.propertyID == pRO.propertyID && x.Status != false).Select(x => x.ID).FirstOrDefault();
                    pRO.ID = id;
                    pRO.UserID = Convert.ToInt32(Session["CustID"]);
                    pRO.EntryTime = DateTime.Now;
                    pRO.dateCreated = DateTime.Now;
                    pRO.dateModified = DateTime.Now;
                    pRO.Status = true;

                    var local = HDE.Set<Room>()
                             .Local
                             .FirstOrDefault(f => f.ID == pRO.ID);
                    if (local != null)
                    {
                        HDE.Entry(local).State = System.Data.EntityState.Detached;
                    }

                    HDE.Entry(pRO).State = System.Data.EntityState.Modified;
                    HDE.SaveChanges();

                    AddRoomNumbers(pRO.RoomCode, pRO.NoOfRooms);

                    return "Room updated succesfully";
                }
                else
                {
                    var id = HDE.Rooms.Where(x => x.RoomCode == pRO.RoomCode && x.RoomCategory == pRO.RoomCategory && x.propertyID == pRO.propertyID && x.Status != false).Select(x => x.ID).FirstOrDefault();
                    pRO.ID = id;
                    pRO.UserID = Convert.ToInt32(Session["CustID"]);
                    pRO.EntryTime = DateTime.Now;
                    pRO.dateCreated = DateTime.Now;
                    pRO.dateModified = DateTime.Now;
                    pRO.Status = true;

                    var local = HDE.Set<Room>()
                             .Local
                             .FirstOrDefault(f => f.ID == pRO.ID);
                    if (local != null)
                    {
                        HDE.Entry(local).State = System.Data.EntityState.Detached;
                    }

                    HDE.Entry(pRO).State = System.Data.EntityState.Modified;
                    HDE.SaveChanges();

                    AddRoomNumbers(pRO.RoomCode, pRO.NoOfRooms);

                    return "Room updated succesfully";
                }
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        [HttpPost]
        public string UpdatePictureDesc(RoomPicture rp) //(FormCollection frm)
        {
            try
            {
                rp.Active = true;

                HDE.Entry(rp).State = System.Data.EntityState.Modified;
                HDE.SaveChanges();

                return "Room updated succesfully";
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        public string AddRoomNumbers(int? RoomCode, int? Num)
        {
            try
            {
                RoomNumber rn = new RoomNumber();

                var local = (from u in HDE.RoomNumbers where u.RoomCode == RoomCode select u).FirstOrDefault();

                if (local != null)
                {
                    int count = HDE.RoomNumbers.Where(x => x.RoomCode == RoomCode).Count();

                    if (count < Num)
                    {
                        for (int i = 1; i <= Num - count; i++)
                        {
                            rn.RoomCode = RoomCode;
                            rn.Status = true;
                            rn.Booked = false;
                            HDE.Entry(rn).State = System.Data.EntityState.Added;
                            HDE.SaveChanges();
                        }
                    }
                }
                else
                {
                    for (int i = 1; i <= Num; i++)
                    {
                        rn.RoomCode = RoomCode;
                        rn.Status = true;
                        rn.Booked = false;
                        HDE.Entry(rn).State = System.Data.EntityState.Added;
                        HDE.SaveChanges();
                    }
                }
                return "Room updated succesfully";
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        [HttpPost]
        public string UpdateRoomNumbers(RoomNumber pRO)
        {
            try
            {
                RoomNumber rn = new RoomNumber();

                var RN = (from obj in HDE.RoomNumbers
                          where (obj.Id == pRO.Id)
                          select new
                          {
                              RoomCode = obj.RoomCode,
                              Status = obj.Status,
                              Booked = obj.Booked
                          }).FirstOrDefault();

                var local = HDE.Set<RoomNumber>()
                          .Local
                          .FirstOrDefault(f => f.Id == pRO.Id);
                if (local != null)
                {
                    HDE.Entry(local).State = System.Data.EntityState.Detached;
                }

                rn.Id = pRO.Id;
                rn.RoomNumber1 = pRO.RoomNumber1;
                rn.Floor = pRO.Floor;
                rn.RoomCode = RN.RoomCode;
                rn.Status = RN.Status;
                rn.Booked = RN.Booked;

                HDE.Entry(rn).State = System.Data.EntityState.Modified;
                HDE.SaveChanges();


                return "Room updated succesfully";
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        [HttpPost]
        public string AddPropertyAddress(Property pPY) //(FormCollection frm)
        {
            try
            {
                int? ID = HDE.Properties.DefaultIfEmpty().Max(p => p == null ? 0 : p.Id);
                int? propertyId = HDE.Properties.DefaultIfEmpty().Max(p => p == null ? 14000 : p.PropertyId);

                pPY.UserID = Convert.ToInt32(Session["CustID"]);
                pPY.EntryTime = DateTime.Now;


                HDE.Entry(pPY).State = System.Data.EntityState.Modified;
                HDE.SaveChanges();

                return "Property updated succesfully";
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        public JsonResult PicNum(int RoomCode)
        {
            var num = (from obj in HDE.RoomPictures
                       where (obj.RoomCode == RoomCode && obj.Active == true)
                       orderby obj.PicNum descending
                       select new
                       {
                           PicNum = obj.PicNum
                       }).FirstOrDefault();

            return Json(num, JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult RoomPictures(int RoomCode)
        {
            return Json(from obj in HDE.RoomPictures
                        where (obj.RoomCode == RoomCode && obj.Active == true)
                        orderby obj.PicNum descending
                        select new
                        {
                            PicId = obj.Id,
                            PicName = obj.Picture,
                            Description = obj.Description,
                            RoomCode = obj.RoomCode,
                            PicNum = obj.PicNum
                        },
            JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult RoomNumbers(int RoomCode)
        {
            return Json(from obj in HDE.RoomNumbers
                        where (obj.RoomCode == RoomCode)
                        select new
                        {
                            roomId = obj.Id,
                            RoomCode = obj.RoomCode,
                            RoomNumber = obj.RoomNumber1,
                            Floor = obj.Floor,
                            Status = obj.Status,
                            Booked = obj.Booked
                        },
            JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult loadRoomNums(int CateId)
        {
            var RoomCode = (from obj in HDE.Rooms
                            where obj.RoomCategory == CateId
                            select new
                            {
                                RoomCode = obj.RoomCode
                            }).FirstOrDefault();


            return Json(from obj in HDE.RoomNumbers
                        where (obj.RoomCode == RoomCode.RoomCode)
                        select new
                        {
                            roomId = obj.Id,
                            RoomCode = obj.RoomCode,
                            RoomNumber = obj.RoomNumber1,
                            Floor = obj.Floor,
                            Status = obj.Status,
                            Booked = obj.Booked
                        },
            JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult loadNumberOfRooms(int CateId)
        {
            var RoomCode = (from obj in HDE.Rooms
                            where obj.RoomCategory == CateId
                            select new
                            {
                                RoomCode = obj.RoomCode
                            }).FirstOrDefault();

            var NumbreOfRooms = HDE.RoomNumbers.Where(x => x.RoomCode == RoomCode.RoomCode && x.Status == true).Count();


            return Json(NumbreOfRooms, JsonRequestBehavior.AllowGet); ;
        }


        public JsonResult LoadRoomsByCategory(int CateId)
        {
            var rooms = (from r in HDE.Rooms
                         join rn in HDE.RoomNumbers
                         on r.RoomCode equals rn.RoomCode
                         where r.RoomCategory == CateId && r.Status == true && rn.Status == true
                         select new
                         {
                             rn.RoomNumber1
                         }).ToList();

            return Json(rooms, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BlockRooms(BlockedRoom br)
        {
            try
            {
                // Check for overlapping dates
                bool isOverlapping = HDE.BlockedRooms.Any(b =>
                    b.RoomNum == br.RoomNum &&
                    b.Status == true &&
                    (
                        (br.StartDate >= b.StartDate && br.StartDate <= b.EndDate) ||
                        (br.EndDate >= b.StartDate && br.EndDate <= b.EndDate) ||
                        (br.StartDate <= b.StartDate && br.EndDate >= b.EndDate)
                    ));

                if (isOverlapping)
                {
                    return Json(new { success = false, message = "The room is already blocked for the selected dates." });
                }

                // Save room block
                br.EntryTime = DateTime.Today;
                br.Status = true;
                HDE.Entry(br).State = EntityState.Added;
                HDE.SaveChanges();

                return Json(new { success = true, message = "Blocked Room done successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        public JsonResult LoadBlockedRooms(int RoomCode)
        {
            return Json(from obj in HDE.BlockedRooms
                        join b in HDE.RoomCategories
                        on obj.CateId equals b.Id into rooms
                        from r in rooms.DefaultIfEmpty()
                        where (obj.CateId == RoomCode && obj.StartDate >= DateTime.Today && obj.Status == true)
                        select new
                        {
                            Id = obj.Id,
                            RoomCode = obj.CateId,
                            Category = r.Category,
                            RoomNumber = obj.RoomNum,
                            StartDate = obj.StartDate,
                            EndDate = obj.EndDate
                        },
            JsonRequestBehavior.AllowGet); ;
        }

        [HttpPost]
        public string UnBlockRooms(int Id)
        {
            try
            {

                using (var context = new HotelDBEntities())
                {
                    // Retrieve entity by id
                    var entity = context.BlockedRooms.FirstOrDefault(item => item.Id == Id);

                    // Validate entity is not null
                    if (entity != null)
                    {
                        entity.Status = false;
                        entity.EntryTime = DateTime.Today;

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

        public ActionResult Image(string img)
        {
            var image = "C:\\inetpub\\wwwroot\\TheSarai\\Website\\Uploads\\" + img;
            //string path = image.Path; // could be any absolute path anywhere on your server, for example c:\foo\bar.jpg
            string contentType = "image/jpg"; // image.ContentType; // for example image/jpg
            return File(image, contentType);
        }

        [HttpPost]
        public ActionResult UploadRoomImages()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    RoomPicture rp = new RoomPicture();
                    string[] fileName;
                    string[] picnum;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        // fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                        fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);

                        file.SaveAs(fname);

                        fileName = file.FileName.Split('-');
                        picnum = fileName[1].Split('.');

                        rp.RoomCode = Convert.ToInt32(fileName[0].ToString());
                        rp.Picture = file.FileName;
                        rp.PicNum = Convert.ToInt32(picnum[0].ToString());
                        rp.Active = true;

                        HDE.Entry(rp).State = System.Data.EntityState.Added;
                        HDE.SaveChanges();
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        public ActionResult RateSlab()
        {
            return View();
        }

        public JsonResult loadRoomsRateSlab(int roomID)
        {
            return Json(from a in HDE.RoomCategories
                        join b in HDE.RateSlabs
                        on a.Id equals b.RoomCategoryCode
                        where (b.RoomCategoryCode == roomID && b.Status == true)
                        select new
                        {
                            RateSlabId = b.Id,
                            RoomCategoryCode = a.Id,
                            RoomDescription = a.Category,
                            RoomRate = b.RoomRate,
                            ExtraAdultPrice = b.ExtraAdultPrice,
                            ExtraChildPrice = b.ExtraChildPrice,
                            Days = b.Days,
                            Discount = b.Discount,
                            PerNightRate = b.RoomRate,
                            StartDate = b.StartDate,
                            EndDate = b.EndDate
                        },
                JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult loadRateSlab(int rateSlabId)
        {
            return Json(from a in HDE.RoomCategories
                        join b in HDE.RateSlabs
                        on a.Id equals b.RoomCategoryCode
                        where (b.Id == rateSlabId && b.Status == true)
                        select new
                        {
                            RateSlabId = b.Id,
                            RoomCategoryCode = a.Id,
                            RoomDescription = a.Category,
                            RoomRate = b.RoomRate,
                            ExtraAdultPrice = b.ExtraAdultPrice,
                            ExtraChildPrice = b.ExtraChildPrice,
                            PerNightRate = b.RoomRate,
                            Discount = b.Discount,
                            Days = b.Days,
                            StartDate = b.StartDate,
                            EndDate = b.EndDate
                        },
                JsonRequestBehavior.AllowGet); ;
        }

        [HttpPost]
        public string AddRateSlabs(RateSlab po) //(FormCollection frm)
        {
            try
            {
                // string dt = Convert.ToDateTime(po.StartDate).ToShortDateString();
                //var local = HDE.Set<RateSlab>().Local.FirstOrDefault(f => f.Id == po.Id); // && f.StartDate == DbFunctions.TruncateTime(po.StartDate));

                var local = (from u in HDE.RateSlabs where u.Id == po.Id select u).FirstOrDefault();

                if (local != null)
                {
                    HDE.Entry(local).State = System.Data.EntityState.Detached;

                    po.UserId = Convert.ToInt32(Session["CustID"]);
                    po.EntryTime = DateTime.Now;
                    po.Status = true;

                    HDE.Entry(po).State = System.Data.EntityState.Modified;
                    HDE.SaveChanges();
                }
                else
                {
                    po.UserId = Convert.ToInt32(Session["CustID"]);
                    po.EntryTime = DateTime.Now;
                    po.Status = true;

                    HDE.Entry(po).State = System.Data.EntityState.Added;
                    HDE.SaveChanges();
                }

                return "Rate Slabs updated succesfully";
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        [HttpPost]
        public string DeleteRateSlab(int rateID)
        {
            try
            {

                using (var context = new HotelDBEntities())
                {
                    // Retrieve entity by id
                    var entity = context.RateSlabs.FirstOrDefault(item => item.Id == rateID);

                    // Validate entity is not null
                    if (entity != null)
                    {
                        entity.Status = false;
                        entity.EntryTime = DateTime.Today;

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
        public string DeleteRatePlan(int ratePlanID)
        {
            try
            {

                using (var context = new HotelDBEntities())
                {
                    // Retrieve entity by id
                    var entity = context.RatePlans.FirstOrDefault(item => item.Id == ratePlanID);

                    // Validate entity is not null
                    if (entity != null)
                    {
                        entity.Status = false;
                        entity.EntryTime = DateTime.Today;

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
        public string DeleteTax(int TaxId)
        {
            try
            {

                using (var context = new HotelDBEntities())
                {
                    // Retrieve entity by id
                    var entity = context.Taxes.FirstOrDefault(item => item.Id == TaxId);

                    // Validate entity is not null
                    if (entity != null)
                    {
                        entity.Status = false;
                        entity.CreatedAt = DateTime.Today;

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
        public string DeleteCategory(int cateId)
        {
            try
            {

                using (var context = new HotelDBEntities())
                {
                    // Retrieve entity by id
                    var entity = context.RoomCategories.FirstOrDefault(item => item.Id == cateId);

                    // Validate entity is not null
                    if (entity != null)
                    {
                        entity.Status = false;

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

        public JsonResult LoadTaxes(int propertyId, int roomCate)
        {
            return Json(from b in HDE.Taxes
                        join a in HDE.RoomCategories
                        on b.RoomCategory equals a.Id
                        where (b.PropertyId == propertyId && a.Id == roomCate && b.Status == true)
                        select new
                        {
                            Id = b.Id,
                            PropertyId = b.PropertyId,
                            RoomCategory = b.RoomCategory,
                            RoomName = a.Category,
                            TaxName = b.TaxName,
                            DetectedAs = b.DetectedAs,
                            Amount = b.Amount,
                            Status = b.Status
                        },
                JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult LoadMinPrice(int roomId)
        {
            return Json(from b in HDE.Rooms
                        where (b.ID == roomId)
                        select new
                        {
                            MinimumPrice = b.MinimumPrice
                        },
                JsonRequestBehavior.AllowGet); ;
        }

        [HttpPost]
        public string AddNewTax(Tax tax) //(FormCollection frm)
        {
            try
            {
                if (tax.Id == 0)
                {
                    tax.UserId = Convert.ToInt32(Session["CustID"]);
                    tax.CreatedAt = DateTime.Now;
                    tax.Status = true;


                    HDE.Entry(tax).State = System.Data.EntityState.Added;
                    HDE.SaveChanges();

                    return "New Tax Added Sccesfully";
                }
                else
                {
                    tax.UserId = Convert.ToInt32(Session["CustID"]);
                    tax.CreatedAt = DateTime.Now;
                    tax.Status = true;

                    HDE.Entry(tax).State = System.Data.EntityState.Modified;
                    HDE.SaveChanges();

                    return "Tax Updated Sccesfully";
                }
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;

                return ex.Message.ToString();
            }
        }

        public JsonResult LoadHalls(int propertyId)
        {
            return Json(from b in HDE.Halls
                        where (b.PropertyId == propertyId)
                        select new
                        {
                            Id = b.Id,
                            PropertyId = b.PropertyId,
                            HallName = b.HallName,
                            ShortCode = b.ShortCode,
                            CoveredArea = b.CoveredArea,
                            Capacity = b.Capacity,
                            Image = b.Image,
                            Status = b.Status
                        },
                JsonRequestBehavior.AllowGet); ;
        }

        [HttpPost]
        public string AddNewHall(MountFeastMVC.Models.Hall hall) //(FormCollection frm)
        {
            try
            {
                if (hall.Id == 0)
                {
                    hall.UserId = Convert.ToInt32(Session["CustID"]);
                    hall.EntryTime = DateTime.Now;


                    HDE.Entry(hall).State = System.Data.EntityState.Added;
                    HDE.SaveChanges();

                    return "New Tax Added Sccesfully";
                }
                else
                {
                    hall.UserId = Convert.ToInt32(Session["CustID"]);
                    hall.EntryTime = DateTime.Now;


                    HDE.Entry(hall).State = System.Data.EntityState.Modified;
                    HDE.SaveChanges();

                    return "Tax Updated Sccesfully";
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