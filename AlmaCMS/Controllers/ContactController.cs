using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.ViewModels;
using AlmaCMS.Models;

using CaptchaMvc;
using CaptchaMvc.HtmlHelpers;
namespace AlmaCMS.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(ViewModels.VMSiteMesage newMSg)
        {
            if (ModelState.IsValid)
            {
                if (!this.IsCaptchaValid("*"))
                {
                    ViewBag.CaptchaMsg = "کد امنیتی وارد شده صحیح نمی باشد";
                    return View(newMSg);
                }
                var db = new DB_AlmaCmsEntities();
                var newmessage = new Models.SiteMessage();
                var pc = new System.Globalization.PersianCalendar();
                newmessage.DateInsert =DateTime.Now;
                newmessage.Email = newMSg.Email;
                newmessage.Message = newMSg.Message;
                newmessage.Name = newMSg.Name;
                newmessage.Subject = newMSg.Subject;
                db.SiteMessages.Add(newmessage);
                db.SaveChanges();
                ViewBag.Msg = "<div class='alert alert-success text-center'> پیام شما با موفقیت ارسال شد </div>";
                ModelState.Clear();
                return View();
            }
            else
            {
                return View(newMSg);
            }

        }
    }
}