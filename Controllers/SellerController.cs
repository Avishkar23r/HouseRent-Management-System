using HouseRentManagement.Controllers;
using HouseRentManagement.Models;
using HouseRentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseRentManagement.Controllers
{
    public class SellerController : Controller
    {
        // GET: Seller
        private HouseRentContext _context;
        public SellerController()
        {
            _context = new HouseRentContext();
        }
        //method for returning property details
        public Object PropertyDetails()
        {

            if (Session["sellerMail"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            string email = Session["sellerMail"].ToString();
            var GetSeller = _context.SellerDetail.ToList().Where(c => c.Email.Equals(email)).FirstOrDefault();
            Session["sellerID"] = GetSeller.Id;
            var model = new SellerViewModel
            {
                SellerData = _context.SellerDetail.ToList().Where(c => c.Email.Equals(email)),
                PropertyDetails = _context.PropertyDetails.ToList().Where(c => c.SellerID.Equals(Session["sellerID"])),
            };
            return model;
        }
        public ActionResult Index()
        {
            if (Session["sellerMail"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View("Seller_Index", PropertyDetails());
        }
        public ActionResult BookingDetails(int Id)
        {
            var SellerBookings = _context.BookingDetails.ToList().Where(c => c.SellerID == Id);
            var SellerProperties = new List<Property_Details>();
            foreach (var property in SellerBookings)
            {
                SellerProperties.Add(_context.PropertyDetails.Single(c => c.Id == property.PropertyId));
            }
            var SellerBookingDetails = new AllViewsModels()
            {
                GetSellers = _context.SellerDetail.Where(C => C.Id == Id),
                GetProperties = SellerProperties.Distinct(),
                GetBookings = SellerBookings,
                GetCustomers = _context.CustomerDetail.ToList()
            };
            return View("Seller_BookingDetails", SellerBookingDetails);
        }
        public ActionResult BookingHistory(int Id)
        {
            var SellerBookings = _context.BookingDetails.ToList().Where(c => c.SellerID == Id);
            var SellerProperties = new List<Property_Details>();
            foreach (var property in SellerBookings)
            {
                SellerProperties.Add(_context.PropertyDetails.Single(c => c.Id == property.PropertyId));
            }
            var SellerBookingDetails = new AllViewsModels()
            {
                GetSellers = _context.SellerDetail.Where(C => C.Id == Id),
                GetProperties = SellerProperties.Distinct(),
                GetBookings = SellerBookings,
                GetCustomers = _context.CustomerDetail.ToList()
            };
            return View("Seller_BookingHistory", SellerBookingDetails);
        }
        public ActionResult Edit(int? id)
        {
            var EditData = _context.SellerDetail.SingleOrDefault(c => c.Id == id);
            Session["imgpath"] = EditData.ProfilePic;
            return View("Seller_Edit", EditData);
        }
        public ActionResult EditData(Seller_Data EditData, HttpPostedFileBase imageEdit)
        {

            if (imageEdit == null)
            {
                EditData.ProfilePic = Session["imgpath"].ToString();
                _context.Entry(EditData).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                string extension = Path.GetExtension(imageEdit.FileName);
                string filename = EditData.Name + DateTime.Now.ToString("-ddMMyy_hhmmss") + extension;

                string filepath = Path.Combine(Server.MapPath("~/Images/SellerImages/"), filename);
                imageEdit.SaveAs(filepath);
                EditData.ProfilePic = "~/Images/SellerImages/" + filename;
                _context.Entry(EditData).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                string oldImage = Request.MapPath(Session["imgpath"].ToString());
                if (System.IO.File.Exists(oldImage))
                {
                    System.IO.File.Delete(oldImage);

                }
            }
            var Editedmodel = new SellerViewModel
            {
                SellerData = _context.SellerDetail.ToList().Where(c => c.Email.Equals(Session["sellerMail"])),
                PropertyDetails = _context.PropertyDetails.ToList().Where(c => c.SellerID.Equals(Session["sellerID"])),
            };
            return View("Seller_Index", Editedmodel);

        }
        public ActionResult CreateProperty()
        {
            return View("Seller_AddProperty");
        }

        public string ImageName(int order)
        {
            string name = "";
            if (order == 1)
                name = "Home";
            if (order == 2)
                name = "Hall";
            if (order == 3)
                name = "BedRoom";
            if (order == 4)
                name = "Kitchen";
            return name;
        }
        public ActionResult AddProperty(Property_Details details, IEnumerable<HttpPostedFileBase> propertyImage)
        {

            details.SellerID = Convert.ToInt32(Session["sellerID"]);
            details.Current_Status = "Available";
            int order = 1;
            if (propertyImage != null)
            {
                foreach (var item in propertyImage)
                {
                    string extension = Path.GetExtension(item.FileName);
                    string filename = "S" + details.SellerID + "_" + ImageName(order) + DateTime.Now.ToString("-ddMM_HHmmss") + extension;
                    string filepath = Path.Combine(Server.MapPath("~/Images/PropertyImages/"), filename);
                    item.SaveAs(filepath);
                    if (order == 1)
                        details.Front_View_img = "~/Images/PropertyImages/" + filename;
                    if (order == 2)
                        details.Hall_img = "~/Images/PropertyImages/" + filename;
                    if (order == 3)
                        details.Bedroom_img = "~/Images/PropertyImages/" + filename;
                    else
                        details.Kitchen_img = "~/Images/PropertyImages/" + filename;

                    order++;

                }
            }
            else
            {
                return View("Seller_AddProperty");
            }

            _context.PropertyDetails.Add(details);
            _context.SaveChanges();
            return View("Seller_Index", PropertyDetails());
        }
       
        public ActionResult ViewPropertyDetails(int? id)
        {
            var PropertyDetails = _context.PropertyDetails.ToList().SingleOrDefault(c => c.Id.Equals(id));

            return View("Seller_ViewPropertyDetails", PropertyDetails);
        }
        public ActionResult EditProperty(int? id)
        {
            var PropertyDetails = _context.PropertyDetails.SingleOrDefault(c => c.Id == id);

            Session["Frontview"] = PropertyDetails.Front_View_img;
            Session["HallImg"] = PropertyDetails.Hall_img;
            Session["BedroomImg"] = PropertyDetails.Bedroom_img;
            Session["KitchenImg"] = PropertyDetails.Kitchen_img;

            return View("Seller_EditProperty", PropertyDetails);
        }
        public string SavedImage(HttpPostedFileBase item, Property_Details details, int order, string Image)
        {
            string oldImage = Request.MapPath(Image);
            if (System.IO.File.Exists(oldImage))
            {
                System.IO.File.Delete(oldImage);

            }
            string extension = Path.GetExtension(item.FileName);
            string filename = "S" + details.SellerID + "_" + ImageName(order) + DateTime.Now.ToString("-ddMM_HHmmss") + extension;
            string filepath = Path.Combine(Server.MapPath("~/Images/PropertyImages/"), filename);
            item.SaveAs(filepath);

            return filename;
        }

        public ActionResult EditedProperty(Property_Details details, IEnumerable<HttpPostedFileBase> propertyImage, int? id)
        {
            details.SellerID = Convert.ToInt32(Session["sellerID"]);
            details.Current_Status = "Available";
            int order = 1;

            foreach (var item in propertyImage)
            {
                if (order == 1)
                    details.Front_View_img = item is null ? Session["Frontview"].ToString() : "~/Images/PropertyImages/" + SavedImage(item, details, order, Session["Frontview"].ToString());
                else if (order == 2)
                    details.Hall_img = item is null ? Session["HallImg"].ToString() : "~/Images/PropertyImages/" + SavedImage(item, details, order, Session["HallImg"].ToString());
                else if (order == 3)
                    details.Bedroom_img = item is null ? Session["BedroomImg"].ToString() : "~/Images/PropertyImages/" + SavedImage(item, details, order, Session["BedroomImg"].ToString());
                else if(order==4)
                    details.Kitchen_img = item is null ? Session["KitchenImg"].ToString() : "~/Images/PropertyImages/" + SavedImage(item, details, order, Session["KitchenImg"].ToString());

                order++;
            }
            _context.Entry(details).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
            var PropertyDetails = _context.PropertyDetails.SingleOrDefault(c => c.Id == id);
            return View("Seller_ViewPropertyDetails", PropertyDetails);
        }
        void DeleteImage(string image)
        {
            string oldImage = Request.MapPath(image);
            if (System.IO.File.Exists(oldImage))
            {
                System.IO.File.Delete(oldImage);

            }
        }
        public ActionResult DeleteProperty(int? id)
        {
            var PropertyDetail = _context.PropertyDetails.SingleOrDefault(c => c.Id == id);
            DeleteImage(PropertyDetail.Front_View_img);
            DeleteImage(PropertyDetail.Hall_img);
            DeleteImage(PropertyDetail.Bedroom_img);
            DeleteImage(PropertyDetail.Kitchen_img);
            _context.PropertyDetails.Remove(PropertyDetail);
            _context.SaveChanges();
            return View("Seller_Index", PropertyDetails());
        }
       
        public ActionResult AcceptRequest(int Id)
        {
            var BookingDetails = _context.BookingDetails.SingleOrDefault(C => C.Id == Id);
            BookingDetails.Booking_Status = "Accepted";
            var Property = _context.PropertyDetails.SingleOrDefault(C => C.Id == BookingDetails.PropertyId);
            Property.Current_Status = "Not Available";
            _context.SaveChanges();

            return RedirectToAction("BookingDetails", new { id = BookingDetails.SellerID });
        }
        public ActionResult DeclineRequest(int Id)
        {
            var BookingDetails = _context.BookingDetails.SingleOrDefault(C => C.Id == Id);
            BookingDetails.Booking_Status = "Declined";
            var Property = _context.PropertyDetails.SingleOrDefault(C => C.Id == BookingDetails.PropertyId);
            Property.Current_Status = "Available";
            _context.SaveChanges();

            return RedirectToAction("BookingDetails", new { Id = BookingDetails.SellerID });

        }
        public ActionResult Logout()
        {
            return this.RedirectToAction("Logout", "Home");
        }


    }
}