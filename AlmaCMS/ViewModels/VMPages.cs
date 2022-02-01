using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlmaCMS.ViewModels
{
    public class VMPages
    {
        [Key]

        public int PageID { get; set; }

        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        public string PageName { get; set; }
        public string UniqID { get; set; }

        public int UserID { get; set; }
        [Required(ErrorMessage = "*")]
        [AllowHtml]
        [UIHint("Html")]
        public string PageContent { get; set; }
        public string ImageUrl { get; set; }
        public string ImageTitle { get; set; }
        public string ImageDesc { get; set; }
        public int ParentId { get; set; }

        public string DirectUrl { get; set; }
        public bool ShowInMenu { get; set; }
        public string KeyWords { get; set; }
        public string ShortDesc { get; set; }

        public DateTime dateInsert { get; set; }
    }
}