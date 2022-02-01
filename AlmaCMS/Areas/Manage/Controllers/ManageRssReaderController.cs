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
   
    public class ManageRssReaderController : Controller
    {
        // GET: Manage/ManageRssReader
        public static bool premission = false;
        private DB_AlmaCmsEntities db;
        private RSSReaderRepository repRssReader;
       
        public ManageRssReaderController()
        {
            db = new DB_AlmaCmsEntities();
            repRssReader = new RSSReaderRepository(db);
            ;
        }

        // GET: Manage/ManageMenuItems
        public ActionResult Index(string searchString, int? page)
        {
            var vmGroupList = new List<VMRSSReader>();
            var groupList = repRssReader.GetAll().ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                groupList = groupList.Where(s => s.Title.Contains(searchString)).ToList();
                ViewBag.SearchString = searchString;
            }
            foreach (var item in groupList)
            {
                vmGroupList.Add(item.toVMRssReader());
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            ViewBag.pageNumber = pageNumber;
            return View(vmGroupList.ToList());

        }

        #region Create
        public ActionResult Create()
        {




            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMRSSReader vmGroup)
        {
         
            if (ModelState.IsValid)
            {



                repRssReader.Insert(vmGroup.toRssReader());
                Helpers.UserLogHelper.AddLog("خبر خوان", (User.Identity.Name), "درج", vmGroup.Title);

                return RedirectToAction("Index");
            }

            return View(vmGroup);
        }
        #endregion


        #region Edit
        public ActionResult Edit(int? id)
        {
            ViewBag.ParentPages = repRssReader.Where(c => c.FeedID != id).ToList();
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentpage = repRssReader.FindById((int)id);
            if (currentpage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            
            return View(currentpage.toVMRssReader());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMRSSReader vmArticlegroup)
        {
          
            if (!ModelState.IsValid)
                return View(vmArticlegroup);

            var currentPAge = repRssReader.FindById(vmArticlegroup.FeedID);
            currentPAge.Title = vmArticlegroup.Title;
            currentPAge.Url = vmArticlegroup.Url;
            //currentPAge.Name = vmMenu.Name;


            repRssReader.Update(currentPAge);
            Helpers.UserLogHelper.AddLog("خبر خوان", (User.Identity.Name), "ویرایش", vmArticlegroup.Title);

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentPage = repRssReader.FindById((int)id);
            if (currentPage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);


            return View(currentPage.toVMRssReader());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var CurrentItem = repRssReader.FindById(id);
            repRssReader.Delete(id);
            Helpers.UserLogHelper.AddLog("خبر خوان", (User.Identity.Name), "حذف", CurrentItem.Title);

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