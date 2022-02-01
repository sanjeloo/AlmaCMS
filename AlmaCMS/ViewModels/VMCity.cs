using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMCity
    {
        [Display(Name = "آیدی")]
        public int Id { get; set; }


        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا نام شهر را وارد کنید.")]
        public string Title { get; set; }


        [Display(Name = "توضیحات")]
        public string Comments { get; set; }


        [Display(Name = "آیدی استان")]
        public int StateID { get; set; }
    }
}