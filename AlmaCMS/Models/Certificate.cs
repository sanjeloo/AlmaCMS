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
    
    public partial class Certificate
    {
        public int id { get; set; }
        public Nullable<int> GroupID { get; set; }
        public string Title { get; set; }
        public string Descriptions { get; set; }
        public string Image { get; set; }
        public string Keywords { get; set; }
        public Nullable<int> Prority { get; set; }
    
        public virtual CerticicateGroup CerticicateGroup { get; set; }
    }
}