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
    
    public partial class UsersMessage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UsersMessage()
        {
            this.MessageAnswers = new HashSet<MessageAnswer>();
        }
    
        public int id { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> DateInsert { get; set; }
        public string Descriptions { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string AdminIId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MessageAnswer> MessageAnswers { get; set; }
    }
}
