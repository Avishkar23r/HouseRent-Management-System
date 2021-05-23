using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseRentManagement.Models
{
    public class Seller_Data
    {
           public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mobile Number is Required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType. Password)]
        [MinLength(8)]
        [MaxLength(20)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Picture is Required")]
        public string ProfilePic { get; set; }

        [Required(ErrorMessage = "State is Required")]
        public string State { get; set; }
    }
}