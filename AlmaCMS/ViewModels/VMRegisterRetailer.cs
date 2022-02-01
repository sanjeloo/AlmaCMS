using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMRegisterRetailer
    {
        [Required(ErrorMessage="*")]
        public string Name { get; set; }
               [Required(ErrorMessage = "*")]
        public string FatherName { get; set; }
               [Required(ErrorMessage = "*")]
        public string BirthDate { get; set; }
               [Required(ErrorMessage = "*")]
        public string ExportPlace { get; set; }
               [Required(ErrorMessage = "*")]
        public string IDNumber { get; set; }
               [Required(ErrorMessage = "*")]
        public string NationalCode { get; set; }
        public string ShopAddress { get; set; }
        public string StoreAddress { get; set; }
        public string HomeTel { get; set; }
        public string ShopTel { get; set; }
        public string StoreTel { get; set; }
               [Required(ErrorMessage = "*")]
        public string Mobile { get; set; }
        public string Fax { get; set; }
               [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage="*")]
        public string Email { get; set; }
        public string Companyfriendship { get; set; }
        public string newProductfriendship { get; set; }
        public string friendshipWay { get; set; }
        public string ProductType { get; set; }
        public string OtherAgency { get; set; }
        public string StoreMetraj { get; set; }
        public string ShopOwnerType { get; set; }
        public string StoreOwnerType { get; set; }
        public string ActivityTime { get; set; }
        public string PublishCarsNumbers { get; set; }
        public string PersonelNumber { get; set; }
        public string AccountNumber { get; set; }
       
    }
}