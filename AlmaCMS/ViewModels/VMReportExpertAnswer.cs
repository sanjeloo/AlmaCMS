using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlmaCMS.ViewModels
{
    public class VMReportExpertAnswer
    {
        public int id { get; set; }

        public int ProductReportId { get; set; }

        public string ExpertId { get; set; }
        [Required(ErrorMessage = "*")]

        public string Title { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime DateInsert { get; set; }




        [Required(ErrorMessage = "*")]

        [AllowHtml]
        public string Descriptions { get; set; }




        public string SelectedFiles { get; set; }
    }
}