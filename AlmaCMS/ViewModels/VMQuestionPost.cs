using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMQuestionPost
    {
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
         [Required(ErrorMessage = "*")]
        public string Title { get; set; }
        
         [Required(ErrorMessage = "*")]
        [EmailAddress]
        
        public string Email { get; set; }

    }
}