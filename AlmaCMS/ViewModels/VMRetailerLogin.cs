using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMRetailerLogin
    {
        [Required(ErrorMessage="*")]
        public string UserName { get; set; }
         [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string Pass { get; set; }
    }
}