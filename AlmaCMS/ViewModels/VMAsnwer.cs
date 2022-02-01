using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMAsnwer
    {
        [Required(ErrorMessage = "*")]
        public int id { get; set; }
        public int QuestionID { get; set; }

        [Required(ErrorMessage = "*")]
        public string AnswerText { get; set; }

        public DateTime DateInsert { get; set; }
    }
}