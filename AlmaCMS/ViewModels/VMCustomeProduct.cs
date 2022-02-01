using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace AlmaCMS.ViewModels
{
    public class VMCustomeProduct
    {
        [Required(ErrorMessage = "*")]
        public int id { get; set; }

        [Required(ErrorMessage = "*")]
        public int GroupID { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        [Required(ErrorMessage = "*")]
        [UIHint("html")]
        [AllowHtml]
        public string PageContent { get; set; }
        [Required(ErrorMessage = "*")]
     
        public string Image { get; set; }

          [Required(ErrorMessage = "*")]
      public string Descriptions { get; set; }



        [Required(ErrorMessage = "*")]
        public string Keywords { get; set; }

   

    }
}