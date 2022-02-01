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
    
    public partial class Project
    {
        public int id { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> DateInsert { get; set; }
        public string Image { get; set; }
        public string ProjectContent { get; set; }
        public string Link { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public Nullable<int> GroupID { get; set; }
        public Nullable<int> Priority { get; set; }
        public string ProjectType { get; set; }
        public string ProjectPlace { get; set; }
        public string Employer { get; set; }
        public string StartDate { get; set; }
        public string FinishDate { get; set; }
    
        public virtual ProjecGroup ProjecGroup { get; set; }
    }
}
