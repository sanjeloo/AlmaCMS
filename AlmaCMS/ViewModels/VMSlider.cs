using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMSlider
    {
        [Display(Name = "آیدی")]
        public int id { get; set; }


        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا عنوان را وارد کنید.")]
        public string Title { get; set; }


        [Display(Name = "توضیحات")]
        public string Description { get; set; }


        [Display(Name = "لینک مستقیم")]
        [Required(ErrorMessage = "لطفا آدرس لینک را وارد کنید.")]
        public string ActionUrl { get; set; }


        [Display(Name = "عکس")]
        [Required(ErrorMessage = "لطفا عکس را وارد کنید.")]
        public string Image { get; set; }


        [Display(Name = "اولویت")]
        [Required(ErrorMessage = "لطفا میزان الویت را مشخص کنید.")]
        public int Priority { get; set; }


        [Display(Name = "وضعیت")]
        public bool Active { get; set; }

    }
}