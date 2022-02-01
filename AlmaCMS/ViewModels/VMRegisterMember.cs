using AlmaCMS.CustomAttr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMRegisterMember
    {
 
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage="*")]
        public string Name { get; set; }
          [MinLength(10,ErrorMessage="کد ملی باید 10 رقم باشد")]
          [MaxLength(10, ErrorMessage = "کد ملی باید 10 رقم باشد")]
        [Required(ErrorMessage="کد ملی الزامی میباشد")]
        public string NationalCode { get; set; }
         [Required(ErrorMessage = "*")]
         [Phone]
        public string MobileNumber { get; set; }
                [Required(ErrorMessage = "*")]
        public string Phone { get; set; }
                [Required(ErrorMessage = "*")]
        public string PostalCode { get; set; }
                [Required(ErrorMessage = "*")]
        public int StateID  { get; set; }
                [Required(ErrorMessage = "*")]
        public string City { get; set; }
                [Required(ErrorMessage = "*")]
        public string Address { get; set; }
                [Required(ErrorMessage = "*")]
                [Range(1, float.MaxValue, ErrorMessage = "لطفا نحوه آشنایی را انتخاب نمایید")]
        public int  introductionId { get; set; }
                [Required(ErrorMessage = "*")]
        public int RegisterType { get; set; }
            [Required(ErrorMessage = "*")]
                public DateTime BirthDate { get; set; }
        
                public int ReagentCode { get; set; }

    }
}