using HouseRentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseRentManagement.ViewModels
{
    public  class SellerViewModel
    {
        public IEnumerable<Seller_Data> SellerData { get; set; }
        public IEnumerable<Property_Details> PropertyDetails { get; set; }
    }
}