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
    
    public partial class SubPage
    {
        public int id { get; set; }
        public int PageId { get; set; }
        public string Title { get; set; }
        public string ShortDescriptions { get; set; }
        public string PageContent { get; set; }
        public string Image { get; set; }
        public Nullable<int> Priority { get; set; }
    }
}
