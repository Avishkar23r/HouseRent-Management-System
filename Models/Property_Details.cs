using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseRentManagement.Models
{
    public class Property_Details
    {
        public int Id { get; set; }
        public int SellerID { get; set; }
        public string Current_Status { get; set; }
        [Required(ErrorMessage = "Property Name is Required")]
        [DisplayName("Property Name")]

        public string Property_Name{ get; set; }
        [Required(ErrorMessage = "Property Type is Required")]
        [DisplayName("Property Type")]


        public string Property_Type{ get; set; }
        [Required(ErrorMessage = "Property Locality is Required")]
        [DisplayName("Property Locality")]


        public string Property_Locality { get; set; }
        [Required(ErrorMessage = "City is Required")]
        [RegularExpression(@"^[a-zA-Z]+(?:[\s-][a-zA-Z]+)*$", ErrorMessage = "Not a valid City")]
        [DisplayName("Property City")]



        public string Property_City { get; set; }
        [Required(ErrorMessage = "State is Required")]
        [RegularExpression(@"^[a-zA-Z]+(?:[\s-][a-zA-Z]+)*$", ErrorMessage = "Not a valid State")]
        [DisplayName("Property State")]


        public string Property_State { get; set; }
        [Required(ErrorMessage = "PinCode is Required")]
        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Not a Valid Pincode")]
        [DisplayName("Property Pincode")]
        //[DataType(DataType.PostalCode)]


        public int Property_Pincode { get; set; }
        [Required(ErrorMessage = "Construction Status is Required")]
        [DisplayName("Status")]

        public string Construction_Status { get; set; }
        [Required(ErrorMessage = "It is Required filed")]

        public string BHK { get; set; }
        [Required(ErrorMessage = "It is Required filed")]

        public string Balcony { get; set; }
        [Required(ErrorMessage = "It is Required filed")]

        public string Floors { get; set; }
        [Required(ErrorMessage = "It is Required filed")]
        [DisplayName("Furnish Type")]

        public string Furnish_Type { get; set; }
        [Required(ErrorMessage = "It is Required filed")]
        [DisplayName("Parking Facilities")]

        public string Parking_Facilities { get; set; }
        [Required(ErrorMessage = "It is Required filed")]
        [DisplayName("Preferred Tenant")]

        public string Preferred_Tenant { get; set; }
        [Required(ErrorMessage = "Property Age is Required")]
        [RegularExpression(@"^([1-9]|1[0-5])$", ErrorMessage = "Property Age must be within 1 to 15 years")]
        [DisplayName("Property Age")]

        public int Property_Age{ get; set; }
        [Required(ErrorMessage = "It is Required filed")]

        public string Facing { get; set; }
        [Required(ErrorMessage = "Size of the Property is Required")]
        [DisplayName("Property Size")]

        public int Property_Size { get; set; }
        [Required(ErrorMessage = "Monthly Rent is Required")]
        [RegularExpression(@"^([1-9][0-9][0-9][0-9][0-9]|[4-9][0-9][0-9][0-9])$", ErrorMessage = "Monthly Rent must within 5000 -99999")]
        [DisplayName("Monthly Rent")]

        public int Monthly_Rent { get; set; }
        public string Description { get; set; }
        [Required]
        [DisplayName("Image of the Front View")]

        public string Front_View_img { get; set; }
        [Required]
        [DisplayName("Image of Hall")]

        public string Hall_img { get; set; }
        [Required]
        [DisplayName("Image of Bedroom")]

        public string Bedroom_img { get; set; }
        [Required]
        [DisplayName("Image of Kitchen")]

        public string Kitchen_img { get; set; }
    }
}