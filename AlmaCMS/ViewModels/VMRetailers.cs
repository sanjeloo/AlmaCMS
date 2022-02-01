using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMRetailers
    {

        public int id { get; set; }

        [Required(ErrorMessage="*")]
        public string CompanyName { get; set; }

        public string Name { get; set; }

        public string Tel { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public string Address { get; set; }
           [Required(ErrorMessage = "*")]
        public string USerName { get; set; }
           [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string Pass { get; set; }
         [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        public bool ActiveState { get; set; }

      

    }
}