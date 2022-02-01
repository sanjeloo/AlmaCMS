using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace AlmaCMS.ViewModels
{
    public class VMSiteMesage
    {
    
        public int id { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }


        public string Tel { get; set; }
        [Required(ErrorMessage = "*")]
        [EmailAddress]
        public string Email { get; set; }
  

           [Required(ErrorMessage = "*")]
        public string Message { get; set; }
           public string Subject { get; set; }
           public DateTime DateInsert { get; set; }

           public string PDateInsert { get; set; }
           public bool IsRead { get; set; }
    }
}