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
    
    public partial class ProductComment
    {
        public int id { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> DateInsert { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Rate { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
