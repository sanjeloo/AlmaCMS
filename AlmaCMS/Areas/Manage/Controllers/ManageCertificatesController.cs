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
  
    [Authorize]
    public class ManageCertificatesController : Controller
    {

        // GET: Manage/ManageCertificates
          // GET: Manage/ManageCertificates
        private DB_AlmaCmsEntities db;
        private CertificateRepository repCertificate;
        private CertificateGroupRepository repCertificateGroup;
        public static bool premission = false;
        
        public ManageCertificatesController()
        {
            db = new DB_AlmaCmsEntities();
            repCertificate = new CertificateRepository(db);
            repCertificateGroup = new CertificateGroupRepository(db);
           
        }


        #region Index
        public ActionResult Index(int id ,string searchString, int? page)
        {
            var vmCertificateList = new List<VMCertificates>();

            var CertificateList = repCertificate.getByGroupID(id);
            if (!String.IsNullOrEmpty(searchString))
            {
                CertificateList = CertificateList.Where(s => s.Title.Contains(searchString)).ToList();
                ViewBag.SearchString = searchString;
            }


            foreach (var item in CertificateList)
            {
                vmCertificateList.Add(item.toVMCertificate());
            }
            var CertificateGroup = repCertificateGroup.FindById(id);
            if (CertificateGroup != null)
                ViewBag.GroupTitle = CertificateGroup.Title;
            ViewBag.GroupID = CertificateGroup.id;
       
            return View(vmCertificateList.ToList());
        }
        #endregion

        #region Create
        public ActionResult Create(int id)
        {
            VMCertificates vmCertificate = new VMCertificates();
           
            ViewBag.GroupID = id;
            vmCertificate.GroupID = id;
            return View(vmCertificate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( VMCertificates vmCertificate)
        {
            if (ModelState.IsValid)
            {
          
                repCertificate.Insert(vmCertificate.toCertificate());
                Helpers.UserLogHelper.AddLog("گواهی نامه ها", (User.Identity.Name), "درج", vmCertificate.Title);
                return RedirectToAction("Index", new { id=vmCertificate.GroupID});
            }

            return View(vmCertificate);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Certificate Certificate = repCertificate.FindById((int)id);
            if (Certificate == null)
            {
                return HttpNotFound();
            }
            return View(Certificate.toVMCertificate());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMCertificates vmCertificate)
        {
            if (ModelState.IsValid)
            {
                repCertificate.Update(vmCertificate.toCertificate());
                Helpers.UserLogHelper.AddLog("گواهی نامه ها", (User.Identity.Name), "ویرایش", vmCertificate.Title);
                return RedirectToAction("Index", new { id = vmCertificate.GroupID });
            }
            return View(vmCertificate);
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Certificate Certificate = repCertificate.FindById((int)id);
            if (Certificate == null)
            {
                return HttpNotFound();
            }
            return View(Certificate.toVMCertificate());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var CurrentItem = repCertificate.FindById(id);
            int GroupID  = (int)repCertificate.FindById(id).GroupID;
            Helpers.UserLogHelper.AddLog("گواهی نامه ها", (User.Identity.Name), "حذف", CurrentItem.Title);
            repCertificate.Delete(id);
            return RedirectToAction("Index", new { id = GroupID });
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //public class RedirectFromManageCertificate : ActionFilterAttribute
        //{
        //    public override void OnActionExecuting(ActionExecutingContext filterContext)
        //    {
        //        if (ManageCertificateController.premission)
        //        {
        //            return;
        //        }


        //        base.OnActionExecuting(filterContext);

        //        var db = new PersiaCMS.Models.DBPersiaEntities();
        //        var rolId = db.Roles.FirstOrDefault(a => a.RoleName == "ManageCertificate").Id;

        //        HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
        //        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
        //        string cookieData = ticket.Name;
        //        var adminId = db.Admins.FirstOrDefault(a => a.Email == cookieData);
        //        if (adminId == null)
        //        {
        //            DashboardController.locked = true;
        //            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
        //            {
        //                controller = "Dashboard",
        //                action = "Index",
        //            }));
        //        }
        //        var exist = db.AdminRoles.Where(a => a.RoleId == rolId && a.UserId == adminId.Id).SingleOrDefault();
        //        if (exist != null)
        //        {
        //            ManageCertificateController.premission = true;
        //            Dictionary<string, string> url = PersiaCMS.Helper.IsIdentity.ReturnUrl();
        //            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
        //            {
        //                controller = url["controller"],
        //                action = url["action"]
        //            }));
        //        }
        //        else
        //        {
        //            DashboardController.locked = true;
        //            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
        //            {
        //                controller = "Dashboard",
        //                action = "Index",
        //            }));
        //        }
        //    }
        //}
    }
}