using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlmaCMS.ViewModels
{
    public class VMChangeUserAccess
    {
        public string UserID { get; set; }
        public string SelectedAccess { get; set; }
        public string UserName { get; set; }
    }
}