using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseRentManagement.Models
{
    public class Booking_Details
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int CustomerId { get; set; }
        public int SellerID { get; set; }
        public string Booking_Status { get; set; }
        public string Booked_Date { get; set; }
    }
}