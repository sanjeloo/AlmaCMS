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
    
    public partial class DiscountHistory
    {
        public int id { get; set; }
        public Nullable<int> DiscountId { get; set; }
        public string UserId { get; set; }
        public Nullable<System.DateTime> DateInsert { get; set; }
        public string Descriptions { get; set; }
    
        public virtual DiscountCode DiscountCode { get; set; }
    }
}