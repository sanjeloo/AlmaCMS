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

    public class ManageArticlesController : Controller
    {
        // GET: Manage/ManageArticles
        private DB_AlmaCmsEntities db;
        private ArticleRepository repArticle;
        private ArticleGroupRepositry repArticleGroup;
        public static bool premission = false;
        public ManageArticlesController()
        {
            db = new DB_AlmaCmsEntities();
            repArticle = new ArticleRepository(db);
            repArticleGroup = new ArticleGroupRepositry(db);
           
        }


        #region Index
        public ActionResult Index(int id)
        {
            var vmArticleList = new List<VMArticle>();

            var ArticleList = repArticle.getByGroupID(id);


            foreach (var item in ArticleList)
            {
                vmArticleList.Add(item.toVMArticle());
            }
            var ArticleGroup = repArticleGroup.FindById(id);
            if (ArticleGroup != null)
                ViewBag.GroupTitle = ArticleGroup.Title;
            ViewBag.GroupID = ArticleGroup.id;

            return View(vmArticleList.ToList());
        }
        #endregion

        #region Create
        public ActionResult Create(int id)
        {
            VMArticle vmArticle = new VMArticle();
            vmArticle.DateInsert = DateTime.Now;
            ViewBag.GroupID = id;
            vmArticle.ArticleGroupID = id;
            return View(vmArticle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ArticleGroupID,Title,Image,ShortDesc,ArticleContent,DateInsert,Active,Keyword")] VMArticle vmArticle)
        {
            if (ModelState.IsValid)
            {
                vmArticle.DateInsert = DateTime.Now;
                repArticle.Insert(vmArticle.toArticle());
                Helpers.UserLogHelper.AddLog("مقالات", (User.Identity.Name), "درج", vmArticle.Title);
                return RedirectToAction("Index", new { id=vmArticle.ArticleGroupID});
            }

            return View(vmArticle);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article Article = repArticle.FindById((int)id);
            if (Article == null)
            {
                return HttpNotFound();
            }
            return View(Article.toVMArticle());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ArticleGroupID,Title,Image,ShortDesc,ArticleContent,DateInsert,Active,Keyword")] VMArticle vmArticle)
        {
            if (ModelState.IsValid)
            {
                repArticle.Update(vmArticle.toArticle());
                Helpers.UserLogHelper.AddLog("مقالات", (User.Identity.Name), "ویرایش", vmArticle.Title);
                return RedirectToAction("Index", new { id = vmArticle.ArticleGroupID });
            }
            return View(vmArticle);
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article Article = repArticle.FindById((int)id);
            if (Article == null)
            {
                return HttpNotFound();
            }
            return View(Article.toVMArticle());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var CurrentItem = repArticle.FindById(id);
            
            int GroupID  = (int)repArticle.FindById(id).ArticleGroupID;
            repArticle.Delete(id);
            Helpers.UserLogHelper.AddLog("مقالات", (User.Identity.Name), "حذف ", CurrentItem.Title);
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