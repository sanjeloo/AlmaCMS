using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlmaCMS.ViewModels
{
    public class VMCertificates
    {

        [Required(ErrorMessage = "*")]
        public int id { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        [UIHint("Html")]
        [AllowHtml]
        public string Descriotion { get; set; }

        public int GroupID { get; set; }

        public int Prority { get; set; }
        public string Image { get; set; }
        public string Keywords { get; set; }
    }
}