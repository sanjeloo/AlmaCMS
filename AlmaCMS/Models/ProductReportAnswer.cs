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
    
    public partial class ProductReportAnswer
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Descriptions { get; set; }
        public Nullable<System.DateTime> AnswerDate { get; set; }
        public Nullable<int> ProductReportId { get; set; }
        public string UserId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual ProductReport ProductReport { get; set; }
    }
}
