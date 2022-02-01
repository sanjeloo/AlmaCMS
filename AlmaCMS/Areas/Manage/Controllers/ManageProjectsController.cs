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
    public class ManageProjectsController : Controller
    {
      private DB_AlmaCmsEntities db;
        private ProjectRepository repProject;
        private ProjectGroupRepository repProjectGroup;
        public static bool premission = false;
        
        public ManageProjectsController()
            {
        
            db = new DB_AlmaCmsEntities();
            repProject = new ProjectRepository (db);
            repProjectGroup = new ProjectGroupRepository(db);
           
        }


        #region Index
        public ActionResult Index(int id ,string searchString, int? page)
        {
            var vmProjectList = new List<VMProjects>();

            var ProjectList = repProject.getByGroupID(id);
            if (!String.IsNullOrEmpty(searchString))
            {
                ProjectList = ProjectList.Where(s => s.Title.Contains(searchString)).ToList();
                ViewBag.SearchString = searchString;
            }


            foreach (var item in ProjectList)
            {
                vmProjectList.Add(item.toVMProject());
            }
            var ProjectGroup = repProjectGroup.FindById(id);
            if (ProjectGroup != null)
                ViewBag.GroupTitle = ProjectGroup.Title;
            ViewBag.GroupID = ProjectGroup.id;
       
            return View(vmProjectList.ToList());
        }
        #endregion

        #region Create
        public ActionResult Create(int id)
        {
            VMProjects vmProject = new VMProjects();
           
            ViewBag.GroupID = id;
            vmProject.GroupID = id;
            return View(vmProject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( VMProjects vmProject)
        {
            if (ModelState.IsValid)
            {
                vmProject.DateInsert = DateTime.Now;
                repProject.Insert(vmProject.toProject());
                Helpers.UserLogHelper.AddLog("پروژه ها", (User.Identity.Name), "درج", vmProject.Title);

                return RedirectToAction("Index", new { id=vmProject.GroupID});
            }

            return View(vmProject);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project Project = repProject.FindById((int)id);
            if (Project == null)
            {
                return HttpNotFound();
            }
            return View(Project.toVMProject());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMProjects vmProject)
        {
            if (ModelState.IsValid)
            {
                repProject.Update(vmProject.toProject());
                Helpers.UserLogHelper.AddLog("پروژه ها ", (User.Identity.Name), "ویرایش", vmProject.Title);

                return RedirectToAction("Index", new { id = vmProject.GroupID });
            }
            return View(vmProject);
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project Project = repProject.FindById((int)id);
            if (Project == null)
            {
                return HttpNotFound();
            }
            return View(Project.toVMProject());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var CurrentItem = repProject.FindById(id);
            int GroupID  = (int)repProject.FindById(id).GroupID;
            Helpers.UserLogHelper.AddLog("پروژه ها", (User.Identity.Name), "حذف", CurrentItem.Title);
            repProject.Delete(id);

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

        //public class RedirectFromManageProject : ActionFilterAttribute
        //{
        //    public override void OnActionExecuting(ActionExecutingContext filterContext)
        //    {
        //        if (ManageProjectController.premission)
        //        {
        //            return;
        //        }


        //        base.OnActionExecuting(filterContext);

        //        var db = new PersiaCMS.Models.DBPersiaEntities();
        //        var rolId = db.Roles.FirstOrDefault(a => a.RoleName == "ManageProject").Id;

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
        //            ManageProjectController.premission = true;
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