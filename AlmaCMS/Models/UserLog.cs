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
    
    public partial class UserLog
    {
        public long Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public string UserName { get; set; }
        public string PageTitle { get; set; }
        public string Action { get; set; }
        public Nullable<System.DateTime> dateinsert { get; set; }
        public string PostTitle { get; set; }
    }
}