using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlmaCMS.ViewModels
{
    public class VMSecureArticle
    {
        public int id { get; set; }

        [Required(ErrorMessage="*")]
        public string Title { get; set; }

        public string Image { get; set; }
        public DateTime DateInsert { get; set; }
              [Required(ErrorMessage = "*")]
        [AllowHtml]
        public string ArticleContent { get; set; }
    }
}