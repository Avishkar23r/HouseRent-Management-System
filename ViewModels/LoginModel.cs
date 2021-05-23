using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseRentManagement.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string UserMail { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(20)]
        public string Password { get; set; }
        [Required]
        public string UserType { get; set; }

    }
   

}