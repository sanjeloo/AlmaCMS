using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
namespace AlmaCMS.ViewModels

{
    public class VMprojectsgroup
    {
        [Required(ErrorMessage = "*")]
        public int id { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        public string Descriotion { get; set; }


        public string Image { get; set; }
        public string Keywords { get; set; }

    }
}