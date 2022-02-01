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

    public class ManageNewsController : Controller
    {
        private DB_AlmaCmsEntities db;
        private NewsRepository repNews;
        private NewsGroupRepository repNewsGroup;
        public static bool premission = false;
        public ManageNewsController()
        {
            db = new DB_AlmaCmsEntities();
            repNews = new NewsRepository(db);
            repNewsGroup = new NewsGroupRepository(db);
           
        }


        #region Index
        public ActionResult Index(int id )
        {
            var vmNewsList = new List<VMNews>();

            var NewsList = repNews.getByGroupID(id);


            foreach (var item in NewsList)
            {
                vmNewsList.Add(item.ToVmNews());
            }
            var NewsGroup = repNewsGroup.FindById(id);
            if (NewsGroup != null)
                ViewBag.GroupTitle = NewsGroup.Title;
            ViewBag.GroupID = NewsGroup.id;

            return View(vmNewsList.ToList());
        }
        #endregion

        #region Create
        public ActionResult Create(int id)
        {
            VMNews vmNews = new VMNews();
            vmNews.DateInsert = DateTime.Now;
            ViewBag.GroupID = id;
            vmNews.NewsGroupID = id;
            return View(vmNews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,NewsGroupID,Title,Image,ShortDesc,NewsContent,DateInsert,Active,Keyword")] VMNews vmNews)
        {

            DateTime dtDateInsert;
            dtDateInsert = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (vmNews.DateInsert != null)
                {
                    dtDateInsert = (DateTime)vmNews.DateInsert;
                }

                vmNews.VisitCount = 1;
                vmNews.DateInsert = dtDateInsert;
                repNews.Insert(vmNews.ToNews());
                Helpers.UserLogHelper.AddLog("اخبار", (User.Identity.Name), "درج", vmNews.Title);

                return RedirectToAction("Index", new { id=vmNews.NewsGroupID});
            }

            return View(vmNews);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = repNews.FindById((int)id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news.ToVmNews());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,NewsGroupID,Title,Image,ShortDesc,NewsContent,DateInsert,Active,Keyword")] VMNews vmNews)
        {

            DateTime dtDateInsert = vmNews.DateInsert;
            if (ModelState.IsValid)
            {
                

                if (vmNews.DateInsert != null)
                {
                    dtDateInsert = (DateTime)vmNews.DateInsert;
                }

                vmNews.DateInsert = dtDateInsert;
                repNews.Update(vmNews.ToNews());
                Helpers.UserLogHelper.AddLog("اخبار", (User.Identity.Name), "ویرایش", vmNews.Title);

                return RedirectToAction("Index", new { id = vmNews.NewsGroupID });
            }
            return View(vmNews);
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = repNews.FindById((int)id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news.ToVmNews());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var CurrentItem = repNews.FindById(id);
            int GroupID  = (int)repNews.FindById(id).NewsGroupID;
            repNews.Delete(id);
            Helpers.UserLogHelper.AddLog("اخبار", (User.Identity.Name), "حذف", CurrentItem.Title);

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

       
    }
}