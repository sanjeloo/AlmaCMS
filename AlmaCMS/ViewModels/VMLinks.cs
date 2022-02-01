using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMLinks
    {
        [Required(ErrorMessage = "*")]
        public int id { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        public string LinkUrl { get; set; }
        public string Image { get; set; }
        public bool Active { get; set; }
        public int LinkGroupID { get; set; }

        public string Descriptions { get; set; }
     
    }
}