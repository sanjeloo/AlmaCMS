using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMDiscountCode
    {

        public int id { get; set; }

        public string strCode { get; set; }


        [Required(ErrorMessage="*")]
        public long     DiscountPrice { get; set; }

        public int CodeCount { get; set; }

        public string Descriptions { get; set; }

        public DateTime  CreateDate { get; set; }

        public DateTime ExpireDate { get; set; }
        public bool Used { get; set; }
    }
}