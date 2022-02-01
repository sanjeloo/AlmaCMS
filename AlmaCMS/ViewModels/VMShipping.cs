using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMShipping
    {
        public string MemberID { get; set; }

        [Required(ErrorMessage = "*")]
        public string DeliverName { get; set; }
        //public int deliverstateID { get; set; }
        //[Required(ErrorMessage = "*")]
        //public string DeliveCity { get; set; }
        [Required(ErrorMessage = "*")]
        public string DeliverTel { get; set; }
        [Required(ErrorMessage = "*")]
        public string DeliverMobile { get; set; }

        //public string DeliverPostalCode { get; set; }
        [Required(ErrorMessage = "*")]
        public string Address { get; set; }
        public DateTime DateInsert { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "لطفا نحوه ارسال را انتخاب نمایید ")]
        public int SendTypeID { get; set; }

        public double SendTypePrice { get; set; }
        public int SendTimeID { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "لطفا نحوه پرداخت را انتخاب نمایید ")]
        public int PaymentTypeid { get; set; }

        [Range(1, 100, ErrorMessage = "لطفا استان را انتخاب نمایید")]
        public int StateID { get; set; }
        [Range(1, 10000, ErrorMessage = "لطفا شهر را انتخاب نمایید")]
        public int CityID { get; set; }

        [Required(ErrorMessage = "لطفا کد پستی را وارد نمایید")]
        public string PostalCode { get; set; }
    }
}