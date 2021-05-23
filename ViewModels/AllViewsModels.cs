using HouseRentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseRentManagement.ViewModels
{
    public class AllViewsModels
    {
        public IEnumerable<Customer_Data> GetCustomers { get; set; }
        public IEnumerable<Seller_Data> GetSellers { get; set; }
        public IEnumerable<Property_Details> GetProperties { get; set; }
        public IEnumerable<Booking_Details> GetBookings { get; set; }
    }
}