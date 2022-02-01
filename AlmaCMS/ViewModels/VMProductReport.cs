using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlmaCMS.ViewModels
{
    public class VMProductReport
    {

        public int id { get; set; }

                        [Range(1, float.MaxValue, ErrorMessage = "لطفا  محصول را انتخاب نمایید")]
        public int  ProductId { get; set; }

        [Required(ErrorMessage="*")]

        public string  Title { get; set; }

        public DateTime DateInsert { get; set; }

        [AllowHtml]
        [Required(ErrorMessage="*")]
        public string Descriptions  { get; set; }

        public string AdminId { get; set; }

        public string SelectedFiles { get; set; }

        public string SelectedFileName { get; set; }

        public string SelectedFeatures { get; set; }
    } 
}