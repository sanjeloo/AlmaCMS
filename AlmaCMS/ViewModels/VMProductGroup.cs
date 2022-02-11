using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlmaCMS.ViewModels
{
    public class VMProductGroup
    {
        [Required(ErrorMessage = "*")]
        public int id { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*")]
        public string Image { get; set; }
        [AllowHtml]
        [Required(ErrorMessage = "*")]
        public string Description { get; set; }

        [Required(ErrorMessage = "*")]
        public int Priority { get; set; }

        [Required(ErrorMessage = "*")]
        public string Keywords { get; set; }
       // [Required(ErrorMessage = "*")]
        public int? ParentId { get; set; }


    }
}