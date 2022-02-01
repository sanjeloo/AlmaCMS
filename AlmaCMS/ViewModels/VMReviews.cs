using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMReviews
    {
        public int Id { get; set; }

        public int ProductID { get; set; }
        public int Rate { get; set; }

        public int IP { get; set; }

        public DateTime DateInsert { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public bool Status { get; set; }

        public string ProductTitle { get; set; }
        public string Answer { get; set; }
    }
}