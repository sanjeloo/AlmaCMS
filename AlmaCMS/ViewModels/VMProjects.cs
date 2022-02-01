using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AlmaCMS.ViewModels
{
    public class VMProjects
    {
    [Required(ErrorMessage = "*") ]
        public int id { get; set; }
          [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        public DateTime DateInsert { get; set; }
          [Required(ErrorMessage = "*")]
        public string Image { get; set; }
          [Required(ErrorMessage = "*")]
        [AllowHtml]
          public string ProjectContent { get; set; }
          public string Link { get; set; }
          public string Keywords { get; set; }
          public string Description { get; set; }
          public int GroupID { get; set; }
          public int Priority { get; set; }
    }
}