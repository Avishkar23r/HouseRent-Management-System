using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HouseRentManagement.Models
{
    public class HouseRentContext:DbContext
    {
        public DbSet<Customer_Data> CustomerDetail { get; set; }
        public DbSet<Seller_Data> SellerDetail { get; set; }
        public DbSet<Property_Details> PropertyDetails { get; set; }
        public DbSet<Booking_Details> BookingDetails { get; set; }

    }
}