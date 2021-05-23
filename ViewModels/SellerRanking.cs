using HouseRentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseRentManagement.ViewModels
{
    public class SellerRanking
    {
        public IEnumerable<TopSeller> TopSellers { get; set; }
        public IEnumerable<Seller_Data> SellerDetails { get; set; }

    }
    public class TopSeller
    {
        public int SellerId { get; set; }
        public int HouseSold { get; set; }
    }
}