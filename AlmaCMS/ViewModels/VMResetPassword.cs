using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMResetPassword
    {

        [Required(ErrorMessage="شماره موبایل را وارد نمایید")]
        [Phone(ErrorMessage="شماره موبال وارد شده صحیح نمی باشد")]
        public string MobileNumber { get; set; }
    }
}