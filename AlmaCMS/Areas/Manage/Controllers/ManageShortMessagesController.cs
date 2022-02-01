using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlmaCMS.Areas.Manage.Controllers
{
    public class ManageShortMessagesController : Controller
    {
        // GET: Manage/ManageShortMessages
        public ActionResult Index(string UserName)
        {
            return View();
        }
    }
}