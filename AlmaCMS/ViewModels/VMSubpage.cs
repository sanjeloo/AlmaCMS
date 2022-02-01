using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlmaCMS.ViewModels
{
    public class VMSubpage
    {
        public int id { get; set; }
        public int PageId { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "عنوان")]
        public string Title { get; set; }
         [Required(ErrorMessage = "*")]
        public string ShortDescriptions { get; set; }
         [Required(ErrorMessage = "*")]
        public string Kyywords { get; set; }
         [Required(ErrorMessage = "*")]
         [AllowHtml]
         [UIHint("html")]
        public string PageContent { get; set; }
         [Required(ErrorMessage = "*")]
        public int Priority { get; set; }
                 [Required(ErrorMessage = "*")]
         public string Image { get; set; }
    }
}