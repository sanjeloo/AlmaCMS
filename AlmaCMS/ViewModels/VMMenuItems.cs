using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlmaCMS.ViewModels
{
    public class VMMenuItems
    {
        public int id { get; set; }
        public int ParentID { get; set; }
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "*")]
        public string DirectUrl { get; set; }
        public bool OpenInNew { get; set; }
        public string Comments { get; set; }
        public bool IsPrimary { get; set; }
        public int Priority { get; set; }
    }
}