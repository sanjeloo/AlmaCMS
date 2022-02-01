using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMGallery
    {
        [Required]
        public int id { get; set; }
        [Required]
        [Display(Name = "شماره آلبوم")]
        public int AlbumId { get; set; }
        [Required]

        public string TumbImage { get; set; }
        [Required]
        [Display(Name = "تصویر")]
        public string FileUrl { get; set; }
        public int MediaType { get; set; }

        public string Comments { get; set; }
        public string MediaTypeTitle { get; set; }
    }
}