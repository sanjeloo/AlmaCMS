using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMFavortis
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public long Price { get; set; }
        public int ProductId { get; set; }
        public string MemberId { get; set; }
    }
}