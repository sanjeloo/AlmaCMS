//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AlmaCMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Payment
    {
        public int id { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> PayStatusID { get; set; }
        public string RefID { get; set; }
        public string SaleRefrenceID { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<System.DateTime> DateInsert { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual PaymentStatu PaymentStatu { get; set; }
    }
}