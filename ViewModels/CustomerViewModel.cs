using HouseRentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseRentManagement.ViewModels{
    public class CustomerViewModel
    {
        public IEnumerable<Customer_Data> CustomerData { get; set; }
        public IEnumerable<Property_Details> PropertyDetails { get; set; }
    }
}