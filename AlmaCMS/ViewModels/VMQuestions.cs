using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMQuestions
    {
        [Required(ErrorMessage = "*")]
        public int id { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.MultilineText)]
        public string Title { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        public DateTime DateInsert { get; set; }  
        [Required(ErrorMessage = "*")]
        [EmailAddress]
        public string Email { get; set; }


        public bool Active { get; set; }
        [DataType(DataType.MultilineText)]
        public string AnswerText { get; set; }
    }
}