using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlmaCMS.ViewModels
{
    public class VMArticle
    {
        public int id { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "عنوان")]
        public string Title { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "تصویر")]
        public string Image { get; set; }


        [Required(ErrorMessage = "*")]
  
        [Display(Name = "خلاصه مقاله")]
        public string ShortDesc { get; set; }


        [Required(ErrorMessage = "*")]
        [AllowHtml]
        [UIHint("html")]
        [Display(Name = "محتوای مقاله")]
        public string ArticleContent { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "تاریخ درج مقابه")]
        public DateTime DateInsert { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "فعال")]
        public bool Active { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "کلمات کلیدی")]
        public string Keyword { get; set; }

        public string persianDate { get; set; }
        public int ArticleGroupID { get; set; }
    }
}