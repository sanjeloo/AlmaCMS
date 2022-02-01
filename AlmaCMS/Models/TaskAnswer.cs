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
    
    public partial class TaskAnswer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaskAnswer()
        {
            this.TaskAnwerDocs = new HashSet<TaskAnwerDoc>();
        }
    
        public int id { get; set; }
        public Nullable<int> TaskId { get; set; }
        public string ExpertId { get; set; }
        public string Title { get; set; }
        public string Descriptions { get; set; }
        public Nullable<System.DateTime> AnswerDate { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual TasksList TasksList { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskAnwerDoc> TaskAnwerDocs { get; set; }
    }
}
