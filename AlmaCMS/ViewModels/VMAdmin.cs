using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMAdmin
    {
        [Key]
        public int Id { get; set; }

        public int SamaneId { get; set; }

        [Required(ErrorMessage = "نام کامل را وارد نمایید")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "آدرس ایمیل را وارد نمایید")]
        [DataType(DataType.EmailAddress, ErrorMessage = "آدرس ایمیلم عتبر وارد نمایید")]
        [EmailAddress(ErrorMessage = "آدرس ایمیل معتبر وارد نمایید")]
        public string Email { get; set; }

        [Required(ErrorMessage = "شماره تماس را وارد نمایید")]
        [Range(0, long.MaxValue, ErrorMessage = "یک مقدار عددی وارد نمایید")]
        public string Tell { get; set; }


        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "رمز عبور را وارد نمایید")]
        [MaxLength(12, ErrorMessage = "رمز عبور نباید بیشتر از 12 کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "رمز عبور نباید کمتر از 6 کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string Pass { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Compare("Pass", ErrorMessage = "رمز عبور با تکرار آن یکی نمیباشد")]
        [DataType(DataType.Password)]
        public string PassReapet { get; set; }


        [Display(Name = "رمز عبور")]
        //[Required(ErrorMessage = "رمز عبور را وارد نمایید")]
        [MaxLength(12, ErrorMessage = "رمز عبور نباید بیشتر از 12 کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "رمز عبور نباید کمتر از 6 کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string PassEdit { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Compare("PassEdit", ErrorMessage = "رمز عبور با تکرار آن یکی نمیباشد")]
        [DataType(DataType.Password)]
        public string PassRepeatEdit { get; set; }


        public string Image { get; set; }


        public bool IsActive { get; set; }

        //[Required(ErrorMessage="سطح دسترسی را مشخص نمایید")]
        public string SelectedFeatures { get; set; }


        public string SamaneName { get; set; }

        public IEnumerable<System.Web.Mvc.SelectList> SamaneList { get; set; }


        public IEnumerable<System.Web.Mvc.SelectList> RoleList { get; set; }
    }
}