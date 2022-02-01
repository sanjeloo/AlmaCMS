using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMOrderList
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string MemberID { get; set; }
        public string Mobile { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public int SendTypeID { get; set; }
        public int StateID { get; set; }
        public int CityID { get; set; }
        public string SendTypeTitle { get; set; }
        public string PaymentTitle { get; set; }
        public int PaymentType { get; set; }
        public int OrderStatusID { get; set; }
        public string StatusTitle { get; set; }
        public string TrackingID { get; set; }
        public long SendPrice { get; set; }
        public long TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string PersianOrderDate { get; set; }
        public double VAT { get; set; }
        public string StateTitle { get; set; }

        public string CityTitle { get; set; }
        public string PstalCode { get; set; }
    }
}