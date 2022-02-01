using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlmaCMS.ViewModels
{
    public class VMTask
    {

        public int id { get; set; }
          [Required(ErrorMessage = "*")]
        public string Title { get; set; }
          [Required(ErrorMessage = "*")]
        public DateTime DateInsert { get; set; }

        public int RequiredTime { get; set; }
          [Required(ErrorMessage = "*")]
        public DateTime ExpireDate { get; set; }
        public string AdminId { get; set; }

        [Required(ErrorMessage="*")]

        [AllowHtml]
        public string Descriptions { get; set; }

        public string SelectedFeatures { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, 24, ErrorMessage = "ساعت را وارد نمایید")]
        public int adHour { get; set; }
        [Required(ErrorMessage = "*")]
        [Range(0, 60, ErrorMessage = "دقیقه را وارد نمایید")]
        public int adMinute { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, 365, ErrorMessage = "روز را وارد نمایید")]
        public int AdDays { get; set; }

        public string SelectedFiles { get; set; }

        public string SelectedFileName { get; set; }

        public bool SendAlarmSMS { get; set; }

    }
}