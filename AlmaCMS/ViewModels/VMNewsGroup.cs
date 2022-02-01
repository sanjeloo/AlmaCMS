using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMNewsGroup
    {
          [Required(ErrorMessage = "*")]
        public int id { get; set; }
             [Required(ErrorMessage = "*")]
          public string Title { get; set; }
          public string Comments { get; set; }
          [Required(ErrorMessage = "*")]
          public string Keywords { get; set; }
    }
}