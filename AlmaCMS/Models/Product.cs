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
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.ProductsImages = new HashSet<ProductsImage>();
            this.OrderProducts = new HashSet<OrderProduct>();
            this.ProductReviews = new HashSet<ProductReview>();
            this.Favorits = new HashSet<Favorit>();
            this.ProductReports = new HashSet<ProductReport>();
            this.ProductComments = new HashSet<ProductComment>();
        }
    
        public int id { get; set; }
        public Nullable<int> GroupID { get; set; }
        public string Title { get; set; }
        public string Specification { get; set; }
        public Nullable<int> Priority { get; set; }
        public string Image { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public Nullable<long> price { get; set; }
        public Nullable<long> PriceBeforeDiscount { get; set; }
        public Nullable<System.DateTime> SpecialSaleStartDate { get; set; }
        public Nullable<System.DateTime> SpeciaSaleEndDate { get; set; }
        public Nullable<bool> ExistStatus { get; set; }
        public Nullable<int> ExistCount { get; set; }
        public int VisitCount { get; set; }
        public bool Visibility { get; set; }
    
        public virtual ProductsGroup ProductsGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductsImage> ProductsImages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductReview> ProductReviews { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Favorit> Favorits { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductReport> ProductReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductComment> ProductComments { get; set; }
    }
}
