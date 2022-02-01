using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels.Expert
{
    public class VMExpertInfo
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string NationalCode { get; set; }
        public string BirthDate { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Descriptions { get; set; }
        public Nullable<bool> isActive { get; set; }
        public int InfioID { get; set; }
        public string PhoneNumber { get; set; }
    }
}