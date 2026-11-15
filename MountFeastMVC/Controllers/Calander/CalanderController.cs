
using MountFeastMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MountFeastMVC.Controllers.Calander
{
    [Authorize]
    public class CalanderController : Controller
    {
        // GET: Calander
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetEventsNew(int? propertyID)
        {
            try
            {
                using (HotelDBEntities HDE = new HotelDBEntities())
                {
                    // Define the statuses to exclude
                    var excludedStatuses = new List<string> { "Pending", "Cancelled", "Did Not Pick" };

                    // Fetch bookings excluding the specified statuses
                    var bookings = (from obj in HDE.Bookings
                                    join b in HDE.RoomCategories on obj.RoomCatCode equals b.Id into rooms
                                    from r in rooms.DefaultIfEmpty()
                                    where obj.HotelCode == propertyID
                                       && !excludedStatuses.Contains(obj.BookingStatus)
                                    orderby r.Id
                                    select new Events
                                    {
                                        EventID = obj.BookingID,
                                        EventName = obj.PaxTitle + " " + obj.PaxFName + " " + obj.PaxLName,
                                        StartDate = obj.CheckIn,
                                        EndDate = obj.CheckOut,
                                        BookingStatus = obj.BookingStatus,
                                        ImageType = "",
                                        Url = "",
                                        ProductName = r.Category,
                                        ProductID = obj.RoomCatCode,
                                        PropertySource = obj.PropertySource,
                                        ConfirmationNo = obj.ConfirmationNo,
                                        RoomNo = string.IsNullOrEmpty(obj.RoomNum) ? "" : obj.RoomNum
                                    }).ToList();

                    // Fetch blocked rooms
                    var blockedRooms = (from br in HDE.BlockedRooms
                                        join rc in HDE.RoomCategories on br.CateId equals rc.Id into categories
                                        from c in categories.DefaultIfEmpty()
                                        where br.PropertyId == propertyID && br.Status == true
                                        orderby br.StartDate
                                        select new Events
                                        {
                                            EventID = br.Id,
                                            EventName = "Blocked",
                                            StartDate = br.StartDate,
                                            EndDate = br.EndDate,
                                            BookingStatus = "Blocked",
                                            ImageType = "",
                                            Url = "",
                                            ProductName = c.Category,
                                            ProductID = br.CateId,
                                            PropertySource = "System",
                                            ConfirmationNo = "",
                                            RoomNo = br.RoomNum
                                        }).ToList();

                    // Normalize RoomNo
                    foreach (var blockedRoom in blockedRooms)
                    {
                        blockedRoom.RoomNo = blockedRoom.RoomNo?.ToString() ?? "";
                    }

                    // Combine bookings and blocked rooms
                    bookings.AddRange(blockedRooms);

                    return new JsonResult { Data = bookings, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        //public JsonResult GetEventsNew(int? propertyID)
        //{
        //    try
        //    {
        //        using (HotelDBEntities HDE = new HotelDBEntities())
        //        {
        //            // Fetch bookings
        //            var bookings = (from obj in HDE.Bookings
        //                            join b in HDE.RoomCategories on obj.RoomCatCode equals b.Id into rooms
        //                            from r in rooms.DefaultIfEmpty()
        //                            where obj.HotelCode == propertyID && obj.BookingStatus != "Pending"
        //                            orderby r.Id
        //                            select new Events
        //                            {
        //                                EventID = obj.BookingID,
        //                                EventName = obj.PaxTitle + " " + obj.PaxFName + " " + obj.PaxLName,
        //                                StartDate = obj.CheckIn,
        //                                EndDate = obj.CheckOut,
        //                                BookingStatus = obj.BookingStatus,
        //                                ImageType = "",
        //                                Url = "",
        //                                ProductName = r.Category,
        //                                ProductID = obj.RoomCatCode,
        //                                PropertySource = obj.PropertySource,
        //                                ConfirmationNo = obj.ConfirmationNo,
        //                                RoomNo = string.IsNullOrEmpty(obj.RoomNum) ? "" : obj.RoomNum // Check if RoomNum is null or empty
        //                            }).ToList();


        //            // Fetch blocked rooms
        //            var blockedRooms = (from br in HDE.BlockedRooms
        //                                join rc in HDE.RoomCategories on br.CateId equals rc.Id into categories
        //                                from c in categories.DefaultIfEmpty()
        //                                where br.PropertyId == propertyID && br.Status == true
        //                                orderby br.StartDate
        //                                select new Events
        //                                {
        //                                    EventID = br.Id,
        //                                    EventName = "Blocked",
        //                                    StartDate = br.StartDate,
        //                                    EndDate = br.EndDate,
        //                                    BookingStatus = "Blocked",
        //                                    ImageType = "",
        //                                    Url = "",
        //                                    ProductName = c.Category,
        //                                    ProductID = br.CateId,
        //                                    PropertySource = "System",
        //                                    ConfirmationNo = "",
        //                                    RoomNo = br.RoomNum //dr["Room_No"].ToString()
        //                                }).ToList();


        //            // After fetching the data, convert RoomNo to string if necessary
        //            foreach (var blockedRoom in blockedRooms)
        //            {
        //                blockedRoom.RoomNo = blockedRoom.RoomNo?.ToString() ?? "";
        //            }


        //            // Combine bookings and blocked rooms
        //            bookings.AddRange(blockedRooms);

        //            return new JsonResult { Data = bookings, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //    }
        //}


        public JsonResult GetEvents(int? propertyID)
        {
            DataTable dates1 = new DataTable();
            dates1.Columns.Add("RoomNum", typeof(string));
            dates1.Columns.Add("CheckIn", typeof(DateTime));
            dates1.Columns.Add("CheckOut", typeof(DateTime));
            dates1.Columns.Add("ThemeColor", typeof(string));
            dates1.Columns.Add("Booked", typeof(string));

            DataTable dates = new DataTable();
            dates.Columns.Add("CheckIn", typeof(DateTime));
            dates.Columns.Add("CheckOut", typeof(DateTime));

            List<string> ColumnName = new List<string>();
            ColumnName.Add("Rooms");
            try
            {

                using (HotelDBEntities dc = new HotelDBEntities())
                {
                    var maxDate = dc.Bookings.Where(s => s.CheckOut == dc.Bookings.Max(x => x.CheckOut) && s.HotelCode == propertyID).FirstOrDefault();

                    if (maxDate == null)
                    {
                        return new JsonResult { Data = "No record found", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }

                    for (DateTime date = DateTime.Today; date <= maxDate.CheckOut; date = date.AddDays(1))
                    {
                        ColumnName.Add(date.ToShortDateString());
                    }


                    var roomNo = dc.Bookings.Where(p => p.HotelCode == propertyID).Select(p => p.RoomNum).Distinct();

                    foreach (var itm in roomNo)
                    {
                        var booking = dc.Bookings.Where(c => c.RoomNum == itm && c.HotelCode == propertyID).Select(p => p).ToList();

                        for (int i = 0; i <= booking.Count - 1; i++)
                        {

                            for (DateTime dt = Convert.ToDateTime(booking[i].CheckIn); dt <= booking[i].CheckOut; dt = dt.AddDays(1))
                            {
                                DataRow row = dates1.NewRow();
                                row["RoomNum"] = itm;
                                row["CheckIn"] = dt;
                                row["CheckOut"] = booking[i].CheckOut;
                                row["ThemeColor"] = booking[i].ThemeColor;
                                row["Booked"] = booking[i].BookingStatus;

                                dates1.Rows.Add(row);
                            }
                        }
                    }

                    DataTable dts = new DataTable(); // ConvertColumnsAsRows(dates);

                    ColumnName = ColumnName.Distinct().ToList();

                    foreach (var itm in ColumnName)
                    {
                        dts.Columns.Add(Convert.ToString(itm));
                    }

                    foreach (var itm in roomNo)
                    {
                        DataRow row = dts.NewRow();
                        row["Rooms"] = itm;
                        dts.Rows.Add(row);
                    }

                    // int dtsRowCount = dts.Rows.Count - 1;
                    //  int ro = 0;

                    for (int c = 1; c <= dts.Columns.Count - 1; c++)
                    {
                        //if (ro == dtsRowCount)
                        //{
                        //    break;
                        //}

                        foreach (DataRow row1 in dts.Rows)
                        {
                            var rowExist = dates1.AsEnumerable().Where(s => s.Field<DateTime>("CheckIn").ToShortDateString() == Convert.ToDateTime(dts.Columns[c].ColumnName.ToString()).ToShortDateString() &&
                            s.Field<string>("RoomNum") == row1[0].ToString() && (s.Field<string>("Booked") == "Booked" || s.Field<string>("Booked") == "CheckIn")).FirstOrDefault();

                            if (rowExist != null)
                            {
                                row1[c] = "red";
                            }
                            else
                            {
                                row1[c] = "green";
                            }
                        }
                        //for (int row = 0; row <= dates1.Rows.Count - 1; row++) //foreach (DataRow row in dates1.Rows)
                        //{
                        //    if (dts.Columns[c].ColumnName.ToString() == Convert.ToDateTime(dates1.Rows[row]["CheckIn"]).ToShortDateString() && dts.Rows[ro][0].ToString() == dates1.Rows[row]["RoomNum"].ToString())
                        //    {
                        //        dts.Rows[ro][c] = "red";
                        //    }
                        // ro = ro + 1;
                        //else if (dts.Columns[c].ColumnName.ToString() != Convert.ToDateTime(row["CheckIn"]).ToShortDateString() && dts.Rows[ro][0].ToString() == row["RoomNum"].ToString())
                        //{                               
                        //        dts.Rows[ro][c] = "green";
                        //}
                        //}

                    }

                    var events = DataTableToJSONWithJavaScriptSerializer(dts); // dts.AsEnumerable().ToList();

                    return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                //using (HotelDBEntities dc = new HotelDBEntities())
                //{

                //    var events = dc.Bookings.ToList();
                //    return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                //}
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        public string DataTableToJSONWithJavaScriptSerializer(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }


        public DataTable ConvertColumnsAsRows(DataTable dt)
        {
            DataTable dtnew = new DataTable();
            //Convert all the rows to columns
            for (int i = 0; i <= dt.Rows.Count; i++)
            {
                dtnew.Columns.Add(Convert.ToString(i));
            }
            DataRow dr;
            // Convert All the Columns to Rows
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                dr = dtnew.NewRow();
                dr[0] = dt.Columns[j].ToString();
                for (int k = 1; k <= dt.Rows.Count; k++)
                    dr[k] = dt.Rows[k - 1][j];
                dtnew.Rows.Add(dr);
            }
            return dtnew;
        }
    }

    public class Events
    {
        public int? EventID { get; set; }
        public string EventName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string BookingStatus { get; set; }
        public string ImageType { get; set; }
        public string Url { get; set; }
        public string ProductName { get; set; }
        public int? ProductID { get; set; }
        public string PropertySource { get; set; }
        public string ConfirmationNo { get; set; }
        public string RoomNo { get; set; }
    }
}