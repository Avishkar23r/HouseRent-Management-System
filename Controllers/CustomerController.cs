using HouseRentManagement.Models;
using HouseRentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseRentManagement.Controllers
{
    public class CustomerController : Controller
    {
        private HouseRentContext _context;
        public CustomerController()
        {
            _context = new HouseRentContext();
        }
        // GET: Customer

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CustomerHome(int? Id)
        {
            if (Session["customerMail"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var email = Session["customerMail"];
            var Customers = _context.CustomerDetail.ToList().Where(C => C.Email.Equals(email));
            int CusID = Customers.Single().Id;
            var Booking = _context.BookingDetails.ToList().Where(C => C.CustomerId == CusID);
            var AvailableProperty = new List<Property_Details>();
            if (Id == 2)
            {
                AvailableProperty.AddRange((List<Property_Details>)TempData["Filteredhouses"]);
                Id = 0;
                if (AvailableProperty == null)
                {
                    TempData["NoResult"] = "No Results found";
                }
            }
            else
            {
                AvailableProperty.AddRange(_context.PropertyDetails.ToList().Where(C => C.Current_Status.Equals("Available")));
            }
            var PropertyToShow = new List<Property_Details>();
            List<int> DeclinedIds = new List<int>();
            List<int> PropertyID = new List<int>();

            foreach (var item in Booking)
            {
                if (item.Booking_Status.Equals("Declined") || item.Booking_Status.Equals("Requested"))
                {
                    DeclinedIds.Add(item.PropertyId);

                }
            }
            if (!DeclinedIds.Any())
            {
                foreach (var Property in AvailableProperty)
                {
                    PropertyToShow.Add(Property);
                }
            }
            else
            {
                foreach (var Property in AvailableProperty)
                {
                    PropertyID.Add(Property.Id);
                }
                var FilterID = PropertyID.Except(DeclinedIds);
                foreach (var Property in AvailableProperty)
                {
                    foreach (var _filterid in FilterID)
                    {
                        if (Property.Id == _filterid)
                        {
                            PropertyToShow.Add(Property);
                        }
                    }
                }
            }
            var HomeModels = new CustomerViewModel
            {
                CustomerData = _context.CustomerDetail.ToList().Where(c => c.Email.Equals(email)),
                PropertyDetails = PropertyToShow.Distinct().ToList()
            };

            return View(HomeModels);
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
            else if (HouseBasedonNames != null && HouseBasedonCity != null)
            {
                FilteredHouses.AddRange(HouseBasedonCity);
                FilteredHouses.AddRange(HouseBasedonNames);
            }
            else if (HouseBasedonCity is null && HouseBasedonNames != null)
            {
                FilteredHouses.AddRange(HouseBasedonNames);
            }
            else if (HouseBasedonNames is null && HouseBasedonCity != null)
            {
                FilteredHouses.AddRange(HouseBasedonCity);
            }
            var HousesSearched = new List<Property_Details>();
            foreach (var property in FilteredHouses)
            {
                if (property.Current_Status.Equals("Available"))
                {
                    HousesSearched.Add(property);
                }
            }

            TempData["Filteredhouses"] = HousesSearched;
            return RedirectToAction("CustomerHome", new { id = 2 });
        }

        public ActionResult CustomerEdit(int? id)

        {
            if (Session["customerMail"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var EditData = _context.CustomerDetail.SingleOrDefault(c => c.Id == id);
            Session["imgpath"] = EditData.ProfilePic;
            return View("Customer_Edit", EditData);
        }
        public ActionResult EditData(Customer_Data EditData, HttpPostedFileBase imageEdit)
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
                string filepath = Path.Combine(Server.MapPath("~/Images/CustomerImages/"), filename);
                imageEdit.SaveAs(filepath);
                EditData.ProfilePic = "~/Images/CustomerImages/" + filename;
                _context.Entry(EditData).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                string oldImage = Request.MapPath(Session["imgpath"].ToString());
                if (System.IO.File.Exists(oldImage))
                {
                    System.IO.File.Delete(oldImage);

                }
            }
            return RedirectToAction("CustomerHome");
        }
        public ActionResult ViewDetails(int? id)
        {
            var Property = _context.PropertyDetails.ToList().Where(c => c.Id == id).FirstOrDefault();
            var model = new SellerViewModel
            {
                PropertyDetails = _context.PropertyDetails.ToList().Where(c => c.Id == id),
                SellerData = _context.SellerDetail.ToList().Where(c => c.Id == Property.SellerID),
            };
            var customer = _context.CustomerDetail.ToList().Where(C => C.Email.Equals(Session["customerMail"]));
            ViewData["CustomerId"] = customer.Single().Id;
            return View("Customer_ViewDetails", model);
        }
        [Route("Customer/RequestingHouse/{Id}/{CustomerID}")]
        public ActionResult RequestingHouse(int Id, int CustomerID)
        {
            var property = _context.PropertyDetails.Where(c => c.Id == Id);
            var BookingDetail = new Booking_Details
            {
                PropertyId = Id,
                CustomerId = CustomerID,
                Booking_Status = "Requested",
                SellerID = property.FirstOrDefault().SellerID,
                Booked_Date = DateTime.Now.ToString("dd MMMM yyyy")
            };
            _context.BookingDetails.Add(BookingDetail);

            _context.SaveChanges();

            return RedirectToAction("CustomerHome");
        }
        public ActionResult BookingDetails(int id)
        {
            if (Session["customerMail"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var BookingById = _context.BookingDetails.ToList().Where(c => c.CustomerId == id);
            var Bookings = new List<Booking_Details>();
            foreach (var item in BookingById)
            {
                if (item.Booking_Status.Equals("Requested"))
                {
                    Bookings.Add(item);
                }
            }
            var PropertiesBooked = new List<Property_Details>();
            foreach (var value in Bookings)
            {
                PropertiesBooked.Add(_context.PropertyDetails.SingleOrDefault(c => c.Id == value.PropertyId));
            }
            var RequestDetails = new AllViewsModels
            {
                GetSellers = _context.SellerDetail.ToList(),
                GetProperties = PropertiesBooked,
                GetCustomers = _context.CustomerDetail.Where(c => c.Id == id),
                GetBookings = Bookings
            };
            return View("Customer_BookingDetails", RequestDetails);
        }
        public ActionResult BookingHistory(int id)
        {
            if (Session["customerMail"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var BookingById = _context.BookingDetails.ToList().Where(c => c.CustomerId == id);
            var Bookings = new List<Booking_Details>();
            foreach (var item in BookingById)
            {
                if (item.Booking_Status != "Requested")
                {
                    Bookings.Add(item);
                }
            }
            var PropertiesBooked = new List<Property_Details>();
            foreach (var value in Bookings)
            {
                PropertiesBooked.Add(_context.PropertyDetails.SingleOrDefault(c => c.Id == value.PropertyId));
            }
            var RequestDetails = new AllViewsModels
            {
                GetSellers = _context.SellerDetail.ToList(),
                GetProperties = PropertiesBooked,
                GetCustomers = _context.CustomerDetail.Where(c => c.Id == id),
                GetBookings = Bookings
            };
            return View("Customer_BookingHistory", RequestDetails);
        }
        public ActionResult CancelBooking(int? Id)
        {
            var Booking = _context.BookingDetails.SingleOrDefault(C => C.Id == Id);
            int CustomerId = Booking.CustomerId;
            var Property = _context.PropertyDetails.SingleOrDefault(C => C.Id == Booking.PropertyId);
            Property.Current_Status = "Available";
            _context.BookingDetails.Remove(Booking);
            _context.SaveChanges();
            return RedirectToAction("BookingDetails", new { id = CustomerId });
        }
        public ActionResult DeleteBooking(int? id)
        {
            var BookingCancellation = _context.BookingDetails.SingleOrDefault(C => C.Id==id);
            int CustomerID = BookingCancellation.CustomerId;
            var PropertyAlter = _context.PropertyDetails.SingleOrDefault(C => C.Id == BookingCancellation.PropertyId);
            PropertyAlter.Current_Status = "Available";
            _context.BookingDetails.Remove(BookingCancellation);
            _context.SaveChanges();
            return RedirectToAction("BookingHistory",new { id=CustomerID});
        }
        public ActionResult TopSeller()
        {
            var Topseller = from bookingDetails in _context.BookingDetails
                            group bookingDetails by bookingDetails.SellerID into groupid
                            orderby groupid.Count() descending
                            select new TopSeller
                            {
                                SellerId = groupid.Key,
                                HouseSold = groupid.Count(),
                             };
            var Sellerrankings = new SellerRanking
            {
                TopSellers = Topseller,
                SellerDetails = _context.SellerDetail.ToList()
            };
            return View(Sellerrankings);
        }
        public ActionResult Logout()
        {
            return this.RedirectToAction("Logout", "Home");
        }
    }

}