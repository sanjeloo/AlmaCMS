using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.Models;
using AlmaCMS.ViewModels;
using AlmaCMS.Helpers;
using System.Web.Security;
using System.Web.Routing;
namespace AlmaCMS.Areas.Manage.Controllers
{
 
    public class ManageSiteMessagesController : Controller
    {
        // GET: AdminManage/ManageSiteMessages
        public static bool premission = false;
        public ActionResult Index()
        {
            var db = new DB_AlmaCmsEntities();
            List<VMSiteMesage> mList = new List<VMSiteMesage>();
            foreach (var item in db.SiteMessages)
            {
                var message = new VMSiteMesage();
                message.id = item.id;
                message.Subject = item.Subject;
                message.Tel = item.Tel;
                message.Name = item.Name;
                message.Email = item.Email;
                message.Message = item.Message;
                message.PDateInsert = ((DateTime)item.DateInsert).ToPersianShortDate();
                message.Subject = item.Subject;
                mList.Add(message);
            }
            return View(mList);
        }


        public ActionResult Delete(int id)
        {
            var db = new DB_AlmaCmsEntities();
            var item = db.SiteMessages.Where(a => a.id == id).SingleOrDefault();
            if (item != null)
            {
                var message = new VMSiteMesage();
                message.Name = item.Name;
                message.Subject = item.Subject;
                message.PDateInsert = ((DateTime)item.DateInsert).ToPersianShortDate();
                message.Email = item.Email;
                message.Tel = item.Tel;
                message.Message = item.Message;
                return View(message);
            }
            else
                return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(VMSiteMesage group)
        {
            var db = new DB_AlmaCmsEntities();
            var item = db.SiteMessages.Where(a => a.id == group.id).SingleOrDefault();
            if (item != null)
            {
                db.SiteMessages.Remove(item);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }

  

}