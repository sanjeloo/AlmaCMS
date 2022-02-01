using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMChargeBag
    {
        [Required(ErrorMessage="*")]
        public string DiscountCode { get; set; }

    }
}