using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MountFeastMVC.Models
{
    public class RoomDetails
    {
        public int? Id { get; set; }
        public int? RoomCode { get; set; }
        public int? RoomCategoryId { get; set; }
        public string RoomCategory { get; set; }
        public string RoomDescription { get; set; }
        public int? PropertyId { get; set; }
        public string BedType { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public int? NoOfRooms { get; set; }
        public int? Capacity { get; set; }
        public int? Adult { get; set; }
        public int? NoOfChild { get; set; }
        public int? ChildAge { get; set; }
        public string ShortCode { get; set; }
        public int? MinimumPrice { get; set; }
        public int? MinimumStay { get; set; }
        public int? MaximumStay { get; set; }
        public int? Size { get; set; }
        public string Features { get; set; }
    }

    public class Pictures
    {
        public string Picture { get; set; }
    }
}