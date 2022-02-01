using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMRSSReader
    {
        [Key]
        [Required(ErrorMessage="*")]
        public int FeedID { get; set; }
             [Required(ErrorMessage = "*")]
        public string Title { get; set; }
             [Required(ErrorMessage = "*")]
             public string Url { get; set; }

    }
}