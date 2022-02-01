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
  
    public class ManageArticleGroupController : Controller
    {
          public static bool premission = false;
        private DB_AlmaCmsEntities db;
        private ArticleGroupRepositry repArticleGroup;
        ArticleRepository repArticle;
        public ManageArticleGroupController()
        {
            db = new DB_AlmaCmsEntities();
            repArticleGroup = new ArticleGroupRepositry(db);
            repArticle = new ArticleRepository(db);
        }

        // GET: Manage/ManageMenuItems
        public ActionResult Index(string searchString, int? page)
        {
            var vmGroupList = new List<VMArticleGroup>();
            var groupList = repArticleGroup.GetAll().ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                groupList = groupList.Where(s => s.Title.Contains(searchString)).ToList();
                ViewBag.SearchString = searchString;
            }
            foreach (var item in groupList)
            {
                vmGroupList.Add(item.toVMArticleGroup());
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
        public ActionResult Create(VMArticleGroup vmGroup)
        {
         
            if (ModelState.IsValid)
            {



                repArticleGroup.Insert(vmGroup.toArticleGroup());
                Helpers.UserLogHelper.AddLog("گروه مقالات", (User.Identity.Name), "درج ", vmGroup.Title);
                return RedirectToAction("Index");
            }

            return View(vmGroup);
        }
        #endregion


        #region Edit
        public ActionResult Edit(int? id)
        {
            ViewBag.ParentPages = repArticleGroup.Where(c => c.id != id).ToList();
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentpage = repArticleGroup.FindById((int)id);
            if (currentpage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(currentpage.toVMArticleGroup());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMArticleGroup vmArticlegroup)
        {
          
            if (!ModelState.IsValid)
                return View(vmArticlegroup);

            var currentPAge = repArticleGroup.FindById(vmArticlegroup.id);
            currentPAge.Comments = vmArticlegroup.Comments;
            currentPAge.Keywords = vmArticlegroup.Keywords;
            currentPAge.Title = vmArticlegroup.Title;
            //currentPAge.Name = vmMenu.Name;
        

            repArticleGroup.Update(currentPAge);
            Helpers.UserLogHelper.AddLog("گروه مقالات", (User.Identity.Name), "ویرایش", vmArticlegroup.Title);
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentPage = repArticleGroup.FindById((int)id);
            if (currentPage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);


            if (repArticle.getByGroupID(currentPage.id).ToList().Count > 0)
            {

                ViewBag.ErrorMessage = "امکان حذف گروه مقالات وجود ندارد.گروه دارای مقاله می باشد";


            }
            return View(currentPage.toVMArticleGroup());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var CurrentItem = repArticleGroup.FindById(id);
           repArticleGroup.Delete(id);
           Helpers.UserLogHelper.AddLog("گروه مقالات", (User.Identity.Name), "حذف", CurrentItem.Title);
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