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
    
    public partial class VWMessageAnswer
    {
        public int id { get; set; }
        public Nullable<int> MessageId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Descritions { get; set; }
        public Nullable<System.DateTime> Answerdate { get; set; }
        public string Name { get; set; }
    }
}
