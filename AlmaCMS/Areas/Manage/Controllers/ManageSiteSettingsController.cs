using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.Models;
using AlmaCMS.ViewModels;
using AlmaCMS.Repositories;
using AlmaCMS.Helpers;
using PagedList;
using PagedList.Mvc;

using System.Web.Routing;
using System.Web.Security;
namespace AlmaCMS.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageSiteSettingsController : Controller
    {

        private DB_AlmaCmsEntities db;
        private SiteSettingRepository RepSiteSetting;
        UserInfoRepositiry repMember;
        public ManageSiteSettingsController()
        {
            db = new DB_AlmaCmsEntities();
            RepSiteSetting = new SiteSettingRepository(db);
            repMember = new UserInfoRepositiry(db);
        }

        // GET: Manage/SiteSettings
        public ActionResult Index()
        {
            //if (!NSShop.Helpers.UserRole.IsUserRole(repMember.GetByUserID(User.Identity.get).MemberID, "SiteSettings"))
            //{
            //    AdminController.locked = true;
            //    return RedirectToAction("Dashboard", "Admin");

            //}
            var currentpage = RepSiteSetting.GetAll().FirstOrDefault();
            if (currentpage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            ViewBag.UpdateSate = "0";
            return View(currentpage.toVMSiteSettings());
        }
        [HttpPost]
        public ActionResult Index(VMSiteSettings vmsitesetting)
        {

            if (ModelState.IsValid)
            {

                RepSiteSetting.Update(vmsitesetting.toSiteSetting());
                ViewBag.UpdateSate = "1";

            }
            else
            {
                ViewBag.UpdateSate = "0";
            }

            return View(vmsitesetting);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}