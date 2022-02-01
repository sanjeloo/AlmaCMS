using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMUserInfo
    {
        public int id { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        public string Phone { get; set; }

        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "*")]
        public string Mobile { get; set; }
        public string CodeMelli { get; set; }
        public string City { get; set; }

        public int  Stateid { get; set; }

        public int IntroductionTypeId { get; set; }

        public string PostalCode { get; set; }
        public string Tel { get; set; }
  

    }
}