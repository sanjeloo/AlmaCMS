using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMLogin
    {

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "نام کاربری را وارد نمایید")]
        public string Username { get; set; }
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "رمز عبور را وارد نمایید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}