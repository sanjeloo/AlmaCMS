using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMState
    {
        [Display(Name = "آیدی")]
        public int id { get; set; }


        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا نام استان را وارد کنید.")]
        public string Title { get; set; }


        [Display(Name = "توضیحات")]
        public string Comments { get; set; }
    }
}