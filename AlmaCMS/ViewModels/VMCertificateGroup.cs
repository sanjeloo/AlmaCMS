using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace AlmaCMS.ViewModels
{
    public class VMCertificateGroup
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