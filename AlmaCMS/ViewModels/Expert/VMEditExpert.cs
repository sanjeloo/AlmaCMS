using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels.Expert
{
    public class VMEditExpert
    {


        public int id { get; set; }
        public string UserID { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }

        public string Family { get; set; }
        [Required(ErrorMessage = "*")]
        public string NationalCode { get; set; }
        public string BirthDate { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }

        public string Tel { get; set; }
        [Required]
        [Phone]
        public string Mobile { get; set; }
        public string Descriptions { get; set; }

    }
}