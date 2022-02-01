using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlmaCMS.ViewModels
{
    public class VMSiteSettings
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "عنوان را وارد کنید")]
        public string Title { get; set; }
        public string SiteIcon { get; set; }
        public string MailSMTP { get; set; }
        public string MailPort { get; set; }
        [Required(ErrorMessage = "ایمیل خود را وارد کنید")]

        public string MailUser { get; set; }
        [Required(ErrorMessage = "رمز عبور خود را وارد کنید")]

        public string MailPassWord { get; set; }
        public string MailForm { get; set; }

        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        [AllowHtml]
        public string Disclaimer { get; set; }
        [AllowHtml]
        public string FooterText { get; set; }
        [AllowHtml]
        public string FooterAbout { get; set; }
        public bool FactorActive { get; set; }

        public bool VAT { get; set; }

        public double VATPercent { get; set; }
        public double SendBoxingPrice { get; set; }
        public double SendTaxPrice { get; set; }
        public double SendInsurancePrice { get; set; }
        public double SentVatPrice { get; set; }

        public bool BirtDateGift { get; set; }
        public long BirtDateGiftPrice { get; set; }


        [Required(ErrorMessage="*")]
        public int ProfitPercentCount { get; set; }
        [Required(ErrorMessage = "*")]
        public int ProfitPercent { get; set; }
    }
}