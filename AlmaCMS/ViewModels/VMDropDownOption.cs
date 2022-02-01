using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMDropDownOption
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string icon { get; set; }

        public string Customevalue { get; set; }
        public string Comment { get; set; }

        public bool  Selected { get; set; }
    }
}