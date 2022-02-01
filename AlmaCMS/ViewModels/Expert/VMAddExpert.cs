using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels.Expert
{
    public class VMAddExpert
    {
        [Required]
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

        [Required(ErrorMessage = "*")]
        public string Name { get; set; }

        public string Family { get; set; }
        [Required(ErrorMessage = "*")]
        public string NationalCode { get; set; }
        public string BirthDate { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }

        public string Tel { get; set; }
        [Required]
        [Phone]
        public string Mobile { get; set; }
        public string Descriptions { get; set; }


    }
}