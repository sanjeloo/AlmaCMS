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
    [HasPermission("ManageNews")]
    public class ManageNewsGroupController : Controller
    {
        public static bool premission = false;
        private DB_AlmaCmsEntities db;
        private NewsGroupRepository repNewsGroup;
        NewsRepository repNews;
        public ManageNewsGroupController()
        {
            db = new DB_AlmaCmsEntities();
            repNewsGroup = new NewsGroupRepository(db);
            repNews = new NewsRepository(db);
        }

        // GET: Manage/ManageMenuItems
        public ActionResult Index(string searchString, int? page)
        {
            var vmGroupList = new List<VMNewsGroup>();
            var groupList = repNewsGroup.GetAll().ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                groupList = groupList.Where(s => s.Title.Contains(searchString)).ToList();
                ViewBag.SearchString = searchString;
            }
            foreach (var item in groupList)
            {
                vmGroupList.Add(item.toVMNewsGroup());
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            ViewBag.pageNumber = pageNumber;
            return View(vmGroupList.ToPagedList(pageNumber, pageSize));

        }

        #region Create
        public ActionResult Create()
        {




            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMNewsGroup vmGroup)
        {
         
            if (ModelState.IsValid)
            {



                repNewsGroup.Insert(vmGroup.toNewsGroup());
                Helpers.UserLogHelper.AddLog("گروه اخبار", (User.Identity.Name), "درج", vmGroup.Title);

                return RedirectToAction("Index");
            }

            return View(vmGroup);
        }
        #endregion


        #region Edit
        public ActionResult Edit(int? id)
        {
            ViewBag.ParentPages = repNewsGroup.Where(c => c.id != id).ToList();
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentpage = repNewsGroup.FindById((int)id);
            if (currentpage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(currentpage.toVMNewsGroup());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMNewsGroup vmnewsgroup)
        {
          
            if (!ModelState.IsValid)
                return View(vmnewsgroup);

            var currentPAge = repNewsGroup.FindById(vmnewsgroup.id);
            currentPAge.Comments = vmnewsgroup.Comments;
            currentPAge.Keywords = vmnewsgroup.Keywords;
            currentPAge.Title = vmnewsgroup.Title;
            //currentPAge.Name = vmMenu.Name;
        

            repNewsGroup.Update(currentPAge);
            Helpers.UserLogHelper.AddLog("گروه اخبار", (User.Identity.Name), "ویرایش", vmnewsgroup.Title);

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentPage = repNewsGroup.FindById((int)id);
            if (currentPage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);


            if (repNews.getByGroupID(currentPage.id).ToList().Count > 0)
            {

                ViewBag.ErrorMessage = "امکان حذف گروه خبر وجود ندارد.گروه دارای خبر می باشد";


            }
            return View(currentPage.toVMNewsGroup());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var CurrentItem = repNewsGroup.FindById(id);
            repNewsGroup.Delete(id);
            Helpers.UserLogHelper.AddLog("گروه اخبار", (User.Identity.Name), "حذف", CurrentItem.Title);

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