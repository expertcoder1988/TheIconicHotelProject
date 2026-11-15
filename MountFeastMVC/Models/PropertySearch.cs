using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MountFeastMVC.Models
{
    public class PropertySearch
    {
        public int? Id { get; set; }
        public int? RoomCode { get; set; }
        public int? RoomCateCode { get; set; }
        public string RoomCategory { get; set; }
        public string RoomDescription { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public int? Normalprice { get; set; }
        public int? AdultAllowed { get; set; }
        public int? AdultPaxs { get; set; }
        public int? ExtraAdultPrice { get; set; }
        public int? NoOfChildAllowed { get; set; }
        public int? ChildPaxs { get; set; }
        public int? ExtraChildPrice { get; set; }
        public int? ChildAge { get; set; }
        public int? discountedprice { get; set; }
        public int? NoOfRooms { get; set; }
        public int? Capacity { get; set; }
        public DateTime? dateCreated { get; set; }
        public int? NoofDays { get; set; }
        public string imgURL { get; set; }
        public string BedType { get; set; }
        public int? TaxAmount { get; set; }
        public int? TaxPercent { get; set; }
        public int? TotalAmount { get; set; }
        public int? DiscountedAmount { get; set; }
        public int? PropertyId { get; set; }
        public int? NoOfRoomsRequired { get; set; }
        public int? Adult { get; set; }
        public int? NoOfChild { get; set; }
        public string ShortCode { get; set; }
        public int? MinimumPrice { get; set; }
        public int? MinimumStay { get; set; }
        public int? MaximumStay { get; set; }
        public int? Size { get; set; }
        public string Features { get; set; }
        public int? rateplanId { get; set; }
        public string Name { get; set; }
        public bool? WantStayMinimumLength { get; set; }
        public Int16? StayMinimumLength { get; set; }
        public int? CancellationPolicy { get; set; }
        public bool? WantToIncludeMeals { get; set; }
        public bool? Breakfast { get; set; }
        public bool? Lunch { get; set; }
        public bool? Dinner { get; set; }
        public bool? DaysBeforeCheckIn { get; set; }
        public Int16? NumberOfDaysBeforeCheckIn { get; set; }
        public Int16? Discount { get; set; }
        public string RoomTypes { get; set; }
        public int? RoomType { get; set; }
        public string VideoLink { get; set; }
    }
}