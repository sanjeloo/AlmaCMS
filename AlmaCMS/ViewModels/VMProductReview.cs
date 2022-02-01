using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlmaCMS.ViewModels
{
    public class VMProductReview
    {
        public int Id { get; set; }

        public int ProductID { get; set; }
        public int Rate { get; set; }

        public string IP { get; set; }
        public string Email { get; set; }
        public DateTime DateInsert { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public bool Status { get; set; }

        public string ArticleTitle { get; set; }
        [AllowHtml]
        public string Answer { get; set; }
    }
}