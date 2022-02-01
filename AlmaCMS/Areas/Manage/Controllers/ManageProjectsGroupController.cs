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
 
    public class ManageProjectsGroupController : Controller
    {
        // GET: Manage/ManageProjectsGroup
                  public static bool premission = false;
        private DB_AlmaCmsEntities db;
        private ProjectGroupRepository repProjectGroup;
        ProjectRepository repProject;
        public ManageProjectsGroupController()
        {
            db = new DB_AlmaCmsEntities();
            repProject = new ProjectRepository(db);
            repProjectGroup = new ProjectGroupRepository(db);
        }

        // GET: Manage/ManageMenuItems
        public ActionResult Index(string searchString, int? page)
        {
            var vmGroupList = new List<VMprojectsgroup>();
            var groupList = repProjectGroup.GetAll().ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                groupList = groupList.Where(s => s.Title.Contains(searchString)).ToList();
                ViewBag.SearchString = searchString;
            }
            foreach (var item in groupList)
            {
                vmGroupList.Add(item.toVMProjectGroup());
            }
      
            return View(vmGroupList.ToList());

        }

        #region Create
        public ActionResult Create()
        {




            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMprojectsgroup vmGroup)
        {
         
            if (ModelState.IsValid)
            {



                repProjectGroup.Insert(vmGroup.toProjectGroup());
                Helpers.UserLogHelper.AddLog("گروه پروژه ها", (User.Identity.Name), "درج", vmGroup.Title);

                return RedirectToAction("Index");
            }

            return View(vmGroup);
        }
        #endregion


        #region Edit
        public ActionResult Edit(int? id)
        {
            ViewBag.ParentPages = repProjectGroup.Where(c => c.id != id).ToList();
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentpage = repProjectGroup.FindById((int)id);
            if (currentpage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(currentpage.toVMProjectGroup());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMprojectsgroup vmProjectgroup)
        {
          
            if (!ModelState.IsValid)
                return View(vmProjectgroup);




            repProjectGroup.Update(vmProjectgroup.toProjectGroup());
            Helpers.UserLogHelper.AddLog("گروه پروژه ها", (User.Identity.Name), "ویرایش", vmProjectgroup.Title);

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentPage = repProjectGroup.FindById((int)id);
            if (currentPage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);


            if (repProject.getByGroupID(currentPage.id).ToList().Count > 0)
            {

                ViewBag.ErrorMessage = "امکان حذف گروه پروژه ها وجود ندارد.گروه دارای پروژه می باشد";

            }
            return View(currentPage.toVMProjectGroup());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var CurrentItem = repProjectGroup.FindById(id);
            repProjectGroup.Delete(id);
            Helpers.UserLogHelper.AddLog("گروه پروژه ها", (User.Identity.Name), "حذف", CurrentItem.Title);

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

        
    }
    
}