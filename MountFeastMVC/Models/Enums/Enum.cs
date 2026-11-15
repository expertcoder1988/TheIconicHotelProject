using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MountFeastMVC.Models
{
    public static class GlobalVariables
    {
        public static IDictionary<int, string> Days = new Dictionary<int, string>()
        {
            {1,"Monday"},
            {2,"Tuesday"},
            {3,"Wednesday"},
            {4,"Thursday" },
            {5,"Friday"},
            {6,"Saturday" },
            {7,"Sunday" }
       };


        public static IDictionary<int, string> Months = new Dictionary<int, string>()
        {
            {1, "January"},
            {2, "February"},
            {3, "March"},
            {4, "April"},
            {5, "May"},
            {6, "June"},
            {7, "July"},
            {8, "August"},
            {9, "September"},
            {10,"October"},
            {11,"November"},
            {12,"December" }
       };

        public static IDictionary<int, string> BookingType = new Dictionary<int, string>()
        {
            {1, "Lodging (Booked per Night)" },
            {2, "Activity (Booked per Day" }
        };

        //public static IDictionary<int, string> PropertyType = new Dictionary<int, string>()
        //{
        //    {1 ,"Apartment"},
        //    {2 ,"Bed and Breakfast"},
        //    {3 ,"Cabin"},
        //    {4 ,"Guesthouse"},
        //    {5 ,"Hostel"},
        //    {6 ,"Hotel"},
        //    {7 ,"House"},
        //    {8 ,"Villa"},
        //    {9 ,"Aparthotel"},
        //    {10 ,"Barn"},
        //    {11 ,"Boat"},
        //    {12 ,"Boutique Hotel"},
        //    {13 ,"Building"},
        //    {14 ,"Bungalow"},
        //    {15 ,"Camping"},
        //    {16 ,"Caravan"},
        //    {17 ,"Casa Particular"},
        //    {18 ,"Castle"},
        //    {19 ,"Chacara"},
        //    {20 ,"Chalet"},
        //    {21 ,"Chateau"},
        //    {22 ,"Condo"},
        //    {23 ,"Cottage"},
        //    {24 ,"Cycladic House"},
        //    {25 ,"Estate"},
        //    {26 ,"Farmhouse"},
        //    {27 ,"Heritage Hotel"},
        //    {28 ,"House Boat"},
        //    {29 ,"Lodge"},
        //    {30 ,"Loft"},
        //    {31 ,"Mas"},
        //    {32 ,"Mill"},
        //    {33 ,"Mobile Home"},
        //    {34 ,"Recreational Vehicle"},
        //    {35 ,"Resort"},
        //    {36 ,"Riad" },
        //    {37 ,"Serviced Apartment" },
        //    {38 ,"Studio"},
        //    {39 ,"Tent"},
        //    {40 ,"Tour"},
        //    {41 ,"Tower"},
        //    {42 ,"Townhome" },
        //    {43 ,"Tree House" },
        //    {44 ,"Yacht" }
        //};

        public static List<DDLOptions> PropertyType = new List<DDLOptions>()
            {
            new DDLOptions {Id=1  ,propertyType="Apartment"},
            new DDLOptions {Id=2  ,propertyType="Bed and Breakfast"},
            new DDLOptions {Id=3  ,propertyType="Cabin"},
            new DDLOptions {Id=4  ,propertyType="Guesthouse"},
            new DDLOptions {Id=5  ,propertyType="Hostel"},
            new DDLOptions {Id=6  ,propertyType="Hotel"},
            new DDLOptions {Id=7  ,propertyType="House"},
            new DDLOptions {Id=8  ,propertyType="Villa"},
            new DDLOptions {Id=9  ,propertyType="Aparthotel"},
            new DDLOptions {Id=10 ,propertyType="Barn"},
            new DDLOptions {Id=11 ,propertyType="Boat"},
            new DDLOptions {Id=12 ,propertyType="Boutique Hotel"},
            new DDLOptions {Id=13 ,propertyType="Building"},
            new DDLOptions {Id=14 ,propertyType="Bungalow"},
            new DDLOptions {Id=15 ,propertyType="Camping"},
            new DDLOptions {Id=16 ,propertyType="Caravan"},
            new DDLOptions {Id=17 ,propertyType="Casa Particular"},
            new DDLOptions {Id=18 ,propertyType="Castle"},
            new DDLOptions {Id=19 ,propertyType="Chacara"},
            new DDLOptions {Id=20 ,propertyType="Chalet"},
            new DDLOptions {Id=21 ,propertyType="Chateau"},
            new DDLOptions {Id=22 ,propertyType="Condo"},
            new DDLOptions {Id=23 ,propertyType="Cottage"},
            new DDLOptions {Id=24 ,propertyType="Cycladic House"},
            new DDLOptions {Id=25 ,propertyType="Estate"},
            new DDLOptions {Id=26 ,propertyType="Farmhouse"},
            new DDLOptions {Id=27 ,propertyType="Heritage Hotel"},
            new DDLOptions {Id=28 ,propertyType="House Boat"},
            new DDLOptions {Id=29 ,propertyType="Lodge"},
            new DDLOptions {Id=30 ,propertyType="Loft"},
            new DDLOptions {Id=31 ,propertyType="Mas"},
            new DDLOptions {Id=32 ,propertyType="Mill"},
            new DDLOptions {Id=33 ,propertyType="Mobile Home"},
            new DDLOptions {Id=34 ,propertyType="Recreational Vehicle"},
            new DDLOptions {Id=35 ,propertyType="Resort"},
            new DDLOptions {Id=36 ,propertyType="Riad" },
            new DDLOptions {Id=37 ,propertyType="Serviced Apartment" },
            new DDLOptions {Id=38 ,propertyType="Studio"},
            new DDLOptions {Id=39 ,propertyType="Tent"},
            new DDLOptions {Id=40 ,propertyType="Tour"},
            new DDLOptions {Id=41 ,propertyType="Tower"},
            new DDLOptions {Id=42 ,propertyType="Townhome" },
            new DDLOptions {Id=43 ,propertyType="Tree House" },
            new DDLOptions {Id=44 ,propertyType="Yacht" }
            }.ToList();

    }

    public class DDLOptions
    {
                
        public int Id { get; set; }
        public string propertyType { get; set; }

    }
}