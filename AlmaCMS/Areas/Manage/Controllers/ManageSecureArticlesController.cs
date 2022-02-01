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

    public class ManageSecureArticlesController : Controller
    {
       
        // GET: Manage/ManageSecureArticles
          // GET: Manage/ManageSecureArticles
        private DB_AlmaCmsEntities db;
        private SecureArticleRepository repSecureArticle;

        public static bool premission = false;

        public ManageSecureArticlesController()
        {
            db = new DB_AlmaCmsEntities();
            repSecureArticle = new SecureArticleRepository(db);
          
           
        }


        #region Index
        public ActionResult Index()
        {
            var vmSecureArticleList = new List<VMSecureArticle>();

            var SecureArticleList = repSecureArticle.GetAll();
           


            foreach (var item in SecureArticleList)
            {
                vmSecureArticleList.Add(item.toVMScureArticles());
            }
           
       
            return View(vmSecureArticleList.ToList());
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            VMSecureArticle vmSecureArticle = new VMSecureArticle();
           
      
            
            return View(vmSecureArticle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( VMSecureArticle vmSecureArticle)
        {
            if (ModelState.IsValid)
            {
                vmSecureArticle.DateInsert = DateTime.Now;
                repSecureArticle.Insert(vmSecureArticle.toRetoSecureArticle());
                Helpers.UserLogHelper.AddLog("اطلاعیه نمایندکان", (User.Identity.Name), "درج", vmSecureArticle.Title);

                return RedirectToAction("Index");
            }

            return View(vmSecureArticle);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecureArticle SecureArticle = repSecureArticle.FindById((int)id);
            if (SecureArticle == null)
            {
                return HttpNotFound();
            }
            return View(SecureArticle.toVMScureArticles());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMSecureArticle vmSecureArticle)
        {
            if (ModelState.IsValid)
            {
                repSecureArticle.Update(vmSecureArticle.toRetoSecureArticle());
                Helpers.UserLogHelper.AddLog("اطلاعیه نمایندکان", (User.Identity.Name), "ویرایش", vmSecureArticle.Title);

                return RedirectToAction("Index");
            }
            return View(vmSecureArticle);
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecureArticle SecureArticle = repSecureArticle.FindById((int)id);
            if (SecureArticle == null)
            {
                return HttpNotFound();
            }
            return View(SecureArticle.toVMScureArticles());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            var CurrentItem = repSecureArticle.FindById(id);
            repSecureArticle.Delete(id);
            Helpers.UserLogHelper.AddLog("اطلاعیه نمایندکان", (User.Identity.Name), "حذف", CurrentItem.Title);

            return RedirectToAction("Index");
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

        //public class RedirectFromManageSecureArticle : ActionFilterAttribute
        //{
        //    public override void OnActionExecuting(ActionExecutingContext filterContext)
        //    {
        //        if (ManageSecureArticleController.premission)
        //        {
        //            return;
        //        }


        //        base.OnActionExecuting(filterContext);

        //        var db = new PersiaCMS.Models.DBPersiaEntities();
        //        var rolId = db.Roles.FirstOrDefault(a => a.RoleName == "ManageSecureArticle").Id;

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
        //            ManageSecureArticleController.premission = true;
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