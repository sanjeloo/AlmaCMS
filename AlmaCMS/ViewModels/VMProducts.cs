using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace AlmaCMS.ViewModels
{
    public class VMProducts
    {

        [Required(ErrorMessage = "*")]
        public int id { get; set; }

        [Required(ErrorMessage = "*")]
        public int GroupID { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        [Required(ErrorMessage = "*")]
        [UIHint("html")]
        [AllowHtml]
        public string Specification { get; set; }
        [Required(ErrorMessage = "*")]
        public int Priority { get; set; }
        [Required(ErrorMessage = "*")]
        public string Image { get; set; }

        [Required(ErrorMessage = "*")]
        public string Description { get; set; }



        [Required(ErrorMessage = "*")]
        public string Keywords { get; set; }

        [Required(ErrorMessage = "*")]
        public long Price { get; set; }
        [Required(ErrorMessage = "*")]
        public long PriceBeforeDiscount { get; set; }

        public DateTime? SpecialSaleStartDate { get; set; }
        public DateTime? SpeciaSaleEndDate { get; set; }

        public bool ExistStatus { get; set; }
        public bool Visibility { get; set; }

        public int  ExistCount { get; set; }

        public int VisitCount { get; set; }

    }
}