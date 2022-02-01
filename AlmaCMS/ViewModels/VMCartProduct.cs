using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMCartProduct
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public double weight { get; set; }
        public string Image { get; set; }
        public string PartNumber { get; set; }

    }
}