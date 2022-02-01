using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMShortMessage
    {
        [Required]
        public string Messagetext { get; set; }

        public string USerId { get; set; }

    }
}