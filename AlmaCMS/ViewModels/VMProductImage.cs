using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace AlmaCMS.ViewModels
{
    public class VMProductImage
    {

        [Required(ErrorMessage = "*")]
        public int id { get; set; }

        public int ProductID { get; set; }
        [Required(ErrorMessage = "*")]

        public string Title { get; set; }
        public string Image { get; set; }
    }
}