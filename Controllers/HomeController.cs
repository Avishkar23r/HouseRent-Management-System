using HouseRentManagement.Models;
using HouseRentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HouseRentManagement.Controllers
{
    public class HomeController : Controller
    {
        private HouseRentContext _context;
        public HomeController()
        {
            _context = new HouseRentContext();
        }

        public ActionResult Index()
        {
            return View(_context.PropertyDetails.ToList());
        }
        public ActionResult CreateCustomer()
        {
            return View();
        }
        public ActionResult CreateSeller()
        {
            return View();
        }
        class UserType
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }
        //methos for login dropdown
        public object UserTypes()
        {
            var users = new List<UserType>()
            { new UserType() { Id = 1, Value = "CUSTOMER" },
              new UserType() { Id = 2, Value = "SELLER" } };
            return users;
        }
        [HttpPost]
        public ActionResult RegisterSeller(Seller_Data seller_Data, HttpPostedFileBase sellerimage)
        {
            if (_context.SellerDetail.Where(c => c.MobileNumber == seller_Data.MobileNumber).Any() ||
                _context.SellerDetail.Where(c => c.Email == seller_Data.Email).Any())
            {
                ModelState.AddModelError("MobileNumber", "The crediential with this Mail or Number are already exists");
                ModelState.AddModelError("Email", "The crediential with this Mail or Number are already exists");
                return View("CreateSeller", seller_Data);
            }
            if (sellerimage != null)
            {
                string extension = Path.GetExtension(sellerimage.FileName);
                string filename = seller_Data.Name + DateTime.Now.ToString("-ddMMyy_hhmmss")  + extension;
                string filepath = Path.Combine(Server.MapPath("~/Images/SellerImages/"), filename);
                sellerimage.SaveAs(filepath);
                seller_Data.ProfilePic = "~/Images/SellerImages/" + filename;
            }
            else
            {
                ModelState.AddModelError("ProfilePic", "Photo required");
                return View("CreateSeller",seller_Data);
            }
            
            _context.SellerDetail.Add(seller_Data);
            _context.SaveChanges();
            TempData["SuccessFullRegister"] = "Your data has been successfully register!!! Please login once to continue";
            ViewBag.UserTypes = UserTypes();
            return View("Login");
        }
        [HttpPost]
        public ActionResult RegisterCustomer(Customer_Data customer_Data, HttpPostedFileBase customerimage)
        {
            if (_context.SellerDetail.Where(c => c.MobileNumber == customer_Data.MobileNumber).Any() ||
                _context.SellerDetail.Where(c => c.Email == customer_Data.Email).Any())
            {
                ModelState.AddModelError("MobileNumber", "The crediential with this Mail or Number are already exists");
                ModelState.AddModelError("Email", "The crediential with this Mail or Number are already exists");
                return View("CreateSeller", customer_Data);
            }
            if (customerimage != null)
            {
                string extension = Path.GetExtension(customerimage.FileName);
                string filename = customer_Data.Name + DateTime.Now.ToString("-ddMMyy_hhmmss") + extension;
                string filepath = Path.Combine(Server.MapPath("~/Images/CustomerImages/"), filename);
                customerimage.SaveAs(filepath);
                customer_Data.ProfilePic = "~/Images/CustomerImages/" + filename;
                
            }
            else
            {
                ModelState.AddModelError("ProfilePic", "Photo required");
                return View("CreateCustomer", customer_Data);
            }
            _context.CustomerDetail.Add(customer_Data);
            _context.SaveChanges();
            TempData["SuccessFullRegister"] = "Your data has been successfully register!!! Please login once to continue";
            ViewBag.UserTypes = UserTypes();
            return View("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.UserTypes = UserTypes();
            return View("Login");

        }

        [HttpPost]
        public ActionResult LoginUser(LoginModel loginData)
        {
          
            if (loginData.UserType == "2")
            {
                if (_context.SellerDetail.Where(c => c.Email == loginData.UserMail).Any() &&
                    _context.SellerDetail.Where(c => c.Password == loginData.Password).Any())
                {
                    Session["sellerMail"] = loginData.UserMail;
                    return RedirectToAction("Index","Seller");

                }
                else
                {
                    ModelState.AddModelError("Password", "Your Credential are Invalid");
                    ViewBag.UserTypes = UserTypes();
                    return View("Login");
                }
            }
            else
            {
                if (_context.CustomerDetail.Where(c => c.Email == loginData.UserMail).Any() &&
                   _context.CustomerDetail.Where(c => c.Password == loginData.Password).Any())
                {
                    Session["customerMail"] = loginData.UserMail;
                    return RedirectToAction("CustomerHome","Customer");

                }
                else
                {
                    ModelState.AddModelError("Password", "Your Credential are Invalid");
                    ViewBag.UserTypes = UserTypes();
                    return View("Login");
                }
            }
        }
        public ActionResult Search(string searchText)
        {
            var HouseBasedonNames = _context.PropertyDetails.ToList().Where(C => C.Property_Name.ToLower().Contains(searchText.ToLower()));
            var HouseBasedonCity = _context.PropertyDetails.ToList().Where(C => C.Property_City.ToLower().Contains(searchText.ToLower()));
            var FilteredHouses = new List<Property_Details>();
             if (HouseBasedonCity is null && HouseBasedonCity is null)
            {
                FilteredHouses.AddRange(_context.PropertyDetails.ToList());    
            }
            else if (HouseBasedonNames != null && HouseBasedonCity!= null)
            {
                FilteredHouses.AddRange(HouseBasedonCity);
                FilteredHouses.AddRange(HouseBasedonNames);
            }
            else if (HouseBasedonCity is null && HouseBasedonNames!=null)
            {
                FilteredHouses.AddRange(HouseBasedonNames);
            }
            else if(HouseBasedonNames is null && HouseBasedonCity!=null)
            {
                FilteredHouses.AddRange(HouseBasedonCity);
            }
            
      
            return View("Index",FilteredHouses);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            ViewBag.UserTypes = UserTypes();
            return View("Login");
        }


    }
}