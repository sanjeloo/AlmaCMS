using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMOrderProductSite
    {
        public int id { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }

        public int Quantity { get; set; }


        public long Price { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string PartNumber { get; set; }
        public string BrandTitle { get; set; }
    }
}